import { createContext, useContext, useState, useEffect } from 'react';
import { cartService } from '../services/cartService'; // Importação correta (Named Export)
import { useAuth } from './AuthContext';
import { toast } from 'react-toastify';

const CartContext = createContext(null);

export const useCart = () => {
    const context = useContext(CartContext);
    if (!context) {
        throw new Error('useCart must be used within CartProvider');
    }
    return context;
};

export const CartProvider = ({ children }) => {
    const [cart, setCart] = useState({ items: [], total: 0 });
    const [loading, setLoading] = useState(false);

    // --- CORREÇÃO CRÍTICA ---
    // Mapeamos 'signed' (do AuthContext) para 'isAuthenticated' (usado aqui)
    const { signed: isAuthenticated } = useAuth();
    // ------------------------

    // Debug: Monitora se o usuário logou/deslogou
    useEffect(() => {
        console.log("CartContext: Estado de Autenticação ->", isAuthenticated);

        if (isAuthenticated) {
            loadCart();
        } else {
            const localCart = localStorage.getItem('cart');
            if (localCart) {
                setCart(JSON.parse(localCart));
            }
        }
    }, [isAuthenticated]);

    // Salvar no localStorage APENAS se estiver offline/deslogado
    useEffect(() => {
        if (!isAuthenticated) {
            localStorage.setItem('cart', JSON.stringify(cart));
        }
    }, [cart, isAuthenticated]);

    const loadCart = async () => {
        try {
            setLoading(true);
            console.log("CartContext: Buscando carrinho no Backend...");
            const data = await cartService.getCart();
            console.log("CartContext: Carrinho recebido:", data);
            setCart(data);
        } catch (error) {
            console.error('Erro ao carregar carrinho:', error);
        } finally {
            setLoading(false);
        }
    };

    const addToCart = async (product, quantity = 1) => {
        console.log(`CartContext: Adicionando ${product.name}. Autenticado? ${isAuthenticated}`);

        try {
            if (isAuthenticated) {
                // --- CONEXÃO COM O BACKEND ---
                console.log("CartContext: Enviando para API...");
                const data = await cartService.addItem(product.id, quantity);
                console.log("CartContext: Sucesso! Banco atualizado.", data);
                setCart(data);
                // -----------------------------
            } else {
                console.log("CartContext: Offline. Salvando localmente.");
                setCart((prev) => {
                    const currentItems = prev.items || [];
                    const existingItem = currentItems.find((item) => item.productId === product.id);

                    if (existingItem) {
                        return {
                            ...prev,
                            items: currentItems.map((item) =>
                                item.productId === product.id
                                    ? { ...item, quantity: item.quantity + quantity }
                                    : item
                            ),
                            total: prev.total + product.price * quantity
                        };
                    }

                    return {
                        ...prev,
                        items: [
                            ...currentItems,
                            {
                                productId: product.id,
                                product,
                                name: product.name,
                                imageUrl: product.imageUrl,
                                quantity,
                                unitPrice: product.price
                            }
                        ],
                        total: prev.total + product.price * quantity
                    };
                });
            }
            toast.success('Produto adicionado ao carrinho!');
        } catch (error) {
            toast.error('Erro ao adicionar produto');
            console.error("ERRO NO ADDTOCART:", error);
        }
    };

    const updateQuantity = async (productId, quantity) => {
        try {
            if (quantity < 1) {
                return removeFromCart(productId);
            }

            if (isAuthenticated) {
                const data = await cartService.updateQuantity(productId, quantity);
                setCart(data);
            } else {
                setCart((prev) => {
                    const item = prev.items.find((i) => i.productId === productId);
                    if (!item) return prev;
                    const diff = quantity - item.quantity;

                    return {
                        ...prev,
                        items: prev.items.map((i) =>
                            i.productId === productId ? { ...i, quantity } : i
                        ),
                        total: prev.total + item.unitPrice * diff
                    };
                });
            }
        } catch (error) {
            toast.error('Erro ao atualizar quantidade');
            console.error(error);
        }
    };

    const removeFromCart = async (productId) => {
        try {
            if (isAuthenticated) {
                const data = await cartService.removeItem(productId);
                setCart(data);
            } else {
                setCart((prev) => {
                    const item = prev.items.find((i) => i.productId === productId);
                    if (!item) return prev;

                    return {
                        ...prev,
                        items: prev.items.filter((i) => i.productId !== productId),
                        total: prev.total - item.unitPrice * item.quantity
                    };
                });
            }
            toast.info('Produto removido');
        } catch (error) {
            toast.error('Erro ao remover produto');
            console.error(error);
        }
    };

    const clearCart = async () => {
        try {
            if (isAuthenticated) {
                await cartService.clear();
            }
            setCart({ items: [], total: 0 });
        } catch (error) {
            console.error(error);
        }
    };

    const itemCount = (cart.items || []).reduce((acc, item) => acc + item.quantity, 0);

    const value = {
        cart,
        loading,
        addToCart,
        updateQuantity,
        removeFromCart,
        clearCart,
        itemCount
    };

    return <CartContext.Provider value={value}>{children}</CartContext.Provider>;
};

export default CartContext;