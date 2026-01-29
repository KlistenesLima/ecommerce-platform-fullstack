import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FiArrowRight } from 'react-icons/fi';
// Importando API subindo 2 níveis (corrigindo o erro anterior do Vite)
import api from '../../services/api';
import Loading from '../../components/common/Loading/Loading'; // Reutilizando seu componente de Loading
import './Categories.css';

const Categories = () => {
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadCategories();
    }, []);

    const loadCategories = async () => {
        try {
            setLoading(true);

            // Chamada API
            const response = await api.get('/categories');
            const data = response.data;

            console.log("Dados recebidos:", data); // Para conferência

            // Garante que é um array
            const list = Array.isArray(data) ? data : data.items || [];

            // === CORREÇÃO ===
            // Removemos o filtro de isActive, pois seu Backend não está mandando esse campo.
            // Agora ele vai mostrar tudo que vier da API (os 4 itens do seu print).
            setCategories(list);

        } catch (error) {
            console.error('Erro ao carregar categorias:', error);

            // Mock de segurança caso a API falhe
            setCategories([
                { id: 1, name: 'Eletrônicos', description: 'Gadgets e Tech' },
                { id: 2, name: 'Roupas', description: 'Moda Masculina e Feminina' },
            ]);
        } finally {
            setLoading(false);
        }
    };

    // Gerador de imagem automático (já que o banco ainda não tem foto)
    const getCategoryImage = (name) => {
        return `https://placehold.co/400x300/1a1a1a/gold?text=${encodeURIComponent(name)}`;
    };

    return (
        <main className="categories-page">
            <div className="container">

                {/* Header igual ao Products */}
                <div className="page-header">
                    <div>
                        <h1>Categorias</h1>
                        <p>Explore nossos produtos por departamento</p>
                    </div>
                </div>

                {/* Lógica de Loading e Listagem igual ao Products */}
                {loading ? (
                    <Loading text="Carregando categorias..." />
                ) : categories.length > 0 ? (
                    <div className="categories-grid">
                        {categories.map((category) => (
                            <Link to={`/products?category=${category.id}`} key={category.id} className="category-card">
                                <div className="category-image">
                                    {/* Imagem Placeholder Elegante */}
                                    <img src={getCategoryImage(category.name)} alt={category.name} />
                                    <div className="category-overlay"></div>
                                </div>

                                <div className="category-content">
                                    <h3>{category.name}</h3>
                                    <p>{category.description || 'Confira os produtos'}</p>

                                    <span className="category-link">
                                        Ver produtos <FiArrowRight />
                                    </span>
                                </div>
                            </Link>
                        ))}
                    </div>
                ) : (
                    <div className="empty-state">
                        <h3>Nenhuma categoria encontrada</h3>
                        <p>Em breve novas categorias estarão disponíveis.</p>
                    </div>
                )}
            </div>
        </main>
    );
};

export default Categories;