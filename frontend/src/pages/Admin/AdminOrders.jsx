import { useState, useEffect } from 'react';
import { FiEye, FiCheck, FiX, FiTruck } from 'react-icons/fi';
import api from '../../services/api';
import { toast } from 'react-toastify';
import './AdminPages.css';

const AdminOrders = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState('all');

  useEffect(() => {
    loadOrders();
  }, []);

  const loadOrders = async () => {
    try {
      setLoading(true);
      const response = await api.get('/orders');
      setOrders(response.data || []);
    } catch (error) {
      console.error('Erro ao carregar pedidos:', error);
      toast.error('Erro ao carregar pedidos');
      setOrders([]);
    } finally {
      setLoading(false);
    }
  };

  const updateStatus = async (id, newStatus) => {
    try {
      await api.patch(`/orders/${id}/status`, { status: newStatus });
      setOrders(orders.map(o => o.id === id ? { ...o, status: newStatus } : o));
      toast.success(`Pedido atualizado para ${newStatus}`);
    } catch (error) {
      console.error('Erro ao atualizar:', error);
      toast.error('Erro ao atualizar pedido');
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
      'Cancelled': 'Cancelado',
      'Pendente': 'Pendente',
      'Processando': 'Processando',
      'Enviado': 'Enviado',
      'Entregue': 'Entregue',
      'Cancelado': 'Cancelado'
    };
    return map[status] || status;
  };

  const getStatusClass = (status) => {
    const map = {
      'Pending': 'pendente',
      'Processing': 'processando',
      'Shipped': 'enviado',
      'Delivered': 'entregue',
      'Cancelled': 'cancelado',
      'Pendente': 'pendente',
      'Processando': 'processando',
      'Enviado': 'enviado',
      'Entregue': 'entregue',
      'Cancelado': 'cancelado'
    };
    return map[status] || 'pendente';
  };

  const filtered = filter === 'all' 
    ? orders 
    : orders.filter(o => getStatusLabel(o.status) === filter || o.status === filter);

  return (
    <div className="admin-page">
      <div className="page-header">
        <div>
          <h1>Pedidos</h1>
          <p>{orders.length} pedidos</p>
        </div>
      </div>

      <div className="filters">
        {['all', 'Pendente', 'Processando', 'Enviado', 'Entregue', 'Cancelado'].map(f => (
          <button
            key={f}
            onClick={() => setFilter(f)}
            className={`filter-btn ${filter === f ? 'active' : ''}`}
          >
            {f === 'all' ? 'Todos' : f}
          </button>
        ))}
      </div>

      <div className="table-container">
        <table className="data-table">
          <thead>
            <tr>
              <th>Pedido</th>
              <th>Cliente</th>
              <th>Itens</th>
              <th>Total</th>
              <th>Status</th>
              <th>Data</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr><td colSpan="7" className="center">Carregando...</td></tr>
            ) : filtered.length === 0 ? (
              <tr><td colSpan="7" className="center">Nenhum pedido encontrado</td></tr>
            ) : filtered.map(order => (
              <tr key={order.id}>
                <td>#{order.id?.substring(0, 8) || order.id}</td>
                <td>
                  <div>
                    <strong>{order.user?.firstName || order.customerName || 'N/A'} {order.user?.lastName || ''}</strong>
                    <small style={{ display: 'block', color: 'var(--text-muted)' }}>{order.user?.email || order.email || ''}</small>
                  </div>
                </td>
                <td>{order.items?.length || order.itemsCount || 0}</td>
                <td className="price">{formatPrice(order.totalAmount || order.total || 0)}</td>
                <td><span className={`badge ${getStatusClass(order.status)}`}>{getStatusLabel(order.status)}</span></td>
                <td>{formatDate(order.createdAt || order.date)}</td>
                <td>
                  <div className="actions">
                    <button className="btn-icon" title="Ver detalhes"><FiEye /></button>
                    {(order.status === 'Pending' || order.status === 'Pendente') && (
                      <>
                        <button onClick={() => updateStatus(order.id, 'Processing')} className="btn-icon edit" title="Processar"><FiCheck /></button>
                        <button onClick={() => updateStatus(order.id, 'Cancelled')} className="btn-icon delete" title="Cancelar"><FiX /></button>
                      </>
                    )}
                    {(order.status === 'Processing' || order.status === 'Processando') && (
                      <button onClick={() => updateStatus(order.id, 'Shipped')} className="btn-icon edit" title="Marcar como enviado"><FiTruck /></button>
                    )}
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default AdminOrders;
