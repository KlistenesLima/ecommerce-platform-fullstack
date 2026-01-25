import api from './api';

const categoryService = {
    // Listar todas as categorias
    getAll: async () => {
        const response = await api.get('/categories');
        return response.data;
    },

    // Pegar uma categoria por ID
    getById: async (id) => {
        const response = await api.get(`/categories/${id}`);
        return response.data;
    },

    // Criar nova categoria
    create: async (data) => {
        const response = await api.post('/categories', data);
        return response.data;
    },

    // Atualizar categoria
    update: async (id, data) => {
        const response = await api.put(`/categories/${id}`, data);
        return response.data;
    },

    // Deletar categoria
    delete: async (id) => {
        const response = await api.delete(`/categories/${id}`);
        return response.data;
    }
};

export default categoryService;