using ProjetoEcommerce.Domain.Interfaces;

namespace ProjetoEcommerce.Infra.MessageQueue.RabbitMQ
{
    public interface IRabbitMQPublisher : IMessageBusService
    {
        // Herda o método Publish<T> do IMessageBusService
        // Podemos adicionar métodos específicos do Rabbit aqui no futuro se precisar
    }
}
