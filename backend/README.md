# ProjetoEcommerce - Plataforma Robusta de E-commerce

## 🚀 Sobre

Plataforma e-commerce escalável e moderna com arquitetura em camadas, integração com KAFKA, RabbitMQ, Redis, AWS e suporte a Kubernetes.

## 🏗️ Arquitetura

```
ProjetoEcommerce.Api (WebAPI)
├── ProjetoEcommerce.Application (Lógica)
├── ProjetoEcommerce.Infra.IoC (Injeção de Dependência)
├── ProjetoEcommerce.Infra.MessageQueue (KAFKA, RabbitMQ)
├── ProjetoEcommerce.Infra.Cache (Redis)
├── ProjetoEcommerce.Infra.Cloud (AWS, Azure)
└── ProjetoEcommerce.Infra.Data (PostgreSQL, MongoDB)
```

## 📋 Requisitos

- .NET 8.0+
- Docker & Docker Compose
- PostgreSQL 15+
- RabbitMQ 3.12+
- Redis 7+
- AWS CLI (para integração AWS)

## 🔧 Instalação

1. Clonar repositório
2. Restaurar dependências: `dotnet restore`
3. Aplicar migrações: `dotnet ef database update`
4. Executar: `dotnet run --project ProjetoEcommerce.Api`

## 🗄️ Banco de Dados

### PostgreSQL
- Tabelas relacionais principais
- Migrations automáticas

### MongoDB
- Dados desnormalizados
- Cache de dados complexos

## 📨 Message Queue

- **RabbitMQ**: Processamento de pedidos, notificações
- **KAFKA**: Streaming de eventos, analytics

## 💾 Cache

- **Redis**: Sessões, cache de produtos, recomendações

## ☁️ Cloud

- **AWS S3**: Armazenamento de imagens
- **AWS DynamoDB**: Dados escaláveis
- **AWS SQS**: Filas de mensagens

## 🧪 Testes

```bash
dotnet test
```

## 🐳 Docker

```bash
docker-compose up -d
```

## 📚 Documentação

Acesse Swagger em: `https://localhost:7001/swagger`

## 🤝 Contribuindo

Contribuições são bem-vindas!

## 📄 Licença

MIT
