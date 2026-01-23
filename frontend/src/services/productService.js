import api from './api';

export const productService = {
  async getAll(params = {}) {
    const response = await api.get('/products', { params });
    return response.data;
  },

  async getById(id) {
    const response = await api.get(`/products/${id}`);
    return response.data;
  },

  async getByCategory(categoryId) {
    const response = await api.get(`/products/category/${categoryId}`);
    return response.data;
  },

  async search(query) {
    const response = await api.get('/products/search', { params: { q: query } });
    return response.data;
  },

  async getFeatured() {
    const response = await api.get('/products/featured');
    return response.data;
  }
};

export default productService;
