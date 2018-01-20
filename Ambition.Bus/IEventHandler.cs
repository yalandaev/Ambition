namespace Ambition.Bus
{
    public interface IEventHandler<T> where T: IEvent
    {
        void Handle(T command);
    }
}