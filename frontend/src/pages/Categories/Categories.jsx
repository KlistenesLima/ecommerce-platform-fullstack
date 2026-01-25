import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FiArrowRight } from 'react-icons/fi';
import './Categories.css';

const Categories = () => {
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    // Mock data - conectar com API depois
    setCategories([
      { id: 1, name: 'Eletrônicos', description: 'Gadgets e dispositivos', image: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400', count: 12 },
      { id: 2, name: 'Acessórios', description: 'Relógios, óculos e mais', image: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400', count: 18 },
      { id: 3, name: 'Calçados', description: 'Tênis e sapatos premium', image: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400', count: 8 },
      { id: 4, name: 'Bolsas', description: 'Bolsas e carteiras de luxo', image: 'https://images.unsplash.com/photo-1584917865442-de89df76afd3?w=400', count: 6 },
      { id: 5, name: 'Perfumaria', description: 'Perfumes e cosméticos', image: 'https://images.unsplash.com/photo-1541643600914-78b084683601?w=400', count: 10 },
      { id: 6, name: 'Joias', description: 'Joias e bijuterias finas', image: 'https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?w=400', count: 15 },
    ]);
  }, []);

  return (
    <main className="categories-page">
      <div className="container">
        <div className="page-header">
          <h1>Categorias</h1>
          <p>Explore nossos produtos por categoria</p>
        </div>

        <div className="categories-grid">
          {categories.map(category => (
            <Link to={`/products?category=${category.id}`} key={category.id} className="category-card">
              <div className="category-image">
                <img src={category.image} alt={category.name} />
                <div className="category-overlay"></div>
              </div>
              <div className="category-content">
                <h3>{category.name}</h3>
                <p>{category.description}</p>
                <span className="category-count">{category.count} produtos</span>
                <span className="category-link">
                  Ver produtos <FiArrowRight />
                </span>
              </div>
            </Link>
          ))}
        </div>
      </div>
    </main>
  );
};

export default Categories;
