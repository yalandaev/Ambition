namespace Ambition.Bus
{
    public interface ICommandHandler<T>
    {
        void Handle(T command);
    }
}