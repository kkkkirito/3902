// Command/CollisionCommands/PlayerBlockCollisionCommand.cs
using Microsoft.Xna.Framework;
using Sprint_0.Interfaces;
using Sprint_0.Collision_System;

namespace Sprint_0.Command.CollisionCommands
{
    public sealed class PlayerBlockCollisionCommand : ICollisionCommand
    {
        public void Execute(CollisionInfo info)
        {
            // Identify sides without assuming order
            var player = info.A as IPlayer ?? info.B as IPlayer;
            var block  = info.A as IBlock  ?? info.B as IBlock;
            if (player == null || block == null || !block.IsSolid) return;

            // 1) Separate
            player.Position += info.MinimumTranslationVector;

            // 2) Remove velocity into the surface
            var v = player.Velocity;
            switch (info.Direction)
            {
                case CollisionDirection.Left:
                case CollisionDirection.Right:
                    player.Velocity = new Vector2(0f, v.Y);
                    break;
                case CollisionDirection.Top:      // player landed on top
                    player.Velocity = new Vector2(v.X, 0f);
                    player.IsGrounded = true;
                    break;
                case CollisionDirection.Bottom:   // hit ceiling
                    player.Velocity = new Vector2(v.X, 0f);
                    break;
            }
        }
    }
}
