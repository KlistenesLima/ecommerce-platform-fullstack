import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FiPackage, FiShoppingCart, FiUsers, FiDollarSign, FiArrowRight } from 'react-icons/fi';
import './Dashboard.css';

const Dashboard = () => {
  const [stats, setStats] = useState({
    totalProducts: 0,
    totalOrders: 0,
    totalUsers: 0,
    totalRevenue: 0
  });
  const [recentOrders, setRecentOrders] = useState([]);

  useEffect(() => {
    // Mock data - conectar com API depois
    setStats({
      totalProducts: 48,
      totalOrders: 156,
      totalUsers: 89,
      totalRevenue: 45890.50
    });

    setRecentOrders([
      { id: '001', customer: 'João Silva', total: 299.90, status: 'Pendente', date: '2026-01-23' },
      { id: '002', customer: 'Maria Santos', total: 459.90, status: 'Enviado', date: '2026-01-23' },
      { id: '003', customer: 'Pedro Costa', total: 189.90, status: 'Entregue', date: '2026-01-22' },
      { id: '004', customer: 'Ana Oliveira', total: 789.90, status: 'Processando', date: '2026-01-22' },
    ]);
  }, []);

  const formatPrice = (price) => {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(price);
  };

  return (
    <div className="dashboard">
      <div className="page-header">
        <h1>Dashboard</h1>
        <p>Bem-vindo ao painel administrativo</p>
      </div>

      {/* Stats Cards */}
      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-icon products"><FiPackage /></div>
          <div className="stat-info">
            <span className="stat-value">{stats.totalProducts}</span>
            <span className="stat-label">Produtos</span>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon orders"><FiShoppingCart /></div>
          <div className="stat-info">
            <span className="stat-value">{stats.totalOrders}</span>
            <span className="stat-label">Pedidos</span>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon users"><FiUsers /></div>
          <div className="stat-info">
            <span className="stat-value">{stats.totalUsers}</span>
            <span className="stat-label">Usuários</span>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon revenue"><FiDollarSign /></div>
          <div className="stat-info">
            <span className="stat-value">{formatPrice(stats.totalRevenue)}</span>
            <span className="stat-label">Receita Total</span>
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="quick-actions">
        <h3>Ações Rápidas</h3>
        <div className="actions-grid">
          <Link to="/admin/products/new" className="action-card">
            <FiPackage />
            <span>Novo Produto</span>
          </Link>
          <Link to="/admin/categories" className="action-card">
            <FiPackage />
            <span>Nova Categoria</span>
          </Link>
          <Link to="/admin/orders" className="action-card">
            <FiShoppingCart />
            <span>Ver Pedidos</span>
          </Link>
        </div>
      </div>

      {/* Recent Orders */}
      <div className="recent-section">
        <div className="section-header">
          <h3>Pedidos Recentes</h3>
          <Link to="/admin/orders" className="view-all">Ver todos <FiArrowRight /></Link>
        </div>
        <div className="table-container">
          <table className="data-table">
            <thead>
              <tr>
                <th>Pedido</th>
                <th>Cliente</th>
                <th>Total</th>
                <th>Status</th>
                <th>Data</th>
              </tr>
            </thead>
            <tbody>
              {recentOrders.map(order => (
                <tr key={order.id}>
                  <td>#{order.id}</td>
                  <td>{order.customer}</td>
                  <td className="price">{formatPrice(order.total)}</td>
                  <td><span className={`badge ${order.status.toLowerCase()}`}>{order.status}</span></td>
                  <td>{order.date}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
