namespace Ambition.Bus
{
    public interface IBusTransport
    {
        void SendMessage(string exchangeName, string routingKey, string message);
        void Subscribe<TType, THandler>(string exchangeName, string queueName, int consumersCount);
    }
}