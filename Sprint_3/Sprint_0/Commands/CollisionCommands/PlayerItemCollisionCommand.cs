using Sprint_0.Collision_System;
using Sprint_0.Interfaces;
using Sprint_0.Items;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class PlayerItemCollisionCommand : ICollisionCommand
    {
        public void Execute(CollisionInfo info)
        {
            var player = info.A as IPlayer ?? info.B as IPlayer;
            var item = info.A as IItem ?? info.B as IItem;

            if (player == null || item == null) return;

            if (item is ICollectible collectible)
            {
                if (collectible.IsCollected) return;
                collectible.Collect(player);
                if (collectible.Celebration)
                {
                    player.Pickup(collectible);
                }
                return;
            }

            // Generic consumable path
            if (item is IConsumable consumable)
            {
                if (!consumable.IsCollected)
                {
                    consumable.Consume(player);
                }
            }
        }
    }
}
