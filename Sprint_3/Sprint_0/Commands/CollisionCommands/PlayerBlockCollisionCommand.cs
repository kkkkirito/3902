// Command/CollisionCommands/PlayerBlockCollisionCommand.cs
using Microsoft.Xna.Framework;
using Sprint_0.Collision_System;
using Sprint_0.Enemies;
using Sprint_0.Blocks;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class PlayerBlockCollisionCommand : ICollisionCommand
    {
        
        public void Execute(CollisionInfo info)
        {

            // Identify sides without assuming order
            var player = info.A as IPlayer ?? info.B as IPlayer;
            var block  = info.A as IBlock  ?? info.B as IBlock;
            if (player == null || block == null || !block.IsSolid) return;

            // Skip left/right collision for trap blocks
            if (block is TrapBlock &&
                (info.Direction == CollisionDirection.Left || info.Direction == CollisionDirection.Right))
            {
                return;
            }

            var delta = SeparationFor(player, info);
            player.Position += delta;

            // 2) Remove velocity into the surface
            var v = player.Velocity;
            switch (info.Direction)
            {
                case CollisionDirection.Left:
                case CollisionDirection.Right:
                    if (block is TrapBlock trapBlock)
                    {
                        break;
                    }
                    player.Velocity = new Vector2(0f, v.Y);
                    break;
                case CollisionDirection.Top:      // player landed on top
                    player.Velocity = new Vector2(v.X, 0f);
                    player.IsGrounded = true;

                    if (block is TrapBlock trap)
                    {
                        trap.TriggerBreak();
                    }

                    break;
                case CollisionDirection.Bottom:   // hit ceiling
                    player.Velocity = new Vector2(v.X, 0f);
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
