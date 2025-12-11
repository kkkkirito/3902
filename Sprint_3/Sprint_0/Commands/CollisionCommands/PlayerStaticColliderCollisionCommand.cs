using Microsoft.Xna.Framework;
using Sprint_0.Collision_System;
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class PlayerStaticColliderCollisionCommand : ICollisionCommand
    {
        public void Execute(CollisionInfo info)
        {
            var player = info.A as IPlayer ?? info.B as IPlayer;
            var surface = info.A as IStaticCollider ?? info.B as IStaticCollider;
            if (player == null || surface == null || !surface.IsSolid) return;

            var mtv = ReferenceEquals(player, info.A) ? info.MinimumTranslationVector : -info.MinimumTranslationVector;
            player.Position += mtv;

            // Kill only the velocity component into the surface
            var v = player.Velocity;
            switch (info.Direction)
            {
                case CollisionDirection.Left:
                case CollisionDirection.Right:
                    player.Velocity = new Vector2(0f, v.Y);
                    break;
                case CollisionDirection.Top:
                    player.Velocity = new Vector2(v.X, 0f);
                    player.IsGrounded = true;
                    if (player is Player_Namespace.Player p)
                    {
                        p.VerticalVelocity = 0f;
                    }
                    break;
                case CollisionDirection.Bottom:
                    player.Velocity = new Vector2(v.X, 0f);
                    break;
            }
        }
    }
}
