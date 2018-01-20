namespace Ambition.Bus
{
    public interface ICommandHandler<T> where T:ICommand
    {
        void Handle(T command);
    }
}