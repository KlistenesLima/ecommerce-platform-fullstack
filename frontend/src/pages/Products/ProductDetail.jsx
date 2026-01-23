import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { FiShoppingCart, FiHeart, FiMinus, FiPlus, FiArrowLeft } from 'react-icons/fi';
import { productService } from '../../services';
import { useCart } from '../../contexts/CartContext';
import Loading from '../../components/common/Loading/Loading';
import { toast } from 'react-toastify';
import './ProductDetail.css';

const ProductDetail = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);
  const [quantity, setQuantity] = useState(1);
  const { addToCart } = useCart();

  useEffect(() => {
    loadProduct();
  }, [id]);

  const loadProduct = async () => {
    try {
      setLoading(true);
      const data = await productService.getById(id);
      setProduct(data);
    } catch (error) {
      // Mock data
      setProduct({
        id: id,
        name: 'Relógio Premium Gold Edition',
        price: 1299.90,
        oldPrice: 1599.90,
        description: 'Relógio de luxo com acabamento em ouro, resistente à água até 100m. Design elegante e sofisticado para ocasiões especiais.',
        imageUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=800',
        stock: 15,
        category: { name: 'Acessórios' },
        features: ['Resistente à água', 'Garantia de 2 anos', 'Acabamento premium', 'Movimento automático']
      });
    } finally {
      setLoading(false);
    }
  };

  const handleAddToCart = () => {
    addToCart(product, quantity);
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(price);
  };

  if (loading) {
    return <Loading size="large" text="Carregando produto..." />;
  }

  if (!product) {
    return (
      <div className="not-found">
        <h2>Produto não encontrado</h2>
        <Link to="/products" className="btn btn-primary">Ver Produtos</Link>
      </div>
    );
  }

  return (
    <main className="product-detail-page">
      <div className="container">
        <Link to="/products" className="back-link">
          <FiArrowLeft /> Voltar para produtos
        </Link>

        <div className="product-detail">
          <div className="product-gallery">
            <div className="main-image">
              <img src={product.imageUrl} alt={product.name} />
            </div>
          </div>

          <div className="product-info">
            <span className="product-category">{product.category?.name}</span>
            <h1 className="product-title">{product.name}</h1>
            
            <div className="product-pricing">
              {product.oldPrice && (
                <span className="old-price">{formatPrice(product.oldPrice)}</span>
              )}
              <span className="current-price">{formatPrice(product.price)}</span>
              {product.oldPrice && (
                <span className="discount-badge">
                  -{Math.round((1 - product.price / product.oldPrice) * 100)}%
                </span>
              )}
            </div>

            <p className="product-description">{product.description}</p>

            {product.features && (
              <ul className="product-features">
                {product.features.map((feature, index) => (
                  <li key={index}>{feature}</li>
                ))}
              </ul>
            )}

            <div className="stock-info">
              {product.stock > 0 ? (
                <span className="in-stock">✓ Em estoque ({product.stock} disponíveis)</span>
              ) : (
                <span className="out-of-stock">✗ Fora de estoque</span>
              )}
            </div>

            <div className="product-actions">
              <div className="quantity-selector">
                <button onClick={() => setQuantity(Math.max(1, quantity - 1))}>
                  <FiMinus />
                </button>
                <span>{quantity}</span>
                <button onClick={() => setQuantity(Math.min(product.stock, quantity + 1))}>
                  <FiPlus />
                </button>
              </div>

              <button 
                className="btn btn-primary btn-lg"
                onClick={handleAddToCart}
                disabled={product.stock === 0}
              >
                <FiShoppingCart /> Adicionar ao Carrinho
              </button>

              <button className="btn btn-outline wishlist-btn">
                <FiHeart />
              </button>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
};

export default ProductDetail;
