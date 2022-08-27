using GeekShopping.MessageBus;

namespace GeekShopping.OrderAPI.RabbitMQSernder
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
