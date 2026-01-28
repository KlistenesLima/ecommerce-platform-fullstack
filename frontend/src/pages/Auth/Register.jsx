import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { FiMail, FiLock, FiUser, FiPhone, FiEye, FiEyeOff } from 'react-icons/fi';
import { toast } from 'react-toastify';
import { useAuth } from '../../contexts/AuthContext';
import './Auth.css'; // Importa o CSS unificado

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
        <div className="auth-container">
            <div className="auth-card">
                {/* Cabeçalho com Logo */}
                <div className="auth-header">
                    <Link to="/" className="auth-logo">
                        <span className="logo-text">LUXE</span>
                        <span className="logo-accent">Store</span>
                    </Link>
                    <h2>Criar conta</h2>
                    <p>Preencha os dados para se cadastrar</p>
                </div>

                <form onSubmit={handleSubmit(onSubmit)} className="auth-form">

                    {/* Nome */}
                    <div className="form-group">
                        <label className="form-label">Nome</label>
                        <div className="icon-input">
                            <FiUser />
                            <input
                                type="text"
                                className={errors.firstName ? 'error' : ''}
                                placeholder="Seu nome"
                                {...register('firstName')}
                            />
                        </div>
                        {errors.firstName && <span className="error-message">{errors.firstName.message}</span>}
                    </div>

                    {/* Sobrenome */}
                    <div className="form-group">
                        <label className="form-label">Sobrenome</label>
                        <div className="icon-input">
                            <FiUser />
                            <input
                                type="text"
                                className={errors.lastName ? 'error' : ''}
                                placeholder="Seu sobrenome"
                                {...register('lastName')}
                            />
                        </div>
                        {errors.lastName && <span className="error-message">{errors.lastName.message}</span>}
                    </div>

                    {/* Email */}
                    <div className="form-group">
                        <label className="form-label">Email</label>
                        <div className="icon-input">
                            <FiMail />
                            <input
                                type="email"
                                className={errors.email ? 'error' : ''}
                                placeholder="seu@email.com"
                                {...register('email')}
                            />
                        </div>
                        {errors.email && <span className="error-message">{errors.email.message}</span>}
                    </div>

                    {/* Telefone */}
                    <div className="form-group">
                        <label className="form-label">Telefone</label>
                        <div className="icon-input">
                            <FiPhone />
                            <input
                                type="tel"
                                className={errors.phone ? 'error' : ''}
                                placeholder="(11) 99999-9999"
                                {...register('phone')}
                            />
                        </div>
                        {errors.phone && <span className="error-message">{errors.phone.message}</span>}
                    </div>

                    {/* Senha */}
                    <div className="form-group">
                        <label className="form-label">Senha</label>
                        <div className="icon-input">
                            <FiLock />
                            <input
                                type={showPassword ? 'text' : 'password'}
                                className={errors.password ? 'error' : ''}
                                placeholder="••••••••"
                                {...register('password')}
                            />
                            <button
                                type="button"
                                className="toggle-password"
                                onClick={() => setShowPassword(!showPassword)}
                                tabIndex="-1"
                            >
                                {showPassword ? <FiEyeOff /> : <FiEye />}
                            </button>
                        </div>
                        {errors.password && <span className="error-message">{errors.password.message}</span>}
                    </div>

                    {/* Confirmar Senha */}
                    <div className="form-group">
                        <label className="form-label">Confirmar Senha</label>
                        <div className="icon-input">
                            <FiLock />
                            <input
                                type={showPassword ? 'text' : 'password'}
                                className={errors.confirmPassword ? 'error' : ''}
                                placeholder="••••••••"
                                {...register('confirmPassword')}
                            />
                        </div>
                        {errors.confirmPassword && <span className="error-message">{errors.confirmPassword.message}</span>}
                    </div>

                    {/* Termos */}
                    <div className="form-group">
                        <label className="checkbox-label">
                            <input type="checkbox" required />
                            Li e aceito os <Link to="/terms">termos de uso</Link>
                        </label>
                    </div>

                    <button type="submit" className="btn-block" disabled={loading}>
                        {loading ? 'Cadastrando...' : 'Criar Conta'}
                    </button>
                </form>

                <div className="auth-footer">
                    <p>Já tem uma conta? <Link to="/login">Faça login</Link></p>
                </div>
            </div>
        </div>
    );
};

export default Register;