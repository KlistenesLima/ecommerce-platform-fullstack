import { useState } from 'react';
import { Link, NavLink, useNavigate } from 'react-router-dom';
import { FiShoppingCart, FiUser, FiSearch, FiMenu, FiX, FiLogOut, FiPackage } from 'react-icons/fi';
import { useAuth } from '../../contexts/AuthContext';
import { useCart } from '../../contexts/CartContext';
import './Header.css';

const Header = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isSearchOpen, setIsSearchOpen] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');
  const { user, isAuthenticated, logout } = useAuth();
  const { itemCount } = useCart();
  const navigate = useNavigate();

  const handleSearch = (e) => {
    e.preventDefault();
    if (searchQuery.trim()) {
      navigate(`/products?search=${encodeURIComponent(searchQuery)}`);
      setSearchQuery('');
      setIsSearchOpen(false);
    }
  };

  const handleLogout = () => {
    logout();
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

        {/* Navigation */}
        <nav className={`nav ${isMenuOpen ? 'nav-open' : ''}`}>
          <NavLink to="/" className="nav-link" onClick={() => setIsMenuOpen(false)}>
            Home
          </NavLink>
          <NavLink to="/products" className="nav-link" onClick={() => setIsMenuOpen(false)}>
            Produtos
          </NavLink>
          <NavLink to="/categories" className="nav-link" onClick={() => setIsMenuOpen(false)}>
            Categorias
          </NavLink>
          <NavLink to="/about" className="nav-link" onClick={() => setIsMenuOpen(false)}>
            Sobre
          </NavLink>
        </nav>

        {/* Actions */}
        <div className="header-actions">
          {/* Search */}
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

          {/* User */}
          {isAuthenticated ? (
            <div className="user-menu">
              <button className="action-btn user-btn">
                <FiUser />
              </button>
              <div className="user-dropdown">
                <div className="user-info">
                  <span className="user-name">{user?.firstName}</span>
                  <span className="user-email">{user?.email}</span>
                </div>
                <div className="dropdown-divider"></div>
                <Link to="/profile" className="dropdown-item">
                  <FiUser /> Meu Perfil
                </Link>
                <Link to="/orders" className="dropdown-item">
                  <FiPackage /> Meus Pedidos
                </Link>
                <div className="dropdown-divider"></div>
                <button onClick={handleLogout} className="dropdown-item logout">
                  <FiLogOut /> Sair
                </button>
              </div>
            </div>
          ) : (
            <Link to="/login" className="btn btn-primary btn-sm">
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
            placeholder="Buscar produtos..."
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
