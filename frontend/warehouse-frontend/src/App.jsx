import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './auth/AuthContext';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import ProtectedRoute from './components/ProtectedRoute';
import Products from './pages/Products';
import Warehouses from './pages/Warehouses';
import Partners from './pages/Partners';
import Documents from './pages/Documents';
import Stock from './pages/Stock';
import Notifications from './pages/Notifications';

function App() {
    return (
        <AuthProvider>
            <Router>
                <Routes>
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />

                    <Route path="/" element={<ProtectedRoute />}>
                        <Route index element={<Dashboard />} />
                        <Route path="products" element={<Products />} />
                        <Route path="warehouses" element={<Warehouses />} />
                        <Route path="partners" element={<Partners />} />
                        <Route path="documents" element={<Documents />} />
                        <Route path="stock" element={<Stock />} />
                        <Route path="notifications" element={<Notifications />} />
                    </Route>

                    <Route path="*" element={<Navigate to="/" />} />
                </Routes>
            </Router>
        </AuthProvider>
    );
}

export default App;
