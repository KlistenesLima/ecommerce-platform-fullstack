import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FiPackage, FiEye } from 'react-icons/fi';
import { orderService } from '../../services';
import Loading from '../../components/common/Loading/Loading';
import './Orders.css';

const Orders = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadOrders();
  }, []);

  const loadOrders = async () => {
    try {
      const data = await orderService.getAll();
      setOrders(Array.isArray(data) ? data : []);
    } catch (error) {
      // Mock data
      setOrders([
        { id: '1', status: 'Entregue', total: 1299.90, createdAt: '2026-01-20', items: 2 },
        { id: '2', status: 'Em trânsito', total: 459.90, createdAt: '2026-01-18', items: 1 },
        { id: '3', status: 'Processando', total: 789.90, createdAt: '2026-01-15', items: 3 },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(price);
  };

  const getStatusClass = (status) => {
    const map = {
      'Entregue': 'delivered',
      'Em trânsito': 'shipping',
      'Processando': 'processing',
      'Cancelado': 'cancelled'
    };
    return map[status] || 'processing';
  };

  if (loading) return <Loading size="large" text="Carregando pedidos..." />;

  return (
    <main className="orders-page">
      <div className="container">
        <h1>Meus Pedidos</h1>

        {orders.length === 0 ? (
          <div className="empty-orders">
            <FiPackage className="empty-icon" />
            <h3>Nenhum pedido encontrado</h3>
            <p>Você ainda não fez nenhum pedido</p>
            <Link to="/products" className="btn btn-primary">Ver Produtos</Link>
          </div>
        ) : (
          <div className="orders-list">
            {orders.map(order => (
              <div key={order.id} className="order-card">
                <div className="order-header">
                  <div>
                    <span className="order-id">Pedido #{order.id}</span>
                    <span className="order-date">{order.createdAt}</span>
                  </div>
                  <span className={`order-status ${getStatusClass(order.status)}`}>
                    {order.status}
                  </span>
                </div>
                <div className="order-body">
                  <div className="order-info">
                    <span>{order.items} item(s)</span>
                    <span className="order-total">{formatPrice(order.total)}</span>
                  </div>
                  <Link to={`/orders/${order.id}`} className="btn btn-outline btn-sm">
                    <FiEye /> Ver detalhes
                  </Link>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </main>
  );
};

export default Orders;
