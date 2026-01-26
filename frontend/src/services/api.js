import axios from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:7146/api', // Confirme se a porta é 7146 mesmo
});

// Interceptador: Antes de cada requisição, coloca o token no cabeçalho
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('@LuxeStore:token');

    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
}, (error) => {
    return Promise.reject(error);
});

// Interceptador de Resposta (Opcional, mas útil): Se der 401, desloga o usuário
api.interceptors.response.use((response) => {
    return response;
}, (error) => {
    if (error.response && error.response.status === 401) {
        // Token expirou ou é inválido
        // localStorage.removeItem('@LuxeStore:token');
        // localStorage.removeItem('@LuxeStore:user');
        // window.location.href = '/login'; 
    }
    return Promise.reject(error);
});

export default api;