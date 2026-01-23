import { Link } from 'react-router-dom';
import { FiTrash2, FiMinus, FiPlus, FiShoppingBag, FiArrowRight } from 'react-icons/fi';
import { useCart } from '../../contexts/CartContext';
import { useAuth } from '../../contexts/AuthContext';
import './Cart.css';

const Cart = () => {
  const { cart, updateQuantity, removeFromCart, clearCart, itemCount } = useCart();
  const { isAuthenticated } = useAuth();

  const formatPrice = (price) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(price);
  };

  if (cart.items.length === 0) {
    return (
      <main className="cart-page">
        <div className="container">
          <div className="empty-cart">
            <FiShoppingBag className="empty-icon" />
            <h2>Seu carrinho está vazio</h2>
            <p>Explore nossos produtos e adicione itens ao carrinho</p>
            <Link to="/products" className="btn btn-primary">
              Ver Produtos
            </Link>
          </div>
        </div>
      </main>
    );
  }

  return (
    <main className="cart-page">
      <div className="container">
        <div className="cart-header">
          <h1>Carrinho de Compras</h1>
          <button className="clear-cart" onClick={clearCart}>
            <FiTrash2 /> Limpar carrinho
          </button>
        </div>

        <div className="cart-content">
          <div className="cart-items">
            {cart.items.map((item) => (
              <div key={item.productId} className="cart-item">
                <div className="item-image">
                  <img src={item.product?.imageUrl || 'https://via.placeholder.com/120'} alt={item.product?.name} />
                </div>

                <div className="item-details">
                  <Link to={`/products/${item.productId}`} className="item-name">
                    {item.product?.name || 'Produto'}
                  </Link>
                  <span className="item-category">{item.product?.category?.name}</span>
                  <span className="item-unit-price">{formatPrice(item.unitPrice)} / un</span>
                </div>

                <div className="item-quantity">
                  <button onClick={() => updateQuantity(item.productId, item.quantity - 1)}>
                    <FiMinus />
                  </button>
                  <span>{item.quantity}</span>
                  <button onClick={() => updateQuantity(item.productId, item.quantity + 1)}>
                    <FiPlus />
                  </button>
                </div>

                <div className="item-total">
                  {formatPrice(item.unitPrice * item.quantity)}
                </div>

                <button 
                  className="item-remove"
                  onClick={() => removeFromCart(item.productId)}
                >
                  <FiTrash2 />
                </button>
              </div>
            ))}
          </div>

          <div className="cart-summary">
            <h3>Resumo do Pedido</h3>
            
            <div className="summary-row">
              <span>Subtotal ({itemCount} itens)</span>
              <span>{formatPrice(cart.total || cart.items.reduce((acc, item) => acc + item.unitPrice * item.quantity, 0))}</span>
            </div>

            <div className="summary-row">
              <span>Frete</span>
              <span className="free-shipping">Grátis</span>
            </div>

            <div className="summary-divider"></div>

            <div className="summary-row total">
              <span>Total</span>
              <span>{formatPrice(cart.total || cart.items.reduce((acc, item) => acc + item.unitPrice * item.quantity, 0))}</span>
            </div>

            {isAuthenticated ? (
              <Link to="/checkout" className="btn btn-primary btn-block">
                Finalizar Compra <FiArrowRight />
              </Link>
            ) : (
              <Link to="/login" state={{ from: { pathname: '/checkout' } }} className="btn btn-primary btn-block">
                Fazer Login para Continuar
              </Link>
            )}

            <Link to="/products" className="continue-shopping">
              Continuar Comprando
            </Link>
          </div>
        </div>
      </div>
    </main>
  );
};

export default Cart;
