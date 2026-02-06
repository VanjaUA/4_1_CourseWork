import { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Alert } from 'react-bootstrap';
import api from '../api/http';
import { useAuth } from '../auth/AuthContext';

const Products = () => {
    const [products, setProducts] = useState([]);
    const [show, setShow] = useState(false);
    const [formData, setFormData] = useState({ sku: '', name: '', unit: 'pcs', unitPrice: 0, minStock: 0 });
    const [error, setError] = useState('');
    const { isAdmin } = useAuth();

    const fetchProducts = async () => {
        try {
            const response = await api.get('/Products');
            setProducts(response.data);
        } catch (err) {
            console.error(err);
        }
    };

    useEffect(() => {
        fetchProducts();
    }, []);

    const handleDelete = async (id) => {
        if (confirm('Are you sure?')) {
            try {
                await api.delete(`/Products/${id}`);
                fetchProducts();
            } catch (err) {
                console.error(err);
            }
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await api.post('/Products', formData);
            setShow(false);
            setFormData({ sku: '', name: '', unit: 'pcs', unitPrice: 0, minStock: 0 });
            fetchProducts();
        } catch (err) {
            setError('Failed to create product');
            console.error(err);
        }
    };

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Products</h2>
                <Button onClick={() => setShow(true)}>Add Product</Button>
            </div>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>SKU</th>
                        <th>Name</th>
                        <th>Unit</th>
                        <th>Price</th>
                        <th>Min Stock</th>
                        {isAdmin && <th>Actions</th>}
                    </tr>
                </thead>
                <tbody>
                    {products.map(p => (
                        <tr key={p.id}>
                            <td>{p.sku}</td>
                            <td>{p.name}</td>
                            <td>{p.unit}</td>
                            <td>{p.unitPrice}</td>
                            <td>{p.minStock}</td>
                            {isAdmin && (
                                <td>
                                    <Button variant="danger" size="sm" onClick={() => handleDelete(p.id)}>Delete</Button>
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </Table>

            <Modal show={show} onHide={() => setShow(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Product</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {error && <Alert variant="danger">{error}</Alert>}
                    <Form onSubmit={handleSubmit}>
                        <Form.Group className="mb-3">
                            <Form.Label>SKU</Form.Label>
                            <Form.Control required onChange={e => setFormData({ ...formData, sku: e.target.value })} />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Name</Form.Label>
                            <Form.Control required onChange={e => setFormData({ ...formData, name: e.target.value })} />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Unit</Form.Label>
                            <Form.Control required value={formData.unit} onChange={e => setFormData({ ...formData, unit: e.target.value })} />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Price</Form.Label>
                            <Form.Control type="number" required onChange={e => setFormData({ ...formData, unitPrice: e.target.value })} />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Min Stock</Form.Label>
                            <Form.Control type="number" required onChange={e => setFormData({ ...formData, minStock: e.target.value })} />
                        </Form.Group>
                        <Button type="submit" variant="primary">Save</Button>
                    </Form>
                </Modal.Body>
            </Modal>
        </div>
    );
};

export default Products;
