import api from './api';

// MUDANÇA AQUI: "export const" direto, sem default no final
export const cartService = {
    // Pega o carrinho
    getCart: async () => {
        const response = await api.get('/cart');
        return response.data;
    },

    // Adiciona item
    addItem: async (productId, quantity) => {
        const response = await api.post('/cart', { productId, quantity });
        return response.data;
    },

    // Atualiza quantidade
    updateQuantity: async (productId, quantity) => {
        const response = await api.put('/cart/item', { productId, quantity });
        return response.data;
    },

    // Remove item
    removeItem: async (productId) => {
        const response = await api.delete(`/cart/${productId}`);
        return response.data;
    },

    // Limpa tudo
    clear: async () => {
        await api.delete('/cart');
    }
};