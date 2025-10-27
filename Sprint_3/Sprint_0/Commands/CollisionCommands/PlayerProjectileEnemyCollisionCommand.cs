using Microsoft.Xna.Framework;
using Sprint_0.Collision_System;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class PlayerProjectileEnemyCollisionCommand : ICollisionCommand
    {
        // Tune knockback speed and damage as desired
        private const float KnockbackSpeed = 140f;
        private const int DefaultDamage = 1;

        public void Execute(CollisionInfo info)
        {
            var projectile = info.A as PlayerProjectile ?? info.B as PlayerProjectile;
            var enemy = info.A as Enemy ?? info.B as Enemy;
            if (projectile == null || enemy == null) return;

            // Push enemy out of intersection minimally
            enemy.Position += info.MinimumTranslationVector;

            // Apply directional knockback based on MTV
            Vector2 n = info.MinimumTranslationVector;
            if (n != Vector2.Zero)
            {
                n.Normalize();
                enemy.Velocity = n * KnockbackSpeed;
            }

            // Damage
            int damage = DefaultDamage;
            enemy.TakeDamage(damage);

            // Deactivate the projectile after hit
            projectile.IsActive = false;
        }
    }
}
