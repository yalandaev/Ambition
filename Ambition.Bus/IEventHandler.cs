namespace Ambition.Bus
{
    public interface IEventHandler<T>
    {
        void Handle(T command);
    }
}