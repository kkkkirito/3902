namespace Sprint_0.Interfaces
{
    public interface ICommandProvider
    {
        ICommand Resolve(ICollidable a, ICollidable b);
    }
}