import { useState, useEffect } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { FiArrowLeft, FiSave, FiImage, FiUpload, FiX } from 'react-icons/fi';
import api from '../../services/api';
import { toast } from 'react-toastify';
import './AdminPages.css';

const ProductForm = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEditing = !!id;

  const [loading, setLoading] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [categories, setCategories] = useState([]);

  const [form, setForm] = useState({
    name: '',
    description: '',
    price: '',
    stockQuantity: '',
    sku: '',
    categoryId: '',
    imageUrl: '',
    isActive: true
  });

  useEffect(() => {
    loadCategories();
    if (isEditing) loadProduct();
  }, [id]);

  const loadCategories = async () => {
    try {
      const response = await api.get('/categories');
      setCategories(response.data || []);
    } catch (error) {
      console.error('Erro ao carregar categorias:', error);
      toast.error('Erro ao carregar categorias');
    }
  };

  const loadProduct = async () => {
    try {
      setLoading(true);
      const response = await api.get(`/products/${id}`);
      const product = response.data;
      setForm({
        name: product.name || '',
        description: product.description || '',
        price: product.price || '',
        stockQuantity: product.stockQuantity || product.stock || '',
        sku: product.sku || '',
        categoryId: product.categoryId || '',
        imageUrl: product.imageUrl || '',
        isActive: product.isActive ?? true
      });
    } catch (error) {
      toast.error('Erro ao carregar produto');
      navigate('/admin/products');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm(prev => ({ ...prev, [name]: type === 'checkbox' ? checked : value }));
  };

  const handleImageUpload = async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    // Validar tipo
    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
    if (!allowedTypes.includes(file.type)) {
      toast.error('Tipo de arquivo não permitido. Use: JPG, PNG, GIF ou WEBP');
      return;
    }

    // Validar tamanho (5MB)
    if (file.size > 5 * 1024 * 1024) {
      toast.error('Arquivo muito grande. Máximo: 5MB');
      return;
    }

    try {
      setUploading(true);
      const formData = new FormData();
      formData.append('file', file);

      const response = await api.post('/upload/image', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });

      setForm(prev => ({ ...prev, imageUrl: response.data.url }));
      toast.success('Imagem enviada com sucesso!');
    } catch (error) {
      console.error('Erro no upload:', error);
      toast.error('Erro ao enviar imagem');
    } finally {
      setUploading(false);
    }
  };

  const removeImage = () => {
    setForm(prev => ({ ...prev, imageUrl: '' }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!form.name || !form.price) {
      toast.error('Preencha os campos obrigatórios');
      return;
    }

    if (!form.categoryId) {
      toast.error('Selecione uma categoria');
      return;
    }

    try {
      setLoading(true);
      const data = {
        name: form.name,
        description: form.description,
        price: parseFloat(form.price),
        stockQuantity: parseInt(form.stockQuantity) || 0,
        sku: form.sku,
        categoryId: form.categoryId,
        imageUrl: form.imageUrl,
        isActive: form.isActive
      };

      if (isEditing) {
        await api.put(`/products/${id}`, data);
        toast.success('Produto atualizado!');
      } else {
        await api.post('/products', data);
        toast.success('Produto criado!');
      }
      navigate('/admin/products');
    } catch (error) {
      console.error('Erro ao salvar:', error);
      toast.error(error.response?.data?.message || 'Erro ao salvar produto');
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
                <input 
                  type="text" 
                  name="name" 
                  value={form.name} 
                  onChange={handleChange} 
                  className="form-control" 
                  placeholder="Ex: Relógio Premium Gold"
                  required 
                />
              </div>

              <div className="form-group">
                <label>Descrição</label>
                <textarea 
                  name="description" 
                  value={form.description} 
                  onChange={handleChange} 
                  className="form-control" 
                  rows="4"
                  placeholder="Descreva o produto..."
                />
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label>Preço *</label>
                  <input 
                    type="number" 
                    name="price" 
                    value={form.price} 
                    onChange={handleChange} 
                    className="form-control" 
                    step="0.01" 
                    min="0"
                    placeholder="0.00"
                    required 
                  />
                </div>
                <div className="form-group">
                  <label>Estoque</label>
                  <input 
                    type="number" 
                    name="stockQuantity" 
                    value={form.stockQuantity} 
                    onChange={handleChange} 
                    className="form-control" 
                    min="0"
                    placeholder="0"
                  />
                </div>
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label>SKU</label>
                  <input 
                    type="text" 
                    name="sku" 
                    value={form.sku} 
                    onChange={handleChange} 
                    className="form-control"
                    placeholder="Ex: PROD-001"
                  />
                </div>
                <div className="form-group">
                  <label>Categoria *</label>
                  <select 
                    name="categoryId" 
                    value={form.categoryId} 
                    onChange={handleChange} 
                    className="form-control"
                    required
                  >
                    <option value="">Selecione...</option>
                    {categories.map(cat => (
                      <option key={cat.id} value={cat.id}>{cat.name}</option>
                    ))}
                  </select>
                </div>
              </div>
            </div>
          </div>

          <div className="form-side">
            <div className="form-section">
              <h3>Imagem do Produto</h3>
              
              <div className="image-upload-area">
                {form.imageUrl ? (
                  <div className="image-preview">
                    <img src={form.imageUrl} alt="Preview" />
                    <button type="button" className="remove-image" onClick={removeImage}>
                      <FiX />
                    </button>
                  </div>
                ) : (
                  <label className="upload-placeholder">
                    <input 
                      type="file" 
                      accept="image/jpeg,image/png,image/gif,image/webp"
                      onChange={handleImageUpload}
                      disabled={uploading}
                      hidden
                    />
                    {uploading ? (
                      <>
                        <div className="spinner-small"></div>
                        <span>Enviando...</span>
                      </>
                    ) : (
                      <>
                        <FiUpload />
                        <span>Clique para enviar imagem</span>
                        <small>JPG, PNG, GIF ou WEBP até 5MB</small>
                      </>
                    )}
                  </label>
                )}
              </div>

              <div className="form-group">
                <label>Ou cole a URL da imagem</label>
                <input 
                  type="url" 
                  name="imageUrl" 
                  value={form.imageUrl} 
                  onChange={handleChange} 
                  className="form-control"
                  placeholder="https://..."
                />
              </div>

              <div className="form-group">
                <label className="checkbox">
                  <input 
                    type="checkbox" 
                    name="isActive" 
                    checked={form.isActive} 
                    onChange={handleChange} 
                  />
                  Produto ativo
                </label>
              </div>
            </div>
          </div>
        </div>

        <div className="form-actions">
          <Link to="/admin/products" className="btn btn-outline">Cancelar</Link>
          <button type="submit" className="btn btn-primary" disabled={loading || uploading}>
            <FiSave /> {loading ? 'Salvando...' : 'Salvar Produto'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default ProductForm;
