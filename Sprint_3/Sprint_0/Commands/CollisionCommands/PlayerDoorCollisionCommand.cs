using Microsoft.Xna.Framework;
using Sprint_0.Blocks;
using Sprint_0.Collision_System;
using Sprint_0.Interfaces;
using System;
using System.Diagnostics;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class PlayerLockedDoorCollisionCommand : ICollisionCommand
    {
        public void Execute(CollisionInfo info)
        {
            var player = info.A as IPlayer ?? info.B as IPlayer;
            var door = info.A as LockedDoor ?? info.B as LockedDoor;

            if (player == null || door == null) return;

            if (door.IsLocked)
            {
                // Check if player has a key
                if (player.KeyCount > 0)
                {
                    // Has a key - unlock the door and consume the key
                    door.Unlock();
                    player.KeyCount--;
                    var pos = (player is Sprint_0.Player_Namespace.Player p) ? p.Position : Vector2.Zero;
                    Debug.WriteLine($"[Game] Door opened at {pos} by player (keys remaining: {player.KeyCount})");
                }
                else
                {
                    // No key - door stays solid, block player
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
            // If door is unlocking or unlocked, it's non-solid so player passes through
        }

        private static Vector2 SeparationFor(ICollidable who, CollisionInfo info)
        {
            return ReferenceEquals(who, info.A) ? info.MinimumTranslationVector
                                                : -info.MinimumTranslationVector;
        }
    }
}