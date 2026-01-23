# Frontend E-commerce - LUXE Store

Frontend React moderno para o projeto de E-commerce.

## 🚀 Tecnologias

- React 18 + Vite
- React Router DOM
- Axios
- Bootstrap + React Bootstrap
- React Icons
- React Toastify
- React Hook Form + Yup
- JWT Decode

## 📁 Estrutura

```
src/
├── components/       # Componentes reutilizáveis
│   ├── common/       # Loading, Button, etc
│   └── product/      # ProductCard, etc
├── contexts/         # Context API (Auth, Cart)
├── layouts/          # Header, Footer
├── pages/            # Páginas da aplicação
├── services/         # Serviços de API
├── styles/           # CSS global
└── App.jsx           # Componente principal
```

## 🔧 Instalação

1. Copie todos os arquivos da pasta `src` para `frontend/src`

2. Crie o arquivo `.env` na raiz do frontend:
```
VITE_API_URL=https://localhost:7001/api
```

3. Inicie o servidor de desenvolvimento:
```bash
npm run dev
```

4. Acesse: http://localhost:5173

## 📄 Páginas

- **/** - Home (produtos em destaque)
- **/products** - Lista de produtos
- **/products/:id** - Detalhes do produto
- **/cart** - Carrinho de compras
- **/checkout** - Finalizar compra
- **/login** - Login
- **/register** - Cadastro
- **/profile** - Perfil do usuário
- **/orders** - Meus pedidos

## 🎨 Design

Design elegante com tema escuro e dourado:
- Fonte display: Playfair Display
- Fonte body: Poppins
- Cores principais: #c9a962 (dourado), #1a1a2e (azul escuro)

## 🔗 API

Configure a URL da API no arquivo `.env`. Por padrão usa:
`https://localhost:7001/api`

Endpoints esperados:
- POST /auth/login
- POST /auth/register
- GET /products
- GET /products/:id
- GET /cart
- POST /cart/items
- GET /orders
- POST /orders
