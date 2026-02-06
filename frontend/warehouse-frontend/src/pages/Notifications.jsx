import { useState, useEffect } from 'react';
import { ListGroup, Badge, Button } from 'react-bootstrap';
import api from '../api/http';

const Notifications = () => {
    const [notifications, setNotifications] = useState([]);

    const fetchNotes = async () => {
        try {
            const res = await api.get('/Notifications/unread');
            setNotifications(res.data);
        } catch (err) {
            console.error(err);
        }
    };

    useEffect(() => {
        fetchNotes();
    }, []);

    const markRead = async (id) => {
        try {
            await api.put(`/Notifications/${id}/read`);
            fetchNotes();
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <div>
            <h2 className="mb-4">Unread Notifications</h2>
            {notifications.length === 0 ? (
                <p>No new notifications.</p>
            ) : (
                <ListGroup>
                    {notifications.map(n => (
                        <ListGroup.Item key={n.id} className="d-flex justify-content-between align-items-center">
                            <div>
                                <p className="mb-1">{n.message}</p>
                                <small className="text-muted">{new Date(n.createdAt).toLocaleString()}</small>
                            </div>
                            <Button size="sm" variant="outline-success" onClick={() => markRead(n.id)}>Mark Read</Button>
                        </ListGroup.Item>
                    ))}
                </ListGroup>
            )}
        </div>
    );
};

export default Notifications;
