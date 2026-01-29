import { createContext, useContext, useState, useEffect } from 'react';
import { cartService } from '../services/cartService';
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
    const [cart, setCart] = useState({ items: [], totalPrice: 0 });
    const [loading, setLoading] = useState(false);

    const { signed: isAuthenticated } = useAuth();

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
                console.log("CartContext: Enviando para API...");
                const data = await cartService.addItem(product.id, quantity);
                console.log("CartContext: Sucesso! Banco atualizado.", data);
                setCart(data);
            } else {
                console.log("CartContext: Offline. Salvando localmente.");
                setCart((prev) => {
                    const currentItems = prev.items || [];
                    const existingItem = currentItems.find((item) => item.productId === product.id);

                    if (existingItem) {
                        const newItems = currentItems.map((item) =>
                            item.productId === product.id
                                ? {
                                    ...item,
                                    quantity: item.quantity + quantity,
                                    total: (item.quantity + quantity) * item.unitPrice
                                }
                                : item
                        );
                        return {
                            ...prev,
                            items: newItems,
                            totalPrice: newItems.reduce((acc, item) => acc + item.total, 0)
                        };
                    }

                    const newItem = {
                        productId: product.id,
                        product: {
                            id: product.id,
                            name: product.name,
                            imageUrl: product.imageUrl,
                            categoryName: product.categoryName || ''
                        },
                        quantity,
                        unitPrice: product.price,
                        total: product.price * quantity
                    };

                    const newItems = [...currentItems, newItem];
                    return {
                        ...prev,
                        items: newItems,
                        totalPrice: newItems.reduce((acc, item) => acc + item.total, 0)
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
                    const newItems = prev.items.map((item) =>
                        item.productId === productId
                            ? { ...item, quantity, total: quantity * item.unitPrice }
                            : item
                    );
                    return {
                        ...prev,
                        items: newItems,
                        totalPrice: newItems.reduce((acc, item) => acc + item.total, 0)
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
                    const newItems = prev.items.filter((item) => item.productId !== productId);
                    return {
                        ...prev,
                        items: newItems,
                        totalPrice: newItems.reduce((acc, item) => acc + item.total, 0)
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
            setCart({ items: [], totalPrice: 0 });
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