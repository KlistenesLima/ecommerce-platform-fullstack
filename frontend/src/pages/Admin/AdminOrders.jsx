import { useState, useEffect } from 'react';
import { FiEye, FiCheck, FiX, FiTruck } from 'react-icons/fi';
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
      // const data = await orderService.getAll();
      setOrders([
        { id: '001', customer: 'João Silva', email: 'joao@email.com', total: 1299.90, status: 'Pendente', date: '2026-01-23', items: 2 },
        { id: '002', customer: 'Maria Santos', email: 'maria@email.com', total: 459.90, status: 'Processando', date: '2026-01-23', items: 1 },
        { id: '003', customer: 'Pedro Costa', email: 'pedro@email.com', total: 789.90, status: 'Enviado', date: '2026-01-22', items: 3 },
        { id: '004', customer: 'Ana Oliveira', email: 'ana@email.com', total: 189.90, status: 'Entregue', date: '2026-01-21', items: 1 },
        { id: '005', customer: 'Carlos Lima', email: 'carlos@email.com', total: 349.90, status: 'Cancelado', date: '2026-01-20', items: 2 },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const updateStatus = (id, newStatus) => {
    setOrders(orders.map(o => o.id === id ? { ...o, status: newStatus } : o));
    toast.success(`Pedido #${id} atualizado para ${newStatus}`);
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(price);
  };

  const filtered = filter === 'all' ? orders : orders.filter(o => o.status === filter);

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
                <td>#{order.id}</td>
                <td>
                  <div>
                    <strong>{order.customer}</strong>
                    <small style={{ display: 'block', color: 'var(--text-muted)' }}>{order.email}</small>
                  </div>
                </td>
                <td>{order.items}</td>
                <td className="price">{formatPrice(order.total)}</td>
                <td><span className={`badge ${order.status.toLowerCase()}`}>{order.status}</span></td>
                <td>{order.date}</td>
                <td>
                  <div className="actions">
                    <button className="btn-icon" title="Ver detalhes"><FiEye /></button>
                    {order.status === 'Pendente' && (
                      <>
                        <button onClick={() => updateStatus(order.id, 'Processando')} className="btn-icon edit" title="Processar"><FiCheck /></button>
                        <button onClick={() => updateStatus(order.id, 'Cancelado')} className="btn-icon delete" title="Cancelar"><FiX /></button>
                      </>
                    )}
                    {order.status === 'Processando' && (
                      <button onClick={() => updateStatus(order.id, 'Enviado')} className="btn-icon edit" title="Marcar como enviado"><FiTruck /></button>
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
