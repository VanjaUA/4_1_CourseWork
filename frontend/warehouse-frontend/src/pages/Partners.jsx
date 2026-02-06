import { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Alert } from 'react-bootstrap';
import api from '../api/http';
import { useAuth } from '../auth/AuthContext';

const Partners = () => {
    const [items, setItems] = useState([]);
    const [show, setShow] = useState(false);
    const [formData, setFormData] = useState({ name: '', contactInfo: '', type: 1 });
    const [error, setError] = useState('');
    const { isAdmin } = useAuth();

    const fetchItems = async () => {
        try {
            const response = await api.get('/Partners');
            setItems(response.data);
        } catch (err) {
            console.error(err);
        }
    };

    useEffect(() => {
        fetchItems();
    }, []);

    const handleDelete = async (id) => {
        if (confirm('Are you sure?')) {
            try {
                await api.delete(`/Partners/${id}`);
                fetchItems();
            } catch (err) {
                console.error(err);
            }
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await api.post('/Partners', { ...formData, type: parseInt(formData.type) });
            setShow(false);
            setFormData({ name: '', contactInfo: '', type: 1 });
            fetchItems();
        } catch (err) {
            setError('Failed to create partner');
        }
    };

    const getTypeName = (type) => type === 1 ? 'Supplier' : (type === 2 ? 'Customer' : 'Unknown');

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Partners</h2>
                {isAdmin && <Button onClick={() => setShow(true)}>Add Partner</Button>}
            </div>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Type</th>
                        <th>Contact</th>
                        {isAdmin && <th>Actions</th>}
                    </tr>
                </thead>
                <tbody>
                    {items.map(item => (
                        <tr key={item.id}>
                            <td>{item.name}</td>
                            <td>{getTypeName(item.type)}</td>
                            <td>{item.contactInfo}</td>
                            {isAdmin && (
                                <td>
                                    <Button variant="danger" size="sm" onClick={() => handleDelete(item.id)}>Delete</Button>
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </Table>

            <Modal show={show} onHide={() => setShow(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Partner</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {error && <Alert variant="danger">{error}</Alert>}
                    <Form onSubmit={handleSubmit}>
                        <Form.Group className="mb-3">
                            <Form.Label>Name</Form.Label>
                            <Form.Control required onChange={e => setFormData({ ...formData, name: e.target.value })} />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Type</Form.Label>
                            <Form.Select value={formData.type} onChange={e => setFormData({ ...formData, type: e.target.value })}>
                                <option value="1">Supplier</option>
                                <option value="2">Customer</option>
                            </Form.Select>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Contact Info</Form.Label>
                            <Form.Control required onChange={e => setFormData({ ...formData, contactInfo: e.target.value })} />
                        </Form.Group>
                        <Button type="submit" variant="primary">Save</Button>
                    </Form>
                </Modal.Body>
            </Modal>
        </div>
    );
};

export default Partners;
