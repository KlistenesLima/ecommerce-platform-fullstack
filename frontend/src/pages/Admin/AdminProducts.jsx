import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FiPlus, FiEdit2, FiTrash2, FiSearch } from 'react-icons/fi';
import api from '../../services/api';
import { toast } from 'react-toastify';
import './AdminPages.css';

const AdminProducts = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      setLoading(true);
      const response = await api.get('/products');
      setProducts(response.data || []);
    } catch (error) {
      console.error('Erro ao carregar produtos:', error);
      toast.error('Erro ao carregar produtos');
      setProducts([]);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Tem certeza que deseja excluir?')) return;
    try {
      await api.delete(`/products/${id}`);
      setProducts(products.filter(p => p.id !== id));
      toast.success('Produto excluído!');
    } catch (error) {
      toast.error('Erro ao excluir produto');
    }
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(price);
  };

  const filtered = products.filter(p => 
    p.name?.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="admin-page">
      <div className="page-header">
        <div>
          <h1>Produtos</h1>
          <p>{products.length} produtos cadastrados</p>
        </div>
        <Link to="/admin/products/new" className="btn btn-primary">
          <FiPlus /> Novo Produto
        </Link>
      </div>

      <div className="search-box">
        <FiSearch />
        <input
          type="text"
          placeholder="Buscar produtos..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>

      <div className="table-container">
        <table className="data-table">
          <thead>
            <tr>
              <th>Produto</th>
              <th>Categoria</th>
              <th>Preço</th>
              <th>Estoque</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr><td colSpan="5" className="center">Carregando...</td></tr>
            ) : filtered.length === 0 ? (
              <tr><td colSpan="5" className="center">Nenhum produto encontrado</td></tr>
            ) : filtered.map(product => (
              <tr key={product.id}>
                <td>
                  <div className="product-cell">
                    <img src={product.imageUrl || 'https://via.placeholder.com/50'} alt="" />
                    <span>{product.name}</span>
                  </div>
                </td>
                <td>{product.category?.name || product.categoryName || '-'}</td>
                <td className="price">{formatPrice(product.price)}</td>
                <td>
                  <span className={`stock-badge ${product.stockQuantity === 0 ? 'out' : product.stockQuantity < 10 ? 'low' : 'ok'}`}>
                    {product.stockQuantity ?? product.stock ?? 0}
                  </span>
                </td>
                <td>
                  <div className="actions">
                    <Link to={`/admin/products/${product.id}/edit`} className="btn-icon edit"><FiEdit2 /></Link>
                    <button onClick={() => handleDelete(product.id)} className="btn-icon delete"><FiTrash2 /></button>
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

export default AdminProducts;
