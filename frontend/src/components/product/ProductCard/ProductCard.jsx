import { Link } from 'react-router-dom';
import { FiShoppingCart, FiHeart, FiEye } from 'react-icons/fi';
import { useCart } from '../../../contexts/CartContext';
import './ProductCard.css';

const ProductCard = ({ product }) => {
  const { addToCart } = useCart();

  const handleAddToCart = (e) => {
    e.preventDefault();
    addToCart(product, 1);
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(price);
  };

  return (
    <div className="product-card">
      <Link to={`/products/${product.id}`} className="product-link">
        <div className="product-image-container">
          <img 
            src={product.imageUrl || '/placeholder.jpg'} 
            alt={product.name}
            className="product-image"
          />
          
          {product.discount > 0 && (
            <span className="product-badge sale">-{product.discount}%</span>
          )}
          
          {product.stock <= 5 && product.stock > 0 && (
            <span className="product-badge limited">Últimas unidades</span>
          )}

          <div className="product-actions">
            <button className="action-icon" title="Favoritar">
              <FiHeart />
            </button>
            <button className="action-icon" title="Visualizar">
              <FiEye />
            </button>
            <button className="action-icon" onClick={handleAddToCart} title="Adicionar ao carrinho">
              <FiShoppingCart />
            </button>
          </div>
        </div>

        <div className="product-info">
          <span className="product-category">{product.category?.name}</span>
          <h3 className="product-name">{product.name}</h3>
          <div className="product-price-container">
            {product.oldPrice && (
              <span className="product-old-price">{formatPrice(product.oldPrice)}</span>
            )}
            <span className="product-price">{formatPrice(product.price)}</span>
          </div>
        </div>
      </Link>

      <button className="btn btn-primary btn-block" onClick={handleAddToCart}>
        <FiShoppingCart /> Adicionar
      </button>
    </div>
  );
};

export default ProductCard;
