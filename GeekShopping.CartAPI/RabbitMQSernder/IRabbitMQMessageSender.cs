using GeekShopping.MessageBus;

namespace GeekShopping.CartAPI.RabbitMQSernder
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
