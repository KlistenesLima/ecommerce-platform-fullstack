import { useState } from 'react';
import { Outlet, NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import {
    FiGrid, FiBox, FiShoppingBag, FiUsers, FiLogOut, FiMenu, FiTag
} from 'react-icons/fi'; // Adicionei FiTag
import './AdminLayout.css';

const AdminLayout = () => {
    const { user, signOut } = useAuth();
    const navigate = useNavigate();
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);

    const handleSignOut = () => {
        signOut();
        navigate('/login');
    };

    const toggleSidebar = () => setIsSidebarOpen(!isSidebarOpen);

    return (
        <div className="admin-container">
            {/* Overlay para fechar menu no mobile */}
            {isSidebarOpen && <div className="sidebar-overlay" onClick={toggleSidebar}></div>}

            {/* SIDEBAR */}
            <aside className={`sidebar ${isSidebarOpen ? 'open' : ''}`}>
                <div className="sidebar-header">
                    <h2>Luxe<span className="text-gold">Store</span></h2>
                    <p className="admin-badge">Painel Admin</p>
                </div>

                <nav className="sidebar-nav">
                    <NavLink to="/admin" end className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'} onClick={() => setIsSidebarOpen(false)}>
                        <FiGrid /> <span>Dashboard</span>
                    </NavLink>

                    <NavLink to="/admin/orders" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'} onClick={() => setIsSidebarOpen(false)}>
                        <FiShoppingBag /> <span>Pedidos</span>
                    </NavLink>

                    <NavLink to="/admin/products" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'} onClick={() => setIsSidebarOpen(false)}>
                        <FiBox /> <span>Produtos</span>
                    </NavLink>

                    {/* === NOVO MENU DE CATEGORIAS === */}
                    <NavLink to="/admin/categories" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'} onClick={() => setIsSidebarOpen(false)}>
                        <FiTag /> <span>Categorias</span>
                    </NavLink>
                    {/* =============================== */}

                    <NavLink to="/admin/users" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'} onClick={() => setIsSidebarOpen(false)}>
                        <FiUsers /> <span>Usuários</span>
                    </NavLink>
                </nav>

                <div className="sidebar-footer">
                    <div className="user-info">
                        <div className="avatar">{user?.name?.charAt(0)}</div>
                        <div className="user-details-text">
                            <p className="user-name">{user?.name}</p>
                            <p className="user-role">Administrador</p>
                        </div>
                    </div>
                    <button onClick={handleSignOut} className="btn-logout" title="Sair">
                        <FiLogOut />
                    </button>
                </div>
            </aside>

            {/* CONTEÚDO PRINCIPAL */}
            <main className="main-content">
                <header className="top-bar">
                    <button className="menu-toggle" onClick={toggleSidebar}>
                        <FiMenu />
                    </button>
                    <h2 className="page-title">Área Administrativa</h2>
                </header>

                <div className="content-wrapper">
                    <Outlet />
                </div>
            </main>
        </div>
    );
};

export default AdminLayout;