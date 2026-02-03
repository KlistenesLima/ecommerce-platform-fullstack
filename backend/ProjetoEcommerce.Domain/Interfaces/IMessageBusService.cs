using System.Threading.Tasks;

namespace ProjetoEcommerce.Domain.Interfaces
{
    public interface IMessageBusService
    {
        void Publish<T>(T message, string queueName);
    }
}
