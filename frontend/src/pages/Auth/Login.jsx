import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { toast } from 'react-toastify';
import { FiMail, FiLock } from 'react-icons/fi';
import './Auth.css';

const Login = () => {
    const [formData, setFormData] = useState({ email: '', password: '' });
    const [loading, setLoading] = useState(false);
    const { signIn } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);

        try {
            // O signIn deve retornar os dados do usuário (incluindo a role)
            const userData = await signIn(formData);

            toast.success(`Bem-vindo, ${userData.name}!`);

            // === AQUI ESTÁ A CORREÇÃO ===
            // Se for admin, manda para o Dashboard. Se não, manda para a Home.
            if (userData.role === 'admin') {
                navigate('/admin');
            } else {
                navigate('/');
            }

        } catch (error) {
            console.error(error);
            toast.error('Email ou senha inválidos');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-container">
            <div className="auth-card">
                <h2>Bem-vindo de volta</h2>
                <p>Acesse sua conta para continuar</p>

                <form onSubmit={handleSubmit}>
                    <div className="form-group icon-input">
                        <FiMail />
                        <input
                            type="email"
                            placeholder="Seu email"
                            value={formData.email}
                            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                            required
                        />
                    </div>

                    <div className="form-group icon-input">
                        <FiLock />
                        <input
                            type="password"
                            placeholder="Sua senha"
                            value={formData.password}
                            onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                            required
                        />
                    </div>

                    <button type="submit" className="btn btn-primary btn-block" disabled={loading}>
                        {loading ? 'Entrando...' : 'Entrar'}
                    </button>
                </form>

                <div className="auth-footer">
                    Não tem uma conta? <Link to="/register">Cadastre-se</Link>
                </div>
            </div>
        </div>
    );
};

export default Login;