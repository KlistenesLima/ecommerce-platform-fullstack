import api from './api';

export const orderService = {
  async getAll() {
    const response = await api.get('/orders');
    return response.data;
  },

  async getById(id) {
    const response = await api.get(`/orders/${id}`);
    return response.data;
  },

  async create(orderData) {
    const response = await api.post('/orders', orderData);
    return response.data;
  },

  async cancel(id) {
    const response = await api.put(`/orders/${id}/cancel`);
    return response.data;
  }
};

export default orderService;
