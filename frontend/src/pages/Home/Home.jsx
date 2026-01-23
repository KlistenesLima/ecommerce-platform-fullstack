import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FiArrowRight, FiTruck, FiShield, FiCreditCard, FiHeadphones } from 'react-icons/fi';
import { productService } from '../../services';
import ProductCard from '../../components/product/ProductCard/ProductCard';
import Loading from '../../components/common/Loading/Loading';
import './Home.css';

const Home = () => {
  const [featuredProducts, setFeaturedProducts] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      const data = await productService.getAll({ limit: 8 });
      setFeaturedProducts(Array.isArray(data) ? data : data.items || []);
    } catch (error) {
      console.error('Erro ao carregar produtos:', error);
      // Mock data para demonstração
      setFeaturedProducts([
        { id: 1, name: 'Relógio Premium Gold', price: 1299.90, imageUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400', category: { name: 'Acessórios' } },
        { id: 2, name: 'Tênis Sport Elite', price: 459.90, imageUrl: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400', category: { name: 'Calçados' } },
        { id: 3, name: 'Bolsa Elegance', price: 789.90, imageUrl: 'https://images.unsplash.com/photo-1584917865442-de89df76afd3?w=400', category: { name: 'Bolsas' } },
        { id: 4, name: 'Óculos Aviator', price: 349.90, imageUrl: 'https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=400', category: { name: 'Acessórios' } },
        { id: 5, name: 'Headphone Pro', price: 599.90, imageUrl: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400', category: { name: 'Eletrônicos' } },
        { id: 6, name: 'Smart Watch X', price: 899.90, imageUrl: 'https://images.unsplash.com/photo-1546868871-7041f2a55e12?w=400', category: { name: 'Eletrônicos' } },
        { id: 7, name: 'Câmera Vintage', price: 1599.90, imageUrl: 'https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?w=400', category: { name: 'Eletrônicos' } },
        { id: 8, name: 'Perfume Luxe', price: 289.90, imageUrl: 'https://images.unsplash.com/photo-1541643600914-78b084683601?w=400', category: { name: 'Perfumaria' } },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const features = [
    { icon: <FiTruck />, title: 'Frete Grátis', description: 'Em compras acima de R$ 299' },
    { icon: <FiShield />, title: 'Compra Segura', description: 'Proteção total dos seus dados' },
    { icon: <FiCreditCard />, title: 'Parcelamento', description: 'Até 12x sem juros' },
    { icon: <FiHeadphones />, title: 'Suporte 24h', description: 'Atendimento especializado' },
  ];

  return (
    <main className="home">
      {/* Hero Section */}
      <section className="hero">
        <div className="hero-content">
          <span className="hero-badge">Nova Coleção 2026</span>
          <h1 className="hero-title">
            Descubra o <span className="highlight">Extraordinário</span>
          </h1>
          <p className="hero-description">
            Explore nossa coleção exclusiva de produtos premium. 
            Qualidade, elegância e sofisticação em cada detalhe.
          </p>
          <div className="hero-actions">
            <Link to="/products" className="btn btn-primary btn-lg">
              Explorar Coleção <FiArrowRight />
            </Link>
            <Link to="/categories" className="btn btn-outline btn-lg">
              Ver Categorias
            </Link>
          </div>
        </div>
        <div className="hero-image">
          <div className="hero-image-wrapper">
            <img 
              src="https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=800" 
              alt="Coleção Premium"
            />
          </div>
        </div>
        <div className="hero-decoration"></div>
      </section>

      {/* Features */}
      <section className="features">
        <div className="container">
          <div className="features-grid">
            {features.map((feature, index) => (
              <div key={index} className="feature-card">
                <div className="feature-icon">{feature.icon}</div>
                <h3 className="feature-title">{feature.title}</h3>
                <p className="feature-description">{feature.description}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Featured Products */}
      <section className="section featured-products">
        <div className="container">
          <div className="section-header">
            <div className="section-title">
              <h2>Produtos em Destaque</h2>
              <p>Os mais desejados da nossa loja</p>
            </div>
            <Link to="/products" className="btn btn-outline">
              Ver Todos <FiArrowRight />
            </Link>
          </div>

          {loading ? (
            <Loading text="Carregando produtos..." />
          ) : (
            <div className="products-grid">
              {featuredProducts.map((product) => (
                <ProductCard key={product.id} product={product} />
              ))}
            </div>
          )}
        </div>
      </section>

      {/* CTA Banner */}
      <section className="cta-banner">
        <div className="container">
          <div className="cta-content">
            <h2>Cadastre-se e ganhe 10% OFF</h2>
            <p>Na sua primeira compra. Aproveite ofertas exclusivas!</p>
            <Link to="/register" className="btn btn-primary btn-lg">
              Criar Conta Grátis
            </Link>
          </div>
        </div>
      </section>
    </main>
  );
};

export default Home;
