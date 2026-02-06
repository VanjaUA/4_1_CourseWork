import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext';
import NavBar from './NavBar';
import { Container } from 'react-bootstrap';

const ProtectedRoute = () => {
    const { user } = useAuth();

    if (!user) {
        return <Navigate to="/login" replace />;
    }

    return (
        <>
            <NavBar />
            <Container className="mt-4">
                <Outlet />
            </Container>
        </>
    );
};

export default ProtectedRoute;
