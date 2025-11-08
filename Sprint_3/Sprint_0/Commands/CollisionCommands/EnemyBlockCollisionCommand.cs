using Microsoft.Xna.Framework;
using Sprint_0.Collision_System;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class EnemyBlockCollisionCommand : ICollisionCommand
    {
        public void Execute(CollisionInfo info)
        {
            var enemy = info.A as Enemy ?? info.B as Enemy;
            var block = info.A as IBlock ?? info.B as IBlock;
            if (enemy == null || block == null || !block.IsSolid) return;

            // Separate overlap
            var delta = SeparationFor(enemy, info);
            enemy.Position += delta;

            enemy.LastCollisionDirection = info.Direction;

            // Bubble enemies have different collision behavior
            if (enemy is BubbleEnemy)
            {
                return;
            }

            // Normal enemy physics - clear velocity into surface, mark grounded
            var v = enemy.Velocity;
            switch (info.Direction)
            {
                case CollisionDirection.Left:
                case CollisionDirection.Right:
                    enemy.Velocity = new Vector2(0f, v.Y);
                    break;
                case CollisionDirection.Top:          // enemy landed on block
                    enemy.Velocity = new Vector2(v.X, 0f);
                    enemy.IsGrounded = true;
                    break;
                case CollisionDirection.Bottom:       // hit ceiling
                    enemy.Velocity = new Vector2(v.X, 0f);
                    break;
            }
        }

        private static Vector2 SeparationFor(ICollidable who, CollisionInfo info)
        {
            // MTV pushes A out of B; flip it when we're resolving B.
            return ReferenceEquals(who, info.A) ? info.MinimumTranslationVector
                                                : -info.MinimumTranslationVector;
        }
    }
}