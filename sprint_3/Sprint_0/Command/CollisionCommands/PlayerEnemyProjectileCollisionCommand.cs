using Microsoft.Xna.Framework;
using Sprint_0.Interfaces;
using Sprint_0.Collision_System;

namespace Sprint_0.Command.CollisionCommands
{
    public sealed class PlayerEnemyProjectileCollisionCommand : ICollisionCommand
    {
        private const float KnockbackDistance = 20f;

        public void Execute(CollisionInfo info)
        {
            var player = info.A as IPlayer ?? info.B as IPlayer;
            var projectile = info.A as IEnemyProjectile ?? info.B as IEnemyProjectile;
            if (player == null || projectile == null) return;

            player.Position += info.MinimumTranslationVector;

            if (!player.IsInvulnerable)
            {
                Vector2 n = info.MinimumTranslationVector;
                if (n != Vector2.Zero) n.Normalize();

                player.Position += n * KnockbackDistance;
                player.Velocity = Vector2.Zero;

                player.TakeDamage(projectile.Damage);

                projectile.IsActive = false;
            }
        }
    }
}
