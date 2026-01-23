import { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import { FiFilter, FiGrid, FiList } from 'react-icons/fi';
import { productService } from '../../services';
import ProductCard from '../../components/product/ProductCard/ProductCard';
import Loading from '../../components/common/Loading/Loading';
import './Products.css';

const Products = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [viewMode, setViewMode] = useState('grid');
  const [searchParams] = useSearchParams();
  const searchQuery = searchParams.get('search');

  useEffect(() => {
    loadProducts();
  }, [searchQuery]);

  const loadProducts = async () => {
    try {
      setLoading(true);
      let data;
      if (searchQuery) {
        data = await productService.search(searchQuery);
      } else {
        data = await productService.getAll();
      }
      setProducts(Array.isArray(data) ? data : data.items || []);
    } catch (error) {
      console.error('Erro ao carregar produtos:', error);
      // Mock data
      setProducts([
        { id: 1, name: 'Relógio Premium Gold', price: 1299.90, imageUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400', category: { name: 'Acessórios' } },
        { id: 2, name: 'Tênis Sport Elite', price: 459.90, imageUrl: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400', category: { name: 'Calçados' } },
        { id: 3, name: 'Bolsa Elegance', price: 789.90, imageUrl: 'https://images.unsplash.com/photo-1584917865442-de89df76afd3?w=400', category: { name: 'Bolsas' } },
        { id: 4, name: 'Óculos Aviator', price: 349.90, imageUrl: 'https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=400', category: { name: 'Acessórios' } },
        { id: 5, name: 'Headphone Pro', price: 599.90, imageUrl: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400', category: { name: 'Eletrônicos' } },
        { id: 6, name: 'Smart Watch X', price: 899.90, imageUrl: 'https://images.unsplash.com/photo-1546868871-7041f2a55e12?w=400', category: { name: 'Eletrônicos' } },
      ]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="products-page">
      <div className="container">
        <div className="page-header">
          <div>
            <h1>{searchQuery ? `Resultados para "${searchQuery}"` : 'Todos os Produtos'}</h1>
            <p>{products.length} produtos encontrados</p>
          </div>
          <div className="header-actions">
            <button className="btn btn-outline">
              <FiFilter /> Filtros
            </button>
            <div className="view-toggle">
              <button 
                className={`toggle-btn ${viewMode === 'grid' ? 'active' : ''}`}
                onClick={() => setViewMode('grid')}
              >
                <FiGrid />
              </button>
              <button 
                className={`toggle-btn ${viewMode === 'list' ? 'active' : ''}`}
                onClick={() => setViewMode('list')}
              >
                <FiList />
              </button>
            </div>
          </div>
        </div>

        {loading ? (
          <Loading text="Carregando produtos..." />
        ) : products.length > 0 ? (
          <div className={`products-grid ${viewMode}`}>
            {products.map((product) => (
              <ProductCard key={product.id} product={product} />
            ))}
          </div>
        ) : (
          <div className="empty-state">
            <h3>Nenhum produto encontrado</h3>
            <p>Tente buscar com outros termos</p>
          </div>
        )}
      </div>
    </main>
  );
};

export default Products;
