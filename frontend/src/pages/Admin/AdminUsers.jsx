import { useState, useEffect } from 'react';
import { FiSearch, FiMail, FiLock, FiUnlock } from 'react-icons/fi';
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
      // const data = await userService.getAll();
      setUsers([
        { id: 1, firstName: 'João', lastName: 'Silva', email: 'joao@email.com', role: 'Cliente', ordersCount: 5, isActive: true, createdAt: '2026-01-15' },
        { id: 2, firstName: 'Maria', lastName: 'Santos', email: 'maria@email.com', role: 'Cliente', ordersCount: 3, isActive: true, createdAt: '2026-01-10' },
        { id: 3, firstName: 'Pedro', lastName: 'Costa', email: 'pedro@email.com', role: 'Admin', ordersCount: 0, isActive: true, createdAt: '2026-01-01' },
        { id: 4, firstName: 'Ana', lastName: 'Oliveira', email: 'ana@email.com', role: 'Cliente', ordersCount: 8, isActive: false, createdAt: '2025-12-20' },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const toggleStatus = (id) => {
    setUsers(users.map(u => u.id === id ? { ...u, isActive: !u.isActive } : u));
    toast.success('Status atualizado!');
  };

  const filtered = users.filter(u => 
    u.firstName.toLowerCase().includes(search.toLowerCase()) ||
    u.lastName.toLowerCase().includes(search.toLowerCase()) ||
    u.email.toLowerCase().includes(search.toLowerCase())
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
          placeholder="Buscar usuários..."
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
              <th>Tipo</th>
              <th>Pedidos</th>
              <th>Status</th>
              <th>Cadastro</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr><td colSpan="7" className="center">Carregando...</td></tr>
            ) : filtered.length === 0 ? (
              <tr><td colSpan="7" className="center">Nenhum usuário encontrado</td></tr>
            ) : filtered.map(user => (
              <tr key={user.id}>
                <td>
                  <div className="user-cell">
                    <div className="avatar">{user.firstName[0]}{user.lastName[0]}</div>
                    <span>{user.firstName} {user.lastName}</span>
                  </div>
                </td>
                <td>{user.email}</td>
                <td><span className={`role-badge ${user.role.toLowerCase()}`}>{user.role}</span></td>
                <td>{user.ordersCount}</td>
                <td><span className={`badge ${user.isActive ? 'entregue' : 'cancelado'}`}>{user.isActive ? 'Ativo' : 'Inativo'}</span></td>
                <td>{user.createdAt}</td>
                <td>
                  <div className="actions">
                    <button className="btn-icon" title="Enviar email"><FiMail /></button>
                    <button 
                      onClick={() => toggleStatus(user.id)} 
                      className={`btn-icon ${user.isActive ? 'delete' : 'edit'}`}
                      title={user.isActive ? 'Bloquear' : 'Desbloquear'}
                    >
                      {user.isActive ? <FiLock /> : <FiUnlock />}
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
