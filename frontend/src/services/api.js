import axios from 'axios';

// Cria a instância do Axios
const api = axios.create({
    // IMPORTANTE: Verifique se a porta 7146 é a correta do seu backend (estava no seu log de erro)
    baseURL: 'https://localhost:7146/api'
});

// === INTERCEPTOR DE REQUISIÇÃO (O Segredo) ===
// Antes de qualquer requisição sair, ele roda isso:
api.interceptors.request.use(async (config) => {
    // 1. Tenta pegar o token salvo no navegador
    const token = localStorage.getItem('@LuxeStore:token');

    // 2. Se tiver token, injeta no cabeçalho Authorization
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
}, (error) => {
    return Promise.reject(error);
});

// === INTERCEPTOR DE RESPOSTA (Opcional, mas recomendado) ===
// Se o backend devolver 401 (Token Expirado), podemos deslogar o usuário automaticamente
api.interceptors.response.use(response => {
    return response;
}, error => {
    if (error.response && error.response.status === 401) {
        // Token expirou ou é inválido
        // Opção: Forçar logout e limpar storage
        // localStorage.removeItem('@LuxeStore:user');
        // localStorage.removeItem('@LuxeStore:token');
        // window.location.href = '/login';
        console.error("Sessão expirada ou não autorizada.");
    }
    return Promise.reject(error);
});

export default api;