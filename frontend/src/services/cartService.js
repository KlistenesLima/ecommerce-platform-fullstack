import api from './api';

export const cartService = {
    getCart: async () => {
        const response = await api.get('/cart');
        return response.data;
    },

    addItem: async (productId, quantity) => {
        const response = await api.post('/cart', { productId, quantity });
        return response.data;
    },

    updateQuantity: async (productId, quantity) => {
        const response = await api.put('/cart/item', { productId, quantity });
        return response.data;
    },

    removeItem: async (productId) => {
        const response = await api.delete(`/cart/${productId}`); 
        return response.data;
    },

    clear: async () => {
        await api.delete('/cart');
    }
};