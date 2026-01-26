import { useState, useEffect } from 'react';
import { FiTrash2, FiSearch, FiUser } from 'react-icons/fi';
import userService from '../../services/userService';
import { toast } from 'react-toastify';
import './AdminPages.css';

const AdminUsers = () => {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');

    useEffect(() => {
        loadUsers();
    }, []);

    const loadUsers = async () => {
        try {
            setLoading(true);
            const data = await userService.getAll();
            setUsers(Array.isArray(data) ? data : []);
        } catch (error) {
            console.error('Erro ao carregar usuários:', error);
            toast.error('Erro ao carregar lista de usuários');
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (id) => {
        if (!window.confirm('Tem certeza que deseja excluir este usuário? Esta ação não pode ser desfeita.')) return;

        try {
            await userService.delete(id);
            setUsers(users.filter(user => user.id !== id));
            toast.success('Usuário excluído com sucesso!');
        } catch (error) {
            console.error(error);
            toast.error('Erro ao excluir usuário. Verifique se ele possui pedidos vinculados.');
        }
    };

    // Filtragem local por nome ou email
    const filtered = users.filter(user =>
        user.name?.toLowerCase().includes(search.toLowerCase()) ||
        user.email?.toLowerCase().includes(search.toLowerCase())
    );

    return (
        <div className="admin-page">
            <div className="page-header">
                <div>
                    <h1>Usuários</h1>
                    <p>{users.length} usuários cadastrados</p>
                </div>
            </div>

            <div className="search-box">
                <FiSearch />
                <input
                    type="text"
                    placeholder="Buscar por nome ou email..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                />
            </div>

            <div className="table-container">
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>Usuário</th>
                            <th>Email</th>
                            <th>Função</th>
                            <th>Data Cadastro</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        {loading ? (
                            <tr><td colSpan="5" className="center">Carregando...</td></tr>
                        ) : filtered.length === 0 ? (
                            <tr><td colSpan="5" className="center">Nenhum usuário encontrado</td></tr>
                        ) : filtered.map(user => (
                            <tr key={user.id}>
                                <td>
                                    <div className="user-cell">
                                        <div className="avatar">
                                            <FiUser />
                                        </div>
                                        <strong>{user.name}</strong>
                                    </div>
                                </td>
                                <td>{user.email}</td>
                                <td>
                                    {/* Verifica se é Admin ou Cliente e muda a cor do badge */}
                                    <span className={`role-badge ${user.role === 'admin' ? 'admin' : 'cliente'}`}>
                                        {user.role === 'admin' ? 'Administrador' : 'Cliente'}
                                    </span>
                                </td>
                                <td>
                                    {user.createdAt ? new Date(user.createdAt).toLocaleDateString('pt-BR') : '-'}
                                </td>
                                <td>
                                    <div className="actions">
                                        <button
                                            onClick={() => handleDelete(user.id)}
                                            className="btn-icon delete"
                                            title="Excluir Usuário"
                                        >
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

export default AdminUsers;