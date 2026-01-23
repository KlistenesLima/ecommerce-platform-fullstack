import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useCart } from '../../contexts/CartContext';
import { orderService } from '../../services';
import { toast } from 'react-toastify';
import './Checkout.css';

const Checkout = () => {
  const { cart, clearCart } = useCart();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const formatPrice = (price) => {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(price);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      // await orderService.create({ ... });
      toast.success('Pedido realizado com sucesso!');
      clearCart();
      navigate('/orders');
    } catch (error) {
      toast.error('Erro ao processar pedido');
    } finally {
      setLoading(false);
    }
  };

  const total = cart.items.reduce((acc, item) => acc + item.unitPrice * item.quantity, 0);

  return (
    <main className="checkout-page">
      <div className="container">
        <h1>Finalizar Compra</h1>
        
        <div className="checkout-content">
          <form onSubmit={handleSubmit} className="checkout-form">
            <div className="form-section">
              <h3>Endereço de Entrega</h3>
              <div className="form-row">
                <div className="form-group">
                  <label>CEP</label>
                  <input type="text" className="form-control" placeholder="00000-000" required />
                </div>
              </div>
              <div className="form-group">
                <label>Endereço</label>
                <input type="text" className="form-control" placeholder="Rua, número" required />
              </div>
              <div className="form-row">
                <div className="form-group">
                  <label>Cidade</label>
                  <input type="text" className="form-control" required />
                </div>
                <div className="form-group">
                  <label>Estado</label>
                  <input type="text" className="form-control" required />
                </div>
              </div>
            </div>

            <div className="form-section">
              <h3>Pagamento</h3>
              <div className="form-group">
                <label>Número do Cartão</label>
                <input type="text" className="form-control" placeholder="0000 0000 0000 0000" required />
              </div>
              <div className="form-row">
                <div className="form-group">
                  <label>Validade</label>
                  <input type="text" className="form-control" placeholder="MM/AA" required />
                </div>
                <div className="form-group">
                  <label>CVV</label>
                  <input type="text" className="form-control" placeholder="000" required />
                </div>
              </div>
            </div>

            <button type="submit" className="btn btn-primary btn-lg btn-block" disabled={loading}>
              {loading ? 'Processando...' : `Pagar ${formatPrice(total)}`}
            </button>
          </form>

          <div className="order-summary">
            <h3>Resumo do Pedido</h3>
            {cart.items.map(item => (
              <div key={item.productId} className="summary-item">
                <span>{item.product?.name} x{item.quantity}</span>
                <span>{formatPrice(item.unitPrice * item.quantity)}</span>
              </div>
            ))}
            <div className="summary-total">
              <span>Total</span>
              <span>{formatPrice(total)}</span>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
};

export default Checkout;
