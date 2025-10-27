namespace Sprint_0.Interfaces
{
    public interface IEnemyProjectile : ICollidable
    {
        bool IsActive { get; set; }
        int Damage { get; }
    }
}