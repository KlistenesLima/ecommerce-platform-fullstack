import { useState } from 'react';
import { useAuth } from '../../contexts/AuthContext';
import { FiUser, FiMail, FiPhone, FiSave } from 'react-icons/fi';
import { toast } from 'react-toastify';
import './Profile.css';

const Profile = () => {
  const { user } = useAuth();
  const [formData, setFormData] = useState({
    firstName: user?.firstName || '',
    lastName: user?.lastName || '',
    email: user?.email || '',
    phone: user?.phoneNumber || ''
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    toast.success('Perfil atualizado com sucesso!');
  };

  return (
    <main className="profile-page">
      <div className="container">
        <h1>Meu Perfil</h1>

        <div className="profile-content">
          <div className="profile-card">
            <div className="profile-avatar">
              <FiUser />
            </div>
            <h3>{formData.firstName} {formData.lastName}</h3>
            <p>{formData.email}</p>
          </div>

          <form onSubmit={handleSubmit} className="profile-form">
            <h3>Informações Pessoais</h3>
            
            <div className="form-row">
              <div className="form-group">
                <label>Nome</label>
                <div className="input-wrapper">
                  <FiUser className="input-icon" />
                  <input
                    type="text"
                    name="firstName"
                    className="form-control"
                    value={formData.firstName}
                    onChange={handleChange}
                  />
                </div>
              </div>
              <div className="form-group">
                <label>Sobrenome</label>
                <div className="input-wrapper">
                  <FiUser className="input-icon" />
                  <input
                    type="text"
                    name="lastName"
                    className="form-control"
                    value={formData.lastName}
                    onChange={handleChange}
                  />
                </div>
              </div>
            </div>

            <div className="form-group">
              <label>Email</label>
              <div className="input-wrapper">
                <FiMail className="input-icon" />
                <input
                  type="email"
                  name="email"
                  className="form-control"
                  value={formData.email}
                  onChange={handleChange}
                  disabled
                />
              </div>
            </div>

            <div className="form-group">
              <label>Telefone</label>
              <div className="input-wrapper">
                <FiPhone className="input-icon" />
                <input
                  type="tel"
                  name="phone"
                  className="form-control"
                  value={formData.phone}
                  onChange={handleChange}
                />
              </div>
            </div>

            <button type="submit" className="btn btn-primary">
              <FiSave /> Salvar Alterações
            </button>
          </form>
        </div>
      </div>
    </main>
  );
};

export default Profile;
