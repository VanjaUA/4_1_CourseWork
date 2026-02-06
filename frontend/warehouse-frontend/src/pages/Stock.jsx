import { useState, useEffect } from 'react';
import { Table, Form, Card, Row, Col, Button } from 'react-bootstrap';
import api from '../api/http';

const Stock = () => {
    const [warehouses, setWarehouses] = useState([]);
    const [selectedWarehouse, setSelectedWarehouse] = useState('');
    const [stock, setStock] = useState([]);
    const [valuation, setValuation] = useState(null);

    useEffect(() => {
        // Fetch warehouses on mount
        api.get('/Warehouses').then(res => setWarehouses(res.data));
    }, []);

    const fetchReports = async () => {
        if (!selectedWarehouse) return;
        try {
            const stockRes = await api.get(`/Stock?warehouseId=${selectedWarehouse}`);
            setStock(stockRes.data);

            const valRes = await api.get(`/Stock/valuation?warehouseId=${selectedWarehouse}`);
            setValuation(valRes.data);
        } catch (err) {
            console.error(err);
        }
    };

    useEffect(() => {
        if (selectedWarehouse) {
            fetchReports();
        } else {
            setStock([]);
            setValuation(null);
        }
    }, [selectedWarehouse]);

    return (
        <div>
            <h2 className="mb-4">Reports</h2>

            <Card className="mb-4">
                <Card.Body>
                    <Form.Group>
                        <Form.Label>Select Warehouse for Report</Form.Label>
                        <Form.Select value={selectedWarehouse} onChange={e => setSelectedWarehouse(e.target.value)}>
                            <option value="">-- Choose Warehouse --</option>
                            {warehouses.map(w => <option key={w.id} value={w.id}>{w.name}</option>)}
                        </Form.Select>
                    </Form.Group>
                </Card.Body>
            </Card>

            {selectedWarehouse && (
                <Row>
                    <Col md={12} className="mb-4">
                        <h4>Stock Level</h4>
                        <Table striped bordered hover>
                            <thead>
                                <tr>
                                    <th>Product</th>
                                    <th>SKU</th>
                                    <th>Quantity</th>
                                </tr>
                            </thead>
                            <tbody>
                                {stock.map((item, idx) => (
                                    <tr key={idx}>
                                        <td>{item.productName}</td>
                                        <td>{item.sku}</td>
                                        <td>{item.quantity}</td>
                                    </tr>
                                ))}
                                {stock.length === 0 && <tr><td colSpan="3">No stock found.</td></tr>}
                            </tbody>
                        </Table>
                    </Col>

                    <Col md={12}>
                        <h4>Valuation Report</h4>
                        {valuation ? (
                            <Card>
                                <Card.Body>
                                    <Card.Title>Total Value: {valuation.totalValue.toFixed(2)}</Card.Title>
                                    <Card.Subtitle className="mb-2 text-muted">Generated At: {new Date(valuation.generatedAt).toLocaleString()}</Card.Subtitle>

                                    <Table size="sm" className="mt-3">
                                        <thead>
                                            <tr>
                                                <th>Product</th>
                                                <th>Quantity</th>
                                                <th>Unit Price</th>
                                                <th>Total</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {valuation.items.map((v, idx) => (
                                                <tr key={idx}>
                                                    <td>{v.productName}</td>
                                                    <td>{v.quantity}</td>
                                                    <td>{v.unitPrice.toFixed(2)}</td>
                                                    <td>{v.totalValue.toFixed(2)}</td>
                                                </tr>
                                            ))}
                                        </tbody>
                                    </Table>
                                </Card.Body>
                            </Card>
                        ) : (
                            <p>Loading valuation...</p>
                        )}
                    </Col>
                </Row>
            )}
        </div>
    );
};

export default Stock;
