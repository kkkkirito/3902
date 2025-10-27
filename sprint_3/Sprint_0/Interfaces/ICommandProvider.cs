namespace Sprint_0.Interfaces
{
    public interface ICommandProvider
    {
        ICollisionCommand Resolve(ICollidable a, ICollidable b);
    }
}