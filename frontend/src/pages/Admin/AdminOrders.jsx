import { useState, useEffect } from 'react';
import { FiEye, FiSearch, FiTruck, FiCheckCircle, FiClock, FiXCircle } from 'react-icons/fi';
import orderService from '../../services/orderService';
import { toast } from 'react-toastify';
import './AdminPages.css';

const AdminOrders = () => {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');

    useEffect(() => {
        loadOrders();
    }, []);

    const loadOrders = async () => {
        try {
            setLoading(true);
            const data = await orderService.getAll();

            // Garante que é um array, mesmo se a API retornar algo diferente
            if (Array.isArray(data)) {
                setOrders(data);
            } else if (data && data.data && Array.isArray(data.data)) {
                // Caso a API retorne paginado dentro de um objeto 'data'
                setOrders(data.data);
            } else {
                setOrders([]);
            }
        } catch (error) {
            console.error('Erro ao buscar pedidos:', error);
            toast.error('Erro ao carregar lista de pedidos');
        } finally {
            setLoading(false);
        }
    };

    // Função para deixar o status bonito e colorido
    const getStatusInfo = (status) => {
        // Converte para string e minúsculo para facilitar a comparação
        const s = String(status).toLowerCase();

        // Mapeamento baseado no seu Backend (Enum ou String)
        // Ajuste os números se o seu Enum for diferente
        switch (s) {
            case '1':
            case 'paid':
            case 'approved':
                return { label: 'Pago', class: 'status-success', icon: <FiCheckCircle /> };
            case '2':
            case 'shipped':
                return { label: 'Enviado', class: 'status-warning', icon: <FiTruck /> };
            case '3':
            case 'delivered':
                return { label: 'Entregue', class: 'status-success', icon: <FiCheckCircle /> };
            case '4':
            case 'canceled':
            case 'cancelled':
                return { label: 'Cancelado', class: 'status-danger', icon: <FiXCircle /> };
            case '0':
            case 'pending':
            default:
                return { label: 'Pendente', class: 'status-warning', icon: <FiClock /> };
        }
    };

    // Filtragem (pelo ID ou valor)
    const filteredOrders = orders.filter(order =>
        (order.id && order.id.toLowerCase().includes(search.toLowerCase())) ||
        (order.userId && order.userId.toLowerCase().includes(search.toLowerCase()))
    );

    return (
        <div className="admin-page">
            <div className="page-header">
                <div>
                    <h1>Pedidos</h1>
                    <p>Gerencie as vendas da loja</p>
                </div>
            </div>

            <div className="search-box">
                <FiSearch />
                <input
                    type="text"
                    placeholder="Buscar por ID do pedido ou cliente..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                />
            </div>

            <div className="table-container">
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>ID do Pedido</th>
                            <th>Data</th>
                            <th>Cliente</th>
                            <th>Valor Total</th>
                            <th>Status</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        {loading ? (
                            <tr><td colSpan="6" className="center">Carregando pedidos...</td></tr>
                        ) : filteredOrders.length === 0 ? (
                            <tr><td colSpan="6" className="center">Nenhum pedido encontrado.</td></tr>
                        ) : filteredOrders.map(order => {
                            const statusInfo = getStatusInfo(order.status);
                            return (
                                <tr key={order.id}>
                                    <td className="font-mono" title={order.id}>
                                        #{order.id.substring(0, 8)}... {/* Mostra só o começo do ID */}
                                    </td>
                                    <td>
                                        {new Date(order.orderDate || order.createdAt).toLocaleDateString('pt-BR')}
                                    </td>
                                    <td title={order.userId}>
                                        {/* Tenta mostrar o nome se vier no DTO, senão mostra ID */}
                                        {order.userName || order.firstName || order.userId?.substring(0, 8) + '...'}
                                    </td>
                                    <td>
                                        {new Intl.NumberFormat('pt-BR', {
                                            style: 'currency',
                                            currency: 'BRL'
                                        }).format(order.totalAmount)}
                                    </td>
                                    <td>
                                        <span className={`status-badge ${statusInfo.class}`}>
                                            {statusInfo.icon}
                                            {statusInfo.label}
                                        </span>
                                    </td>
                                    <td>
                                        <div className="actions">
                                            <button className="btn-icon view" title="Ver Detalhes">
                                                <FiEye />
                                            </button>
                                            {/* Botão de Cancelar (opcional) */}
                                            {statusInfo.label !== 'Cancelado' && (
                                                <button
                                                    className="btn-icon delete"
                                                    title="Cancelar Pedido"
                                                    onClick={() => {
                                                        if (window.confirm('Cancelar este pedido?')) {
                                                            orderService.cancel(order.id).then(() => {
                                                                toast.success('Pedido cancelado');
                                                                loadOrders();
                                                            });
                                                        }
                                                    }}
                                                >
                                                    <FiXCircle />
                                                </button>
                                            )}
                                        </div>
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default AdminOrders;