using Microsoft.Xna.Framework;
using Sprint_0.Blocks;
using Sprint_0.Collision_System;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class PlayerTopDownDoorCollisionCommand : ICollisionCommand
    {
        public void Execute(CollisionInfo info)
        {
            var player = info.A as IPlayer ?? info.B as IPlayer;
            var door = info.A as TopDownDoor ?? info.B as TopDownDoor;

            if (player == null || door == null) return;

            if (door.IsUnlocked)
            {
                return;
            }

            if (player is Player p && p.HasTopDownKey)
            {
                door.Unlock();
                player.GameMode = GameModeType.TopDown;
            }
            else
            {
                ResolveCollision(player, door, info.Direction);
            }
        }

        private void ResolveCollision(IPlayer player, TopDownDoor door, CollisionDirection direction)
        {
            Rectangle playerBox = player.BoundingBox;
            Rectangle doorBox = door.BoundingBox;

            switch (direction)
            {
                case CollisionDirection.Left:
                    player.Position = new Vector2(doorBox.Left - playerBox.Width, player.Position.Y);
                    break;
                case CollisionDirection.Right:
                    player.Position = new Vector2(doorBox.Right, player.Position.Y);
                    break;
                case CollisionDirection.Top:
                    player.Position = new Vector2(player.Position.X, doorBox.Top - playerBox.Height);
                    break;
                case CollisionDirection.Bottom:
                    player.Position = new Vector2(player.Position.X, doorBox.Bottom);
                    break;
            }
        }
    }
}