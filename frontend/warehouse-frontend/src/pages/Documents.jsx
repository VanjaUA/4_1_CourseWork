import { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Alert, Row, Col } from 'react-bootstrap';
import api from '../api/http';
import { useAuth } from '../auth/AuthContext';

const Documents = () => {
    const [documents, setDocuments] = useState([]);
    const [show, setShow] = useState(false);
    const { isAdmin } = useAuth();

    // Data for dropdowns
    const [warehouses, setWarehouses] = useState([]);
    const [partners, setPartners] = useState([]);
    const [products, setProducts] = useState([]);

    const [formData, setFormData] = useState({
        type: 1,
        warehouseId: '',
        partnerId: '',
        items: []
    });

    // Local state for adding items
    const [currentItem, setCurrentItem] = useState({ productId: '', quantity: 1 });

    const [error, setError] = useState('');

    const fetchData = async () => {
        try {
            const [docsRes, whRes, partRes, prodRes] = await Promise.all([
                api.get('/Documents'),
                api.get('/Warehouses'),
                api.get('/Partners'),
                api.get('/Products')
            ]);
            setDocuments(docsRes.data);
            setWarehouses(whRes.data);
            setPartners(partRes.data);
            setProducts(prodRes.data);
        } catch (err) {
            console.error(err);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    const handleDelete = async (id) => {
        if (confirm('Are you sure?')) {
            try {
                await api.delete(`/Documents/${id}`);
                fetchData();
            } catch (err) {
                console.error(err);
            }
        }
    };

    const addItem = () => {
        if (!currentItem.productId || currentItem.quantity <= 0) return;
        const product = products.find(p => p.id === currentItem.productId);

        setFormData({
            ...formData,
            items: [...formData.items, { ...currentItem, productName: product?.name || 'Unknown' }]
        });
        setCurrentItem({ productId: '', quantity: 1 });
    };

    const removeItem = (idx) => {
        const newItems = [...formData.items];
        newItems.splice(idx, 1);
        setFormData({ ...formData, items: newItems });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        // Validation
        if (!formData.warehouseId) { setError('Select a warehouse'); return; }
        if (formData.type !== 3 && !formData.partnerId) { setError('Select a partner'); return; }
        if (formData.items.length === 0) { setError('Add at least one item'); return; }

        try {
            const payload = {
                type: parseInt(formData.type),
                warehouseId: formData.warehouseId,
                partnerId: formData.partnerId || null,
                items: formData.items.map(i => ({ productId: i.productId, quantity: parseInt(i.quantity) }))
            };

            await api.post('/Documents', payload);
            setShow(false);
            setFormData({ type: 1, warehouseId: '', partnerId: '', items: [] });
            fetchData();
        } catch (err) {
            console.error(err);
            setError('Failed to create document');
        }
    };

    const getTypeName = (type) => {
        switch (type) {
            case 1: return 'Receipt';
            case 2: return 'Shipment';
            case 3: return 'WriteOff';
            default: return type;
        }
    };

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Documents</h2>
                <Button onClick={() => setShow(true)}>Create Document</Button>
            </div>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Type</th>
                        <th>Number</th>
                        <th>Warehouse</th>
                        <th>Partner</th>
                        <th>Date</th>
                        {isAdmin && <th>Actions</th>}
                    </tr>
                </thead>
                <tbody>
                    {documents.map(d => (
                        <tr key={d.id}>
                            <td>{getTypeName(d.type)}</td>
                            <td>{d.number}</td>
                            <td>{d.warehouseName}</td>
                            <td>{d.partnerName || '-'}</td>
                            <td>{new Date(d.createdAt).toLocaleString()}</td>
                            {isAdmin && (
                                <td>
                                    <Button variant="danger" size="sm" onClick={() => handleDelete(d.id)}>Delete</Button>
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </Table>

            <Modal show={show} onHide={() => setShow(false)} size="lg">
                <Modal.Header closeButton>
                    <Modal.Title>Create Document</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {error && <Alert variant="danger">{error}</Alert>}
                    <Form onSubmit={handleSubmit}>
                        <Row>
                            <Col>
                                <Form.Group className="mb-3">
                                    <Form.Label>Type</Form.Label>
                                    <Form.Select value={formData.type} onChange={e => setFormData({ ...formData, type: parseInt(e.target.value) })}>
                                        <option value="1">Receipt (In)</option>
                                        <option value="2">Shipment (Out)</option>
                                        <option value="3">WriteOff (Loss)</option>
                                    </Form.Select>
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group className="mb-3">
                                    <Form.Label>Warehouse</Form.Label>
                                    <Form.Select value={formData.warehouseId} onChange={e => setFormData({ ...formData, warehouseId: e.target.value })}>
                                        <option value="">Select Warehouse...</option>
                                        {warehouses.map(w => <option key={w.id} value={w.id}>{w.name}</option>)}
                                    </Form.Select>
                                </Form.Group>
                            </Col>
                        </Row>

                        {formData.type !== 3 && (
                            <Form.Group className="mb-3">
                                <Form.Label>Partner</Form.Label>
                                <Form.Select value={formData.partnerId} onChange={e => setFormData({ ...formData, partnerId: e.target.value })}>
                                    <option value="">Select Partner...</option>
                                    {partners.map(p => <option key={p.id} value={p.id}>{p.name} ({p.type === 1 ? 'Supplier' : 'Customer'})</option>)}
                                </Form.Select>
                            </Form.Group>
                        )}

                        <hr />
                        <h5>Items</h5>
                        <Row className="align-items-end mb-3">
                            <Col md={6}>
                                <Form.Label>Product</Form.Label>
                                <Form.Select value={currentItem.productId} onChange={e => setCurrentItem({ ...currentItem, productId: e.target.value })}>
                                    <option value="">Select Product...</option>
                                    {products.map(p => <option key={p.id} value={p.id}>{p.name} ({p.sku})</option>)}
                                </Form.Select>
                            </Col>
                            <Col md={3}>
                                <Form.Label>Quantity</Form.Label>
                                <Form.Control type="number" min="1" value={currentItem.quantity} onChange={e => setCurrentItem({ ...currentItem, quantity: e.target.value })} />
                            </Col>
                            <Col md={3}>
                                <Button variant="secondary" onClick={addItem}>Add Line</Button>
                            </Col>
                        </Row>

                        <Table size="sm">
                            <thead>
                                <tr>
                                    <th>Product</th>
                                    <th>Qty</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                {formData.items.map((item, idx) => (
                                    <tr key={idx}>
                                        <td>{item.productName}</td>
                                        <td>{item.quantity}</td>
                                        <td><Button variant="outline-danger" size="sm" onClick={() => removeItem(idx)}>X</Button></td>
                                    </tr>
                                ))}
                            </tbody>
                        </Table>

                        <div className="d-flex justify-content-end">
                            <Button type="submit" variant="primary">Create Document</Button>
                        </div>
                    </Form>
                </Modal.Body>
            </Modal>
        </div>
    );
};

export default Documents;
