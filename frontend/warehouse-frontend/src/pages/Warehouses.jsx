import { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Alert } from 'react-bootstrap';
import api from '../api/http';
import { useAuth } from '../auth/AuthContext';

const Warehouses = () => {
    const [items, setItems] = useState([]);
    const [show, setShow] = useState(false);
    const [formData, setFormData] = useState({ name: '', location: '' });
    const [error, setError] = useState('');
    const { isAdmin } = useAuth();

    const fetchItems = async () => {
        try {
            const response = await api.get('/Warehouses');
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
                await api.delete(`/Warehouses/${id}`);
                fetchItems();
            } catch (err) {
                console.error(err);
            }
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await api.post('/Warehouses', formData);
            setShow(false);
            setFormData({ name: '', location: '' });
            fetchItems();
        } catch (err) {
            setError('Failed to create warehouse');
        }
    };

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Warehouses</h2>
                <Button onClick={() => setShow(true)}>Add Warehouse</Button>
            </div>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Location</th>
                        {isAdmin && <th>Actions</th>}
                    </tr>
                </thead>
                <tbody>
                    {items.map(item => (
                        <tr key={item.id}>
                            <td>{item.name}</td>
                            <td>{item.location}</td>
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
                    <Modal.Title>Add Warehouse</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {error && <Alert variant="danger">{error}</Alert>}
                    <Form onSubmit={handleSubmit}>
                        <Form.Group className="mb-3">
                            <Form.Label>Name</Form.Label>
                            <Form.Control required onChange={e => setFormData({ ...formData, name: e.target.value })} />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Location</Form.Label>
                            <Form.Control required onChange={e => setFormData({ ...formData, location: e.target.value })} />
                        </Form.Group>
                        <Button type="submit" variant="primary">Save</Button>
                    </Form>
                </Modal.Body>
            </Modal>
        </div>
    );
};

export default Warehouses;
