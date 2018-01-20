namespace Ambition.Bus
{
    public interface IBusTransport
    {
        void SendCommand(ICommand command);
        void PublishEvent(IEvent @event);
        void Subscribe<TType, THandler>();

        //void SendMessage(string exchangeName, string routingKey, string message);
        //void Subscribe<TType, THandler>(string exchangeName, string queueName, int consumersCount);
    }
}