import React, { useState } from 'react';
import { Link, NavLink, useNavigate } from 'react-router-dom';
import { FiShoppingCart, FiUser, FiSearch, FiMenu, FiX, FiLogOut, FiPackage } from 'react-icons/fi';
import { useAuth } from '../../contexts/AuthContext';
import { useCart } from '../../contexts/CartContext';
import './Header.css';

const Header = () => {
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [isSearchOpen, setIsSearchOpen] = useState(false);
    const [searchQuery, setSearchQuery] = useState('');

    const { user, signed, signOut } = useAuth();
    const { cart } = useCart();
    const navigate = useNavigate();

    // === CORREÇÃO TÉCNICA (Mantida) ===
    const cartItems = cart?.items || [];
    const itemCount = cartItems.reduce((acc, item) => acc + item.quantity, 0);

    const handleSearch = (e) => {
        e.preventDefault();
        if (searchQuery.trim()) {
            navigate(`/products?search=${encodeURIComponent(searchQuery)}`);
            setSearchQuery('');
            setIsSearchOpen(false);
        }
    };

    const handleLogout = () => {
        signOut();
        navigate('/');
    };

    return (
        <header className="header">
            <div className="header-container">
                {/* Logo */}
                <Link to="/" className="logo">
                    <span className="logo-text">LUXE</span>
                    <span className="logo-accent">Store</span>
                </Link>

                {/* Navigation - MENUS RESTAURADOS AQUI */}
                <nav className={`nav ${isMenuOpen ? 'nav-open' : ''}`}>
                    <NavLink to="/" className="nav-link" onClick={() => setIsMenuOpen(false)}>
                        Home
                    </NavLink>

                    <NavLink to="/products" className="nav-link" onClick={() => setIsMenuOpen(false)}>
                        Produtos
                    </NavLink>

                    {/* Restaurado: Visível para todos */}
                    <NavLink to="/categories" className="nav-link" onClick={() => setIsMenuOpen(false)}>
                        Categorias
                    </NavLink>

                    {/* Restaurado: Menu Sobre */}
                    <NavLink to="/about" className="nav-link" onClick={() => setIsMenuOpen(false)}>
                        Sobre
                    </NavLink>
                </nav>

                {/* Actions */}
                <div className="header-actions">
                    {/* Search Toggle */}
                    <button
                        className="action-btn"
                        onClick={() => setIsSearchOpen(!isSearchOpen)}
                        aria-label="Buscar"
                    >
                        <FiSearch />
                    </button>

                    {/* Cart */}
                    <Link to="/cart" className="action-btn cart-btn">
                        <FiShoppingCart />
                        {itemCount > 0 && <span className="cart-count">{itemCount}</span>}
                    </Link>

                    {/* User Area */}
                    {signed ? (
                        <div className="user-menu">
                            <button className="action-btn user-btn">
                                <FiUser />
                            </button>

                            <div className="user-dropdown">
                                <div className="user-info">
                                    <span className="user-name">Olá, {user?.name || user?.firstName}</span>
                                    <span className="user-email">{user?.email}</span>
                                </div>

                                <div className="dropdown-divider"></div>

                                {/* Links extras do usuário se quiser restaurar também */}
                                {/* <Link to="/orders" className="dropdown-item">
                  <FiPackage /> Meus Pedidos
                </Link> */}

                                <button onClick={handleLogout} className="dropdown-item logout">
                                    <FiLogOut /> Sair
                                </button>
                            </div>
                        </div>
                    ) : (
                        <Link to="/login" className="btn-login-header">
                            Entrar
                        </Link>
                    )}

                    {/* Mobile Menu Toggle */}
                    <button
                        className="menu-toggle"
                        onClick={() => setIsMenuOpen(!isMenuOpen)}
                        aria-label="Menu"
                    >
                        {isMenuOpen ? <FiX /> : <FiMenu />}
                    </button>
                </div>
            </div>

            {/* Search Bar */}
            <div className={`search-bar ${isSearchOpen ? 'search-open' : ''}`}>
                <form onSubmit={handleSearch} className="search-form">
                    <input
                        type="text"
                        placeholder="O que você procura?"
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        className="search-input"
                    />
                    <button type="submit" className="search-submit">
                        <FiSearch />
                    </button>
                </form>
            </div>
        </header>
    );
};

export default Header;