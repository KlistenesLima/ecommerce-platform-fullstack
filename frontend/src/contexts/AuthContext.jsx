import { createContext, useState, useEffect, useContext } from 'react';
import api from '../services/api';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const recoveredUser = localStorage.getItem('@LuxeStore:user');
        const token = localStorage.getItem('@LuxeStore:token');

        if (recoveredUser && token) {
            setUser(JSON.parse(recoveredUser));
            api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
        }

        setLoading(false);
    }, []);

    const signIn = async ({ email, password }) => {
        try {
            console.log("Tentando logar com:", email);
            const response = await api.post('/auth/login', { email, password });

            console.log("RESPOSTA DO BACKEND:", response.data); // <--- O SEGREDO ESTÁ AQUI

            // Tenta pegar os dados. Se o backend mandar com letra maiúscula ou minúscula, garantimos aqui:
            const token = response.data.token || response.data.Token;
            const role = response.data.role || response.data.Role;
            const userId = response.data.userId || response.data.UserId || response.data.id;
            const firstName = response.data.firstName || response.data.FirstName;

            if (!token) {
                console.error("Token não veio!");
                throw new Error("Token ausente na resposta");
            }

            const userData = {
                id: userId,
                name: firstName,
                role: role,
                email
            };

            console.log("Objeto User montado:", userData);

            localStorage.setItem('@LuxeStore:user', JSON.stringify(userData));
            localStorage.setItem('@LuxeStore:token', token);

            api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
            setUser(userData);

            return userData;
        } catch (error) {
            console.error("Erro no signIn:", error);
            throw error; // Joga o erro pro Login.jsx tratar
        }
    };

    const signOut = () => {
        localStorage.removeItem('@LuxeStore:user');
        localStorage.removeItem('@LuxeStore:token');
        api.defaults.headers.common['Authorization'] = undefined;
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{
            signed: !!user,
            user,
            loading,
            signIn,
            signOut
        }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    return context;
};