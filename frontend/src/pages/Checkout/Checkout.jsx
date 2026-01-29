import React, { useState } from 'react';
import { useCart } from '../../contexts/CartContext';
import { useAuth } from '../../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import { FiCreditCard, FiMapPin, FiCheckCircle, FiArrowLeft } from 'react-icons/fi';
import { toast } from 'react-toastify';
import api from '../../services/api'; // <--- O IMPORT QUE FALTAVA
import './Checkout.css';

const Checkout = () => {
    const { cart, clearCart } = useCart();
    const { user } = useAuth();
    const navigate = useNavigate();

    const [step, setStep] = useState(1); // 1: Endereço, 2: Pagamento, 3: Sucesso
    const [loading, setLoading] = useState(false);

    // Garante que cartItems seja um array mesmo se cart for null
    const cartItems = cart?.items || [];

    // Calcula total
    const totalValue = cart?.total || cartItems.reduce((acc, item) => acc + item.unitPrice * item.quantity, 0);

    const formatPrice = (price) => {
        return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(price);
    };

    const [address, setAddress] = useState({
        zip: '', street: '', number: '', city: '', state: ''
    });

    const [card, setCard] = useState({
        number: '', name: '', expiry: '', cvc: ''
    });

    // Processamento de pagamento REAL (Salva no banco)
    const handlePayment = async (e) => {
        e.preventDefault();
        setLoading(true);

        try {
            // 1. Formata o endereço como STRING única (exigência do Backend)
            const fullAddress = `${address.street}, ${address.number} - ${address.city}/${address.state} - CEP: ${address.zip}`;

            // 2. Monta o Payload exato do 'CreateOrderRequest.cs'
            const orderPayload = {
                userId: user.id,          // Validação exige: RuleFor(x => x.UserId).NotEmpty()
                cartId: cart.id,          // Backend usa isso para pegar os itens do banco
                shippingAddress: fullAddress, // Tem que ser string, min 10 chars
                billingAddress: fullAddress   // Validação exige: RuleFor(x => x.BillingAddress).NotEmpty()
            };

            console.log("Enviando Payload Correto:", orderPayload);

            // 3. Envia
            await api.post('/orders', orderPayload);

            toast.success("Pedido realizado com sucesso!");
            clearCart();
            setStep(3);

        } catch (error) {
            console.error("Erro API:", error);

            // Tratamento de erro detalhado para validações do FluentValidation
            if (error.response?.data?.errors) {
                const validationErrors = Object.values(error.response.data.errors).flat();
                validationErrors.forEach(msg => toast.error(msg));
            } else {
                const serverMessage = error.response?.data?.message || "Erro ao processar pedido.";
                toast.error(serverMessage);
            }
        } finally {
            setLoading(false);
        }
    };

    // Se não tiver itens e não estiver na tela de sucesso, avisa
    if (cartItems.length === 0 && step !== 3) {
        return (
            <div className="checkout-page">
                <div className="container empty-checkout">
                    <h2>Seu carrinho está vazio.</h2>
                    <button className="btn-back" onClick={() => navigate('/products')}>
                        Voltar para Produtos
                    </button>
                </div>
            </div>
        );
    }

    return (
        <main className="checkout-page">
            <div className="container">

                {/* Indicador de Passos */}
                <div className="steps-indicator">
                    <div className={`step ${step >= 1 ? 'active' : ''}`}>1. Endereço</div>
                    <div className="line"></div>
                    <div className={`step ${step >= 2 ? 'active' : ''}`}>2. Pagamento</div>
                    <div className="line"></div>
                    <div className={`step ${step >= 3 ? 'active' : ''}`}>3. Conclusão</div>
                </div>

                <div className="checkout-container">

                    {/* ETAPA 1: ENDEREÇO */}
                    {step === 1 && (
                        <div className="checkout-card animate-fade">
                            <div className="card-header">
                                <h2><FiMapPin /> Endereço de Entrega</h2>
                            </div>

                            <form onSubmit={(e) => { e.preventDefault(); setStep(2); }}>
                                <div className="form-group">
                                    <label>CEP</label>
                                    <input required type="text" placeholder="00000-000" className="form-control"
                                        value={address.zip} onChange={e => setAddress({ ...address, zip: e.target.value })} />
                                </div>

                                <div className="form-row">
                                    <div className="form-group" style={{ flex: 2 }}>
                                        <label>Rua / Avenida</label>
                                        <input required type="text" placeholder="Ex: Av. Paulista" className="form-control"
                                            value={address.street} onChange={e => setAddress({ ...address, street: e.target.value })} />
                                    </div>
                                    <div className="form-group" style={{ flex: 1 }}>
                                        <label>Número</label>
                                        <input required type="text" placeholder="123" className="form-control"
                                            value={address.number} onChange={e => setAddress({ ...address, number: e.target.value })} />
                                    </div>
                                </div>

                                <div className="form-row">
                                    <div className="form-group">
                                        <label>Cidade</label>
                                        <input required type="text" placeholder="Cidade" className="form-control"
                                            value={address.city} onChange={e => setAddress({ ...address, city: e.target.value })} />
                                    </div>
                                    <div className="form-group">
                                        <label>Estado</label>
                                        <input required type="text" placeholder="UF" className="form-control"
                                            value={address.state} onChange={e => setAddress({ ...address, state: e.target.value })} />
                                    </div>
                                </div>

                                <div className="checkout-footer">
                                    <button type="button" className="btn-secondary" onClick={() => navigate('/cart')}>Voltar</button>
                                    <button type="submit" className="btn-primary">Ir para Pagamento</button>
                                </div>
                            </form>
                        </div>
                    )}

                    {/* ETAPA 2: PAGAMENTO */}
                    {step === 2 && (
                        <div className="checkout-card animate-fade">
                            <div className="card-header">
                                <h2><FiCreditCard /> Dados do Pagamento</h2>
                                <span className="total-badge">{formatPrice(totalValue)}</span>
                            </div>

                            <form onSubmit={handlePayment}>
                                <div className="form-group">
                                    <label>Número do Cartão</label>
                                    <input required type="text" placeholder="0000 0000 0000 0000" className="form-control" maxLength="19"
                                        value={card.number} onChange={e => setCard({ ...card, number: e.target.value })} />
                                </div>
                                <div className="form-group">
                                    <label>Nome no Cartão</label>
                                    <input required type="text" placeholder="COMO NO CARTAO" className="form-control"
                                        value={card.name} onChange={e => setCard({ ...card, name: e.target.value })} />
                                </div>
                                <div className="form-row">
                                    <div className="form-group">
                                        <label>Validade</label>
                                        <input required type="text" placeholder="MM/AA" className="form-control" maxLength="5"
                                            value={card.expiry} onChange={e => setCard({ ...card, expiry: e.target.value })} />
                                    </div>
                                    <div className="form-group">
                                        <label>CVV</label>
                                        <input required type="text" placeholder="123" className="form-control" maxLength="3"
                                            value={card.cvc} onChange={e => setCard({ ...card, cvc: e.target.value })} />
                                    </div>
                                </div>

                                <div className="payment-warning">
                                    <small>ℹ️ Ambiente de Teste: Nenhuma cobrança real será feita.</small>
                                </div>

                                <div className="checkout-footer">
                                    <button type="button" className="btn-secondary" onClick={() => setStep(1)}>Voltar</button>
                                    <button type="submit" className="btn-primary" disabled={loading}>
                                        {loading ? 'Processando...' : `Pagar ${formatPrice(totalValue)}`}
                                    </button>
                                </div>
                            </form>
                        </div>
                    )}

                    {/* ETAPA 3: SUCESSO */}
                    {step === 3 && (
                        <div className="checkout-success animate-fade">
                            <div className="success-content">
                                <FiCheckCircle className="success-icon" />
                                <h1>Pedido Confirmado!</h1>
                                <p>Obrigado, {user?.name || 'Cliente'}.</p>
                                <p>Seu pedido foi processado e será enviado para:</p>
                                <div className="address-confirm">
                                    <strong>{address.street}, {address.number}</strong><br />
                                    {address.city} - {address.state}<br />
                                    {address.zip}
                                </div>
                                <button className="btn-primary" onClick={() => navigate('/')}>Voltar para Loja</button>
                            </div>
                        </div>
                    )}

                </div>
            </div>
        </main>
    );
};

export default Checkout;