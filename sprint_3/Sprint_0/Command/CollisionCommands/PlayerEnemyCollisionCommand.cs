using Microsoft.Xna.Framework;
using Sprint_0.Collision_System;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.Command.CollisionCommands
{
    public sealed class PlayerEnemyCollisionCommand : ICollisionCommand
    {
        private const float KnockbackDistance = 20f; 

        public void Execute(CollisionInfo info)
        {
            var player = info.A as IPlayer ?? info.B as IPlayer;
            var enemy  = info.A as Enemy    ?? info.B as Enemy;
            if (player == null || enemy == null) return;

            // Always separate overlap first
            player.Position += info.MinimumTranslationVector;

            // Only apply knockback when damage actually applies
            if (!player.IsInvulnerable)
            {
                // Knockback away from impact normal (same dir as MTV)
                Vector2 n = info.MinimumTranslationVector;
                if (n != Vector2.Zero) n.Normalize();

                // Positional shove
                player.Position += n * KnockbackDistance;

                // Optionally zero horizontal velocity; HurtState will handle visuals
                player.Velocity = Vector2.Zero;

                // Apply damage
                player.TakeDamage(10);
            }
        }
    }
}