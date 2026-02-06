import { Row, Col, Card, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const Dashboard = () => {
    return (
        <div>
            <h1 className="mb-4">Dashboard</h1>
            <Row xs={1} md={2} lg={3} className="g-4">
                <Col>
                    <Card>
                        <Card.Body>
                            <Card.Title>Products</Card.Title>
                            <Card.Text>Manage inventory items.</Card.Text>
                            <Button as={Link} to="/products" variant="primary">Go to Products</Button>
                        </Card.Body>
                    </Card>
                </Col>
                <Col>
                    <Card>
                        <Card.Body>
                            <Card.Title>Warehouses</Card.Title>
                            <Card.Text>Manage warehouse locations.</Card.Text>
                            <Button as={Link} to="/warehouses" variant="primary">Go to Warehouses</Button>
                        </Card.Body>
                    </Card>
                </Col>
                <Col>
                    <Card>
                        <Card.Body>
                            <Card.Title>Documents</Card.Title>
                            <Card.Text>Create receipts and shipments.</Card.Text>
                            <Button as={Link} to="/documents" variant="primary">Go to Documents</Button>
                        </Card.Body>
                    </Card>
                </Col>
                <Col>
                    <Card>
                        <Card.Body>
                            <Card.Title>Partners</Card.Title>
                            <Card.Text>Manage suppliers and customers.</Card.Text>
                            <Button as={Link} to="/partners" variant="primary">Go to Partners</Button>
                        </Card.Body>
                    </Card>
                </Col>
                <Col>
                    <Card>
                        <Card.Body>
                            <Card.Title>Reports</Card.Title>
                            <Card.Text>View stock and valuation reports.</Card.Text>
                            <Button as={Link} to="/stock" variant="primary">View Reports</Button>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>
        </div>
    );
};

export default Dashboard;
