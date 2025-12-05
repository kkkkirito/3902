using Microsoft.Xna.Framework;
using Sprint_0.Blocks;
using Sprint_0.Collision_System;
using Sprint_0.Interfaces;
using System.Diagnostics;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class PlayerTopDownDoorCollisionCommand : ICollisionCommand
    {
        public void Execute(CollisionInfo info)
        {
            var player = info.A as IPlayer ?? info.B as IPlayer;
            var door = info.A as TopDownDoor ?? info.B as TopDownDoor;

            if (player == null || door == null) return;

            if (door.IsLocked)
            {
                if (player.TopDownKeyCount > 0)
                {
                    door.Unlock();
                    player.TopDownKeyCount--;
                    player.GameMode = GameModeType.TopDown;
                    Debug.WriteLine($"[PlayerTopDownDoorCollision] Door opened, switched to TopDown mode (keys remaining: {player.TopDownKeyCount})");
                }
                else
                {
                    var delta = SeparationFor(player, info);
                    player.Position += delta;

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
                            break;
                        case CollisionDirection.Bottom:
                            player.Velocity = new Vector2(v.X, 0f);
                            break;
                    }
                }
            }
        }

        private static Vector2 SeparationFor(ICollidable who, CollisionInfo info)
        {
            return ReferenceEquals(who, info.A) ? info.MinimumTranslationVector
                                                : -info.MinimumTranslationVector;
        }
    }
}