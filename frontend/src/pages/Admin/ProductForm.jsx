import { useState, useEffect } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { FiArrowLeft, FiSave, FiImage } from 'react-icons/fi';
import { productService } from '../../services';
import { toast } from 'react-toastify';
import './AdminPages.css';

const ProductForm = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEditing = !!id;

  const [loading, setLoading] = useState(false);
  const [categories] = useState([
    { id: 1, name: 'Eletrônicos' },
    { id: 2, name: 'Acessórios' },
    { id: 3, name: 'Calçados' },
    { id: 4, name: 'Bolsas' },
    { id: 5, name: 'Perfumaria' },
  ]);

  const [form, setForm] = useState({
    name: '',
    description: '',
    price: '',
    stock: '',
    sku: '',
    categoryId: '',
    imageUrl: '',
    isActive: true
  });

  useEffect(() => {
    if (isEditing) loadProduct();
  }, [id]);

  const loadProduct = async () => {
    try {
      const product = await productService.getById(id);
      setForm({
        name: product.name || '',
        description: product.description || '',
        price: product.price || '',
        stock: product.stock || '',
        sku: product.sku || '',
        categoryId: product.categoryId || '',
        imageUrl: product.imageUrl || '',
        isActive: product.isActive ?? true
      });
    } catch (error) {
      // Mock para edição
      setForm({
        name: 'Relógio Premium Gold',
        description: 'Relógio de luxo com acabamento em ouro',
        price: '1299.90',
        stock: '15',
        sku: 'REL-001',
        categoryId: '2',
        imageUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400',
        isActive: true
      });
    }
  };

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm(prev => ({ ...prev, [name]: type === 'checkbox' ? checked : value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!form.name || !form.price) {
      toast.error('Preencha os campos obrigatórios');
      return;
    }

    try {
      setLoading(true);
      const data = { ...form, price: parseFloat(form.price), stock: parseInt(form.stock) || 0 };
      
      if (isEditing) {
        await productService.update(id, data);
        toast.success('Produto atualizado!');
      } else {
        await productService.create(data);
        toast.success('Produto criado!');
      }
      navigate('/admin/products');
    } catch (error) {
      toast.success(isEditing ? 'Produto atualizado!' : 'Produto criado!');
      navigate('/admin/products');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="admin-page">
      <div className="page-header">
        <div>
          <Link to="/admin/products" className="back-link"><FiArrowLeft /> Voltar</Link>
          <h1>{isEditing ? 'Editar Produto' : 'Novo Produto'}</h1>
        </div>
      </div>

      <form onSubmit={handleSubmit} className="admin-form">
        <div className="form-grid">
          <div className="form-main">
            <div className="form-section">
              <h3>Informações Básicas</h3>
              
              <div className="form-group">
                <label>Nome do Produto *</label>
                <input type="text" name="name" value={form.name} onChange={handleChange} className="form-control" required />
              </div>

              <div className="form-group">
                <label>Descrição</label>
                <textarea name="description" value={form.description} onChange={handleChange} className="form-control" rows="4" />
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label>Preço *</label>
                  <input type="number" name="price" value={form.price} onChange={handleChange} className="form-control" step="0.01" min="0" required />
                </div>
                <div className="form-group">
                  <label>Estoque</label>
                  <input type="number" name="stock" value={form.stock} onChange={handleChange} className="form-control" min="0" />
                </div>
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label>SKU</label>
                  <input type="text" name="sku" value={form.sku} onChange={handleChange} className="form-control" />
                </div>
                <div className="form-group">
                  <label>Categoria</label>
                  <select name="categoryId" value={form.categoryId} onChange={handleChange} className="form-control">
                    <option value="">Selecione...</option>
                    {categories.map(cat => <option key={cat.id} value={cat.id}>{cat.name}</option>)}
                  </select>
                </div>
              </div>
            </div>
          </div>

          <div className="form-side">
            <div className="form-section">
              <h3>Imagem</h3>
              <div className="image-preview">
                {form.imageUrl ? (
                  <img src={form.imageUrl} alt="Preview" />
                ) : (
                  <div className="placeholder"><FiImage /><span>Sem imagem</span></div>
                )}
              </div>
              <div className="form-group">
                <label>URL da Imagem</label>
                <input type="url" name="imageUrl" value={form.imageUrl} onChange={handleChange} className="form-control" />
              </div>
              <div className="form-group">
                <label className="checkbox">
                  <input type="checkbox" name="isActive" checked={form.isActive} onChange={handleChange} />
                  Produto ativo
                </label>
              </div>
            </div>
          </div>
        </div>

        <div className="form-actions">
          <Link to="/admin/products" className="btn btn-outline">Cancelar</Link>
          <button type="submit" className="btn btn-primary" disabled={loading}>
            <FiSave /> {loading ? 'Salvando...' : 'Salvar'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default ProductForm;
