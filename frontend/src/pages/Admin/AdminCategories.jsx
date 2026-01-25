import { useState, useEffect } from 'react';
import { FiPlus, FiEdit2, FiTrash2, FiSearch } from 'react-icons/fi';
import { Modal, Button, Form } from 'react-bootstrap'; // Usamos apenas o Modal do Bootstrap para funcionalidade
import api from '../../services/api';
import { toast } from 'react-toastify';
import './AdminPages.css';

const AdminCategories = () => {
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');

    // Estado do Modal
    const [showModal, setShowModal] = useState(false);
    const [editingCategory, setEditingCategory] = useState(null);
    const [saving, setSaving] = useState(false);
    const [form, setForm] = useState({ name: '', description: '' });

    useEffect(() => {
        loadCategories();
    }, []);

    const loadCategories = async () => {
        try {
            setLoading(true);
            // Usando api direta para manter consistência com o seu padrão, 
            // ou use categoryService se preferir a refatoração anterior.
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

    // Filtragem local
    const filtered = categories.filter(c =>
        c.name?.toLowerCase().includes(search.toLowerCase())
    );

    // --- Lógica do Modal ---
    const handleOpenModal = (category = null) => {
        setEditingCategory(category);
        setForm(category ? { name: category.name, description: category.description || '' } : { name: '', description: '' });
        setShowModal(true);
    };

    const handleCloseModal = () => {
        setShowModal(false);
        setEditingCategory(null);
        setForm({ name: '', description: '' });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!form.name.trim()) {
            toast.warn('O nome da categoria é obrigatório');
            return;
        }

        try {
            setSaving(true);
            if (editingCategory) {
                // Editar
                const response = await api.put(`/categories/${editingCategory.id}`, form);
                setCategories(categories.map(c => c.id === editingCategory.id ? response.data : c));
                toast.success('Categoria atualizada!');
            } else {
                // Criar
                const response = await api.post('/categories', form);
                setCategories([...categories, response.data]);
                toast.success('Categoria criada!');
            }
            handleCloseModal();
        } catch (error) {
            console.error('Erro ao salvar:', error);
            toast.error(error.response?.data?.message || 'Erro ao salvar categoria');
        } finally {
            setSaving(false);
        }
    };

    const handleDelete = async (id) => {
        if (!window.confirm('Tem certeza que deseja excluir esta categoria?')) return;

        try {
            await api.delete(`/categories/${id}`);
            setCategories(categories.filter(c => c.id !== id));
            toast.success('Categoria excluída!');
        } catch (error) {
            console.error('Erro ao excluir:', error);
            toast.error('Erro ao excluir categoria');
        }
    };

    return (
        <div className="admin-page">
            {/* Cabeçalho igual ao de Produtos */}
            <div className="page-header">
                <div>
                    <h1>Categorias</h1>
                    <p>{categories.length} categorias cadastradas</p>
                </div>
                <button onClick={() => handleOpenModal()} className="btn btn-primary">
                    <FiPlus /> Nova Categoria
                </button>
            </div>

            {/* Barra de Busca igual ao de Produtos */}
            <div className="search-box">
                <FiSearch />
                <input
                    type="text"
                    placeholder="Buscar categorias..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                />
            </div>

            {/* Tabela com as mesmas classes de Produtos */}
            <div className="table-container">
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>Nome</th>
                            <th>Descrição</th>
                            <th width="150">Produtos</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        {loading ? (
                            <tr><td colSpan="4" className="center">Carregando...</td></tr>
                        ) : filtered.length === 0 ? (
                            <tr><td colSpan="4" className="center">Nenhuma categoria encontrada</td></tr>
                        ) : filtered.map(category => (
                            <tr key={category.id}>
                                <td>
                                    <strong>{category.name}</strong>
                                </td>
                                <td>{category.description || '-'}</td>
                                <td>
                                    <span className="stock-badge ok">
                                        {category.productsCount || 0}
                                    </span>
                                </td>
                                <td>
                                    <div className="actions">
                                        <button onClick={() => handleOpenModal(category)} className="btn-icon edit">
                                            <FiEdit2 />
                                        </button>
                                        <button onClick={() => handleDelete(category.id)} className="btn-icon delete">
                                            <FiTrash2 />
                                        </button>
                                    </div>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            {/* Modal do React Bootstrap (Para funcionar corretamente o z-index/backdrop) */}
            <Modal show={showModal} onHide={handleCloseModal} centered>
                <Modal.Header closeButton>
                    <Modal.Title>{editingCategory ? 'Editar Categoria' : 'Nova Categoria'}</Modal.Title>
                </Modal.Header>
                <Form onSubmit={handleSubmit}>
                    <Modal.Body>
                        <Form.Group className="mb-3">
                            <Form.Label>Nome *</Form.Label>
                            <Form.Control
                                type="text"
                                value={form.name}
                                onChange={(e) => setForm({ ...form, name: e.target.value })}
                                placeholder="Ex: Eletrônicos"
                                autoFocus
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Descrição</Form.Label>
                            <Form.Control
                                as="textarea"
                                rows={3}
                                value={form.description}
                                onChange={(e) => setForm({ ...form, description: e.target.value })}
                                placeholder="Descreva a categoria..."
                            />
                        </Form.Group>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleCloseModal}>
                            Cancelar
                        </Button>
                        <Button variant="primary" type="submit" disabled={saving}>
                            {saving ? 'Salvando...' : editingCategory ? 'Atualizar' : 'Criar'}
                        </Button>
                    </Modal.Footer>
                </Form>
            </Modal>
        </div>
    );
};

export default AdminCategories;