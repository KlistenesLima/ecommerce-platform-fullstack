import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { FiMail, FiLock, FiUser, FiPhone, FiEye, FiEyeOff } from 'react-icons/fi';
import { toast } from 'react-toastify';
import { useAuth } from '../../contexts/AuthContext';
import './Auth.css';

const schema = yup.object({
  firstName: yup.string().required('Nome é obrigatório'),
  lastName: yup.string().required('Sobrenome é obrigatório'),
  email: yup.string().email('Email inválido').required('Email é obrigatório'),
  phone: yup.string().required('Telefone é obrigatório'),
  password: yup.string().min(6, 'Mínimo 6 caracteres').required('Senha é obrigatória'),
  confirmPassword: yup.string()
    .oneOf([yup.ref('password')], 'Senhas não conferem')
    .required('Confirmação é obrigatória'),
});

const Register = () => {
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const { register: registerUser } = useAuth();
  const navigate = useNavigate();

  const { register, handleSubmit, formState: { errors } } = useForm({
    resolver: yupResolver(schema)
  });

  const onSubmit = async (data) => {
    try {
      setLoading(true);
      await registerUser({
        firstName: data.firstName,
        lastName: data.lastName,
        email: data.email,
        phoneNumber: data.phone,
        password: data.password
      });
      toast.success('Cadastro realizado! Faça login para continuar.');
      navigate('/login');
    } catch (error) {
      toast.error(error.response?.data?.message || 'Erro ao cadastrar');
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="auth-page">
      <div className="auth-container">
        <div className="auth-card">
          <div className="auth-header">
            <Link to="/" className="auth-logo">
              <span className="logo-text">LUXE</span>
              <span className="logo-accent">Store</span>
            </Link>
            <h1>Criar conta</h1>
            <p>Preencha os dados para se cadastrar</p>
          </div>

          <form onSubmit={handleSubmit(onSubmit)} className="auth-form">
            <div className="input-row">
              <div className="form-group">
                <label className="form-label">Nome</label>
                <div className="input-wrapper">
                  <FiUser className="input-icon" />
                  <input
                    type="text"
                    className={`form-control ${errors.firstName ? 'error' : ''}`}
                    placeholder="Seu nome"
                    {...register('firstName')}
                  />
                </div>
                {errors.firstName && <span className="error-message">{errors.firstName.message}</span>}
              </div>

              <div className="form-group">
                <label className="form-label">Sobrenome</label>
                <div className="input-wrapper">
                  <FiUser className="input-icon" />
                  <input
                    type="text"
                    className={`form-control ${errors.lastName ? 'error' : ''}`}
                    placeholder="Seu sobrenome"
                    {...register('lastName')}
                  />
                </div>
                {errors.lastName && <span className="error-message">{errors.lastName.message}</span>}
              </div>
            </div>

            <div className="form-group">
              <label className="form-label">Email</label>
              <div className="input-wrapper">
                <FiMail className="input-icon" />
                <input
                  type="email"
                  className={`form-control ${errors.email ? 'error' : ''}`}
                  placeholder="seu@email.com"
                  {...register('email')}
                />
              </div>
              {errors.email && <span className="error-message">{errors.email.message}</span>}
            </div>

            <div className="form-group">
              <label className="form-label">Telefone</label>
              <div className="input-wrapper">
                <FiPhone className="input-icon" />
                <input
                  type="tel"
                  className={`form-control ${errors.phone ? 'error' : ''}`}
                  placeholder="(11) 99999-9999"
                  {...register('phone')}
                />
              </div>
              {errors.phone && <span className="error-message">{errors.phone.message}</span>}
            </div>

            <div className="input-row">
              <div className="form-group">
                <label className="form-label">Senha</label>
                <div className="input-wrapper">
                  <FiLock className="input-icon" />
                  <input
                    type={showPassword ? 'text' : 'password'}
                    className={`form-control ${errors.password ? 'error' : ''}`}
                    placeholder="••••••••"
                    {...register('password')}
                  />
                  <button
                    type="button"
                    className="toggle-password"
                    onClick={() => setShowPassword(!showPassword)}
                  >
                    {showPassword ? <FiEyeOff /> : <FiEye />}
                  </button>
                </div>
                {errors.password && <span className="error-message">{errors.password.message}</span>}
              </div>

              <div className="form-group">
                <label className="form-label">Confirmar Senha</label>
                <div className="input-wrapper">
                  <FiLock className="input-icon" />
                  <input
                    type={showPassword ? 'text' : 'password'}
                    className={`form-control ${errors.confirmPassword ? 'error' : ''}`}
                    placeholder="••••••••"
                    {...register('confirmPassword')}
                  />
                </div>
                {errors.confirmPassword && <span className="error-message">{errors.confirmPassword.message}</span>}
              </div>
            </div>

            <div className="form-options">
              <label className="checkbox-label">
                <input type="checkbox" required />
                <span className="checkmark"></span>
                Aceito os <Link to="/terms">termos de uso</Link>
              </label>
            </div>

            <button type="submit" className="btn btn-primary btn-block" disabled={loading}>
              {loading ? 'Cadastrando...' : 'Criar Conta'}
            </button>
          </form>

          <div className="auth-footer">
            <p>Já tem uma conta? <Link to="/login">Faça login</Link></p>
          </div>
        </div>

        <div className="auth-banner">
          <div className="banner-content">
            <h2>Junte-se a nós</h2>
            <p>Ganhe 10% de desconto na primeira compra</p>
          </div>
        </div>
      </div>
    </main>
  );
};

export default Register;
