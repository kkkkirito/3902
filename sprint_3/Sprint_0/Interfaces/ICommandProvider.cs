using Sprint_0.Collision_System;
namespace Sprint_0.Interfaces
{
    public interface ICommandProvider
    {
        ICollisionCommand Resolve(ICollidable a, ICollidable b);
    }
}