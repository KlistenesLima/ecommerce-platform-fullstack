import { Outlet, Navigate, Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { FiGrid, FiBox, FiList, FiUsers, FiLogOut, FiFolder } from 'react-icons/fi'; // Adicionei FiFolder
import './AdminLayout.css';

const AdminLayout = () => {
    const { user, loading, signOut } = useAuth();
    const navigate = useNavigate();

    // === ADICIONE ISSO PARA VER NO CONSOLE (F12) ===
    console.log("AdminLayout - Loading:", loading);
    console.log("AdminLayout - User:", user);
    // ==============================================

    // 1. Enquanto carrega, mostra tela branca ou spinner (não redireciona ainda)
    if (loading) {
        return <div className="loading-screen">Carregando...</div>;
    }

    // 2. Se não tem usuário logado, vai para login
    if (!user) {
        return <Navigate to="/login" />;
    }

    // 3. Se tem usuário mas NÃO é admin, chuta para a Home
    if (user.role !== 'admin') {
        return <Navigate to="/" />;
    }

    const handleLogout = () => {
        signOut();
        navigate('/login');
    };

    return (
        <div className="admin-layout">
            <aside className="sidebar">
                <div className="sidebar-header">
                    <h3>Admin Panel</h3>
                </div>
                <nav className="sidebar-nav">
                    <Link to="/admin" className="nav-item">
                        <FiGrid /> Dashboard
                    </Link>
                    <Link to="/admin/products" className="nav-item">
                        <FiBox /> Produtos
                    </Link>
                    <Link to="/admin/categories" className="nav-item"> {/* Link Novo */}
                        <FiFolder /> Categorias
                    </Link>
                    <Link to="/admin/orders" className="nav-item">
                        <FiList /> Pedidos
                    </Link>
                    <Link to="/admin/users" className="nav-item">
                        <FiUsers /> Usuários
                    </Link>
                </nav>
                <div className="sidebar-footer">
                    <div className="user-info">
                        <span>{user.name}</span>
                        <small>Administrador</small>
                    </div>
                    <button onClick={handleLogout} className="logout-btn">
                        <FiLogOut />
                    </button>
                </div>
            </aside>
            <main className="admin-content">
                <Outlet />
            </main>
        </div>
    );
};

export default AdminLayout;