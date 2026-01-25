import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FiPlus, FiEdit2, FiTrash2, FiSearch } from 'react-icons/fi';
import api from '../../services/api';
import { toast } from 'react-toastify';
import './AdminPages.css';

const AdminCategories = () => {
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');

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
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (id) => {
        if (!window.confirm('Tem certeza que deseja excluir esta categoria?')) return;

        try {
            await api.delete(`/categories/${id}`);
            setCategories(categories.filter(c => c.id !== id));
            toast.success('Categoria excluída!');
        } catch (error) {
            toast.error('Erro ao excluir categoria');
        }
    };

    const filtered = categories.filter(c =>
        c.name?.toLowerCase().includes(search.toLowerCase())
    );

    return (
        <div className="admin-page">
            <div className="page-header">
                <div>
                    <h1>Categorias</h1>
                    <p>{categories.length} categorias cadastradas</p>
                </div>
                {/* Link direto para a página de criação, igual Produtos */}
                <Link to="/admin/categories/new" className="btn btn-primary">
                    <FiPlus /> Nova Categoria
                </Link>
            </div>

            <div className="search-box">
                <FiSearch />
                <input
                    type="text"
                    placeholder="Buscar categorias..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                />
            </div>

            <div className="table-container">
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>Nome</th>
                            <th>Descrição</th>
                            <th width="150">Produtos</th>
                            <th width="120" className="text-end">Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        {loading ? (
                            <tr><td colSpan="4" className="center">Carregando...</td></tr>
                        ) : filtered.length === 0 ? (
                            <tr><td colSpan="4" className="center">Nenhuma categoria encontrada</td></tr>
                        ) : filtered.map(category => (
                            <tr key={category.id}>
                                <td><strong>{category.name}</strong></td>
                                <td className="text-muted">{category.description || '-'}</td>
                                <td>
                                    <span className="stock-badge ok">
                                        {category.productsCount || 0}
                                    </span>
                                </td>
                                <td>
                                    <div className="actions justify-content-end">
                                        {/* Link para edição passando o ID na URL */}
                                        <Link to={`/admin/categories/${category.id}/edit`} className="btn-icon edit" title="Editar">
                                            <FiEdit2 />
                                        </Link>
                                        <button onClick={() => handleDelete(category.id)} className="btn-icon delete" title="Excluir">
                                            <FiTrash2 />
                                        </button>
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

export default AdminCategories;