import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FiPackage, FiShoppingCart, FiUsers, FiDollarSign, FiArrowRight } from 'react-icons/fi';
import api from '../../services/api';
import './Dashboard.css';

const Dashboard = () => {
  const [stats, setStats] = useState({
    totalProducts: 0,
    totalOrders: 0,
    totalUsers: 0,
    totalRevenue: 0
  });
  const [recentOrders, setRecentOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);

      // Carregar produtos
      const productsRes = await api.get('/products').catch(() => ({ data: [] }));
      const products = productsRes.data || [];

      // Carregar pedidos
      const ordersRes = await api.get('/orders').catch(() => ({ data: [] }));
      const orders = ordersRes.data || [];

      // Carregar usuários
      const usersRes = await api.get('/users').catch(() => ({ data: [] }));
      const users = usersRes.data || [];

      // Calcular estatísticas
      const totalRevenue = orders.reduce((sum, o) => sum + (o.totalAmount || o.total || 0), 0);

      setStats({
        totalProducts: products.length,
        totalOrders: orders.length,
        totalUsers: users.length,
        totalRevenue
      });

      // Pedidos recentes (últimos 5)
      const recent = orders
        .sort((a, b) => new Date(b.createdAt || b.date) - new Date(a.createdAt || a.date))
        .slice(0, 5);
      setRecentOrders(recent);

    } catch (error) {
      console.error('Erro ao carregar dashboard:', error);
    } finally {
      setLoading(false);
    }
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(price);
  };

  const formatDate = (date) => {
    return new Date(date).toLocaleDateString('pt-BR');
  };

  const getStatusLabel = (status) => {
    const map = {
      'Pending': 'Pendente',
      'Processing': 'Processando',
      'Shipped': 'Enviado',
      'Delivered': 'Entregue',
      'Cancelled': 'Cancelado'
    };
    return map[status] || status;
  };

  const getStatusClass = (status) => {
    const map = {
      'Pending': 'pendente',
      'Processing': 'processando',
      'Shipped': 'enviado',
      'Delivered': 'entregue',
      'Cancelled': 'cancelado'
    };
    return map[status] || 'pendente';
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
            <span className="stat-value">{loading ? '...' : stats.totalProducts}</span>
            <span className="stat-label">Produtos</span>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon orders"><FiShoppingCart /></div>
          <div className="stat-info">
            <span className="stat-value">{loading ? '...' : stats.totalOrders}</span>
            <span className="stat-label">Pedidos</span>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon users"><FiUsers /></div>
          <div className="stat-info">
            <span className="stat-value">{loading ? '...' : stats.totalUsers}</span>
            <span className="stat-label">Usuários</span>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon revenue"><FiDollarSign /></div>
          <div className="stat-info">
            <span className="stat-value">{loading ? '...' : formatPrice(stats.totalRevenue)}</span>
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
            <span>Categorias</span>
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
              {loading ? (
                <tr><td colSpan="5" className="center">Carregando...</td></tr>
              ) : recentOrders.length === 0 ? (
                <tr><td colSpan="5" className="center">Nenhum pedido encontrado</td></tr>
              ) : recentOrders.map(order => (
                <tr key={order.id}>
                  <td>#{order.id?.substring(0, 8) || order.id}</td>
                  <td>{order.user?.firstName || order.customerName || 'N/A'}</td>
                  <td className="price">{formatPrice(order.totalAmount || order.total || 0)}</td>
                  <td><span className={`badge ${getStatusClass(order.status)}`}>{getStatusLabel(order.status)}</span></td>
                  <td>{formatDate(order.createdAt || order.date)}</td>
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
