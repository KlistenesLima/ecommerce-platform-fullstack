import { useState, useEffect } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { FiArrowLeft, FiSave } from 'react-icons/fi';
import api from '../../services/api';
import { toast } from 'react-toastify';
import './AdminPages.css';

const CategoryForm = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [saving, setSaving] = useState(false);

    const [formData, setFormData] = useState({
        name: '',
        description: ''
    });

    useEffect(() => {
        if (id) {
            loadCategory(id);
        }
    }, [id]);

    const loadCategory = async (categoryId) => {
        try {
            setLoading(true);
            const response = await api.get(`/categories/${categoryId}`);
            setFormData({
                name: response.data.name,
                description: response.data.description || ''
            });
        } catch (error) {
            toast.error('Erro ao carregar categoria');
            navigate('/admin/categories');
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!formData.name.trim()) {
            toast.warning('O nome é obrigatório');
            return;
        }

        try {
            setSaving(true);
            if (id) {
                await api.put(`/categories/${id}`, formData);
                toast.success('Categoria atualizada com sucesso!');
            } else {
                await api.post('/categories', formData);
                toast.success('Categoria criada com sucesso!');
            }
            navigate('/admin/categories');
        } catch (error) {
            console.error(error);
            toast.error(error.response?.data?.message || 'Erro ao salvar categoria');
        } finally {
            setSaving(false);
        }
    };

    if (loading) return <div className="admin-page"><p className="center">Carregando...</p></div>;

    return (
        <div className="admin-page">
            <div className="page-header">
                <div>
                    <Link to="/admin/categories" className="back-link">
                        <FiArrowLeft /> Voltar para categorias
                    </Link>
                    <h1>{id ? 'Editar Categoria' : 'Nova Categoria'}</h1>
                </div>
            </div>

            <form onSubmit={handleSubmit} className="admin-form">
                <div className="form-section">
                    <h3>Informações Básicas</h3>

                    <div className="form-group">
                        <label>Nome da Categoria *</label>
                        <input
                            type="text"
                            className="form-control"
                            value={formData.name}
                            onChange={e => setFormData({ ...formData, name: e.target.value })}
                            placeholder="Ex: Smartphones"
                            required
                        />
                    </div>

                    <div className="form-group">
                        <label>Descrição</label>
                        <textarea
                            className="form-control"
                            rows="4"
                            value={formData.description}
                            onChange={e => setFormData({ ...formData, description: e.target.value })}
                            placeholder="Digite uma descrição para a categoria..."
                        />
                    </div>

                    <div className="form-actions">
                        <Link to="/admin/categories" className="btn btn-outline">
                            Cancelar
                        </Link>
                        <button type="submit" className="btn btn-primary" disabled={saving}>
                            {saving ? 'Salvando...' : <><FiSave /> Salvar Categoria</>}
                        </button>
                    </div>
                </div>
            </form>
        </div>
    );
};

export default CategoryForm;