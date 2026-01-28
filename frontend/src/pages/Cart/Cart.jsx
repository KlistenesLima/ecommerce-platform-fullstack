import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { FiTrash2, FiMinus, FiPlus, FiShoppingBag, FiArrowRight } from 'react-icons/fi';
import { useCart } from '../../contexts/CartContext';
import { useAuth } from '../../contexts/AuthContext';
import './Cart.css';

const Cart = () => {
    // Nota: Certifique-se de que seu CartContext retorna 'cart' (com .items dentro) 
    // ou ajuste aqui para const { cartItems } = useCart() se for direto.
    // Mantive a estrutura do seu código original (cart.items).
    const { cart, updateQuantity, removeFromCart, clearCart, itemCount } = useCart();

    // Usamos 'signed' porque foi assim que definimos no AuthContext
    const { signed } = useAuth();
    const navigate = useNavigate();

    const formatPrice = (price) => {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        }).format(price);
    };

    // Função inteligente de Checkout
    const handleCheckout = () => {
        if (signed) {
            // Se logado, vai pagar
            navigate('/checkout');
        } else {
            // Se não, vai logar (e o Login pode redirecionar de volta se você configurar)
            navigate('/login');
        }
    };

    // Verificação de segurança para evitar erro se cart for null
    if (!cart?.items || cart.items.length === 0) {
        return (
            <main className="cart-page">
                <div className="container">
                    <div className="empty-cart">
                        <FiShoppingBag className="empty-icon" />
                        <h2>Seu carrinho está vazio</h2>
                        <p>Explore nossos produtos e adicione itens ao carrinho</p>
                        <Link to="/products" className="btn btn-primary">
                            Ver Produtos
                        </Link>
                    </div>
                </div>
            </main>
        );
    }

    // Calcula o total (caso o contexto não forneça cartTotal pronto)
    const calculateTotal = () => {
        return cart.total || cart.items.reduce((acc, item) => acc + item.unitPrice * item.quantity, 0);
    };

    return (
        <main className="cart-page">
            <div className="container">
                <div className="cart-header">
                    <h1>Carrinho de Compras</h1>
                    <button className="clear-cart" onClick={clearCart}>
                        <FiTrash2 /> Limpar carrinho
                    </button>
                </div>

                <div className="cart-content">
                    <div className="cart-items">
                        {cart.items.map((item) => (
                            <div key={item.productId} className="cart-item">
                                <div className="item-image">
                                    <img src={item.product?.imageUrl || 'https://via.placeholder.com/120'} alt={item.product?.name} />
                                </div>

                                <div className="item-details">
                                    <Link to={`/products/${item.productId}`} className="item-name">
                                        {item.product?.name || 'Produto'}
                                    </Link>
                                    <span className="item-category">{item.product?.category?.name}</span>
                                    <span className="item-unit-price">{formatPrice(item.unitPrice)} / un</span>
                                </div>

                                <div className="item-quantity">
                                    <button onClick={() => updateQuantity(item.productId, item.quantity - 1)}>
                                        <FiMinus />
                                    </button>
                                    <span>{item.quantity}</span>
                                    <button onClick={() => updateQuantity(item.productId, item.quantity + 1)}>
                                        <FiPlus />
                                    </button>
                                </div>

                                <div className="item-total">
                                    {formatPrice(item.unitPrice * item.quantity)}
                                </div>

                                <button
                                    className="item-remove"
                                    onClick={() => removeFromCart(item.productId)}
                                >
                                    <FiTrash2 />
                                </button>
                            </div>
                        ))}
                    </div>

                    <div className="cart-summary">
                        <h3>Resumo do Pedido</h3>

                        <div className="summary-row">
                            <span>Subtotal ({itemCount} itens)</span>
                            <span>{formatPrice(calculateTotal())}</span>
                        </div>

                        <div className="summary-row">
                            <span>Frete</span>
                            <span className="free-shipping">Grátis</span>
                        </div>

                        <div className="summary-divider"></div>

                        <div className="summary-row total">
                            <span>Total</span>
                            <span>{formatPrice(calculateTotal())}</span>
                        </div>

                        {/* === AQUI ESTÁ A LÓGICA INTELIGENTE === */}
                        <button className="btn btn-primary btn-block" onClick={handleCheckout}>
                            {signed ? (
                                <>Finalizar Compra <FiArrowRight /></>
                            ) : (
                                'Fazer Login para Finalizar'
                            )}
                        </button>

                        <Link to="/products" className="continue-shopping">
                            Continuar Comprando
                        </Link>
                    </div>
                </div>
            </div>
        </main>
    );
};

export default Cart;