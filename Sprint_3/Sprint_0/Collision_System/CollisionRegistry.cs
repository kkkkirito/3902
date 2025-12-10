using Sprint_0.Blocks;
using Sprint_0.Commands.CollisionCommands;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;

namespace Sprint_0.Collision_System
{
    public static class CollisionRegistry
    {
        public static void Register(CollisionSystem system)
        {
            system.Provider.Register<IPlayer, LockedDoor>(new PlayerLockedDoorCollisionCommand());
            system.Provider.Register<IPlayer, TopDownDoor>(new PlayerTopDownDoorCollisionCommand());
            system.Provider.Register<IPlayer, IBlock>(new PlayerBlockCollisionCommand());
            system.Provider.Register<IPlayer, Enemy>(new PlayerEnemyCollisionCommand());
            system.Provider.Register<IPlayer, IItem>(new PlayerItemCollisionCommand());
            system.Provider.Register<PlayerAttackHitbox, Enemy>(new PlayerAttackEnemyCollisionCommand());
            system.Provider.Register<Enemy, IBlock>(new EnemyBlockCollisionCommand());
            system.Provider.Register<IPlayer, IEnemyProjectile>(new PlayerEnemyProjectileCollisionCommand());
            system.Provider.Register<IPlayer, IStaticCollider>(new PlayerStaticColliderCollisionCommand());
            system.Provider.Register<Enemy, IStaticCollider>(new EnemyStaticColliderCollisionCommand());
            system.Provider.Register<PlayerProjectile, Enemy>(new PlayerProjectileEnemyCollisionCommand());
            
        }
    }
}