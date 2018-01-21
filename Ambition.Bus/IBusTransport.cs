namespace Ambition.Bus
{
    public interface IBusTransport
    {
        void SendCommand(ICommand command);
        void PublishEvent(IEvent @event);
        void Subscribe<TType, THandler>();
    }
}