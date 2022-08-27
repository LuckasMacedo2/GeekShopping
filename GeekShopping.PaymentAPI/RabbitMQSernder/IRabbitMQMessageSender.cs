using GeekShopping.MessageBus;

namespace GeekShopping.PaymentAPI.RabbitMQSernder
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
