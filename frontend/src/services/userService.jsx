import api from './api';

const userService = {
    // Listar todos os usuários
    getAll: async () => {
        const response = await api.get('/users');
        return response.data;
    },

    // Pegar usuário por ID
    getById: async (id) => {
        const response = await api.get(`/users/${id}`);
        return response.data;
    },

    // Atualizar usuário (ex: mudar de Cliente para Admin)
    update: async (id, data) => {
        const response = await api.put(`/users/${id}`, data);
        return response.data;
    },

    // Deletar usuário
    delete: async (id) => {
        const response = await api.delete(`/users/${id}`);
        return response.data;
    }
};

export default userService;