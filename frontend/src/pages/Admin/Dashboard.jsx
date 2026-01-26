import { useState, useEffect } from 'react';
import { FiDollarSign, FiShoppingBag, FiUsers, FiBox } from 'react-icons/fi';
import api from '../../services/api'; // O caminho da API deve estar correto
import './Dashboard.css';

const Dashboard = () => {
    const [stats, setStats] = useState({
        totalSales: 0,
        totalOrders: 0,
        totalUsers: 0,
        totalProducts: 0
    });
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadDashboardData();
    }, []);

    const loadDashboardData = async () => {
        try {
            setLoading(true);

            // 1. Fazemos todas as chamadas direto pela API para evitar erros de importação
            const [ordersRes, usersRes, productsRes] = await Promise.allSettled([
                api.get('/orders'),
                api.get('/users'),
                api.get('/products')
            ]);

            console.log("Resultados do Dashboard:", { ordersRes, usersRes, productsRes });

            // 2. Processar Pedidos
            let orders = [];
            let totalSales = 0;
            if (ordersRes.status === 'fulfilled') {
                // Garante que é um array
                orders = Array.isArray(ordersRes.value.data) ? ordersRes.value.data : [];

                // Soma segura: converte para Number e se der NaN usa 0
                totalSales = orders.reduce((acc, order) => {
                    const valor = Number(order.totalAmount);
                    return acc + (isNaN(valor) ? 0 : valor);
                }, 0);
            }

            // 3. Processar Usuários
            let totalUsers = 0;
            if (usersRes.status === 'fulfilled') {
                const data = usersRes.value.data;
                // Backend pode retornar o array direto ou um objeto { data: [...] }
                const listaUsers = Array.isArray(data) ? data : (data?.data || []);
                totalUsers = listaUsers.length;
            }

            // 4. Processar Produtos
            let totalProducts = 0;
            if (productsRes.status === 'fulfilled') {
                const data = productsRes.value.data;
                const listaProdutos = Array.isArray(data) ? data : (data?.data || []);
                totalProducts = listaProdutos.length;
            }

            // 5. Atualiza o estado
            setStats({
                totalSales,
                totalOrders: orders.length,
                totalUsers,
                totalProducts
            });

        } catch (error) {
            console.error('Erro crítico no Dashboard:', error);
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return (
            <div className="admin-page">
                <div className="page-header">
                    <h1>Dashboard</h1>
                </div>
                <div className="loading-dashboard">Carregando dados...</div>
            </div>
        );
    }

    return (
        <div className="admin-page">
            <div className="page-header">
                <h1>Dashboard</h1>
                <p>Visão geral da sua loja</p>
            </div>

            <div className="stats-grid">
                {/* Card Vendas */}
                <div className="stat-card">
                    <div className="stat-icon sales">
                        <FiDollarSign />
                    </div>
                    <div className="stat-info">
                        <h3>Vendas Totais</h3>
                        <p>
                            {new Intl.NumberFormat('pt-BR', {
                                style: 'currency',
                                currency: 'BRL'
                            }).format(stats.totalSales)}
                        </p>
                    </div>
                </div>

                {/* Card Pedidos */}
                <div className="stat-card">
                    <div className="stat-icon orders">
                        <FiShoppingBag />
                    </div>
                    <div className="stat-info">
                        <h3>Pedidos</h3>
                        <p>{stats.totalOrders}</p>
                    </div>
                </div>

                {/* Card Usuários */}
                <div className="stat-card">
                    <div className="stat-icon users">
                        <FiUsers />
                    </div>
                    <div className="stat-info">
                        <h3>Usuários</h3>
                        <p>{stats.totalUsers}</p>
                    </div>
                </div>

                {/* Card Produtos */}
                <div className="stat-card">
                    <div className="stat-icon products">
                        <FiBox />
                    </div>
                    <div className="stat-info">
                        <h3>Produtos</h3>
                        <p>{stats.totalProducts}</p>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Dashboard;