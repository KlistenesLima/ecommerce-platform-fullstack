import { useState, useEffect } from 'react';
import { FiPlus, FiEdit2, FiTrash2, FiX } from 'react-icons/fi';
import api from '../../services/api';
import { toast } from 'react-toastify';
import './AdminPages.css';

const AdminCategories = () => {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState(null);
  const [saving, setSaving] = useState(false);
  const [form, setForm] = useState({ name: '', description: '' });

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    try {
      setLoading(true);
      const response = await api.get('/categories');
      setCategories(response.data || []);
    } catch (error) {
      console.error('Erro ao carregar categorias:', error);
      toast.error('Erro ao carregar categorias');
      setCategories([]);
    } finally {
      setLoading(false);
    }
  };

  const openModal = (category = null) => {
    setEditing(category);
    setForm(category ? { name: category.name, description: category.description || '' } : { name: '', description: '' });
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
    setEditing(null);
    setForm({ name: '', description: '' });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!form.name.trim()) {
      toast.error('Nome é obrigatório');
      return;
    }

    try {
      setSaving(true);
      if (editing) {
        const response = await api.put(`/categories/${editing.id}`, form);
        setCategories(categories.map(c => c.id === editing.id ? response.data : c));
        toast.success('Categoria atualizada!');
      } else {
        const response = await api.post('/categories', form);
        setCategories([...categories, response.data]);
        toast.success('Categoria criada!');
      }
      closeModal();
    } catch (error) {
      console.error('Erro ao salvar:', error);
      toast.error(error.response?.data?.message || 'Erro ao salvar categoria');
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Excluir categoria?')) return;
    
    try {
      await api.delete(`/categories/${id}`);
      setCategories(categories.filter(c => c.id !== id));
      toast.success('Categoria excluída!');
    } catch (error) {
      console.error('Erro ao excluir:', error);
      toast.error(error.response?.data?.message || 'Erro ao excluir categoria');
    }
  };

  return (
    <div className="admin-page">
      <div className="page-header">
        <div>
          <h1>Categorias</h1>
          <p>{categories.length} categorias</p>
        </div>
        <button onClick={() => openModal()} className="btn btn-primary">
          <FiPlus /> Nova Categoria
        </button>
      </div>

      <div className="cards-grid">
        {loading ? (
          <p className="center">Carregando...</p>
        ) : categories.length === 0 ? (
          <p className="center">Nenhuma categoria cadastrada</p>
        ) : categories.map(category => (
          <div key={category.id} className="category-card">
            <div className="card-info">
              <h3>{category.name}</h3>
              <p>{category.description || 'Sem descrição'}</p>
              <span className="count">{category.productsCount || 0} produtos</span>
            </div>
            <div className="card-actions">
              <button onClick={() => openModal(category)} className="btn-icon edit"><FiEdit2 /></button>
              <button onClick={() => handleDelete(category.id)} className="btn-icon delete"><FiTrash2 /></button>
            </div>
          </div>
        ))}
      </div>

      {/* Modal */}
      {modalOpen && (
        <div className="modal-overlay" onClick={closeModal}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <div className="modal-header">
              <h2>{editing ? 'Editar Categoria' : 'Nova Categoria'}</h2>
              <button onClick={closeModal} className="close-btn"><FiX /></button>
            </div>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Nome *</label>
                <input
                  type="text"
                  value={form.name}
                  onChange={(e) => setForm({ ...form, name: e.target.value })}
                  className="form-control"
                  placeholder="Ex: Eletrônicos"
                  required
                />
              </div>
              <div className="form-group">
                <label>Descrição</label>
                <textarea
                  value={form.description}
                  onChange={(e) => setForm({ ...form, description: e.target.value })}
                  className="form-control"
                  rows="3"
                  placeholder="Descreva a categoria..."
                />
              </div>
              <div className="modal-actions">
                <button type="button" onClick={closeModal} className="btn btn-outline">Cancelar</button>
                <button type="submit" className="btn btn-primary" disabled={saving}>
                  {saving ? 'Salvando...' : editing ? 'Atualizar' : 'Criar'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default AdminCategories;
