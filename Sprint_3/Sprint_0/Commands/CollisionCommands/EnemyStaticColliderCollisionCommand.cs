using Microsoft.Xna.Framework;
using Sprint_0.Collision_System;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class EnemyStaticColliderCollisionCommand : ICollisionCommand
    {
        public void Execute(CollisionInfo info)
        {
            var enemy = info.A as Enemy ?? info.B as Enemy;
            var surface = info.A as IStaticCollider ?? info.B as IStaticCollider;
            if (enemy == null || surface == null || !surface.IsSolid) return;

            var mtv = ReferenceEquals(enemy, info.A) ? info.MinimumTranslationVector : -info.MinimumTranslationVector;
            enemy.Position += mtv;

            var v = enemy.Velocity;
            switch (info.Direction)
            {
                case CollisionDirection.Left:
                case CollisionDirection.Right:
                    enemy.Velocity = new Vector2(0f, v.Y);
                    break;
                case CollisionDirection.Top:
                    enemy.Velocity = new Vector2(v.X, 0f);
                    enemy.IsGrounded = true;
                    break;
                case CollisionDirection.Bottom:
                    enemy.Velocity = new Vector2(v.X, 0f);
                    break;
            }
        }
    }
}
