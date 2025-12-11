using Sprint_0.Blocks;
using Sprint_0.Collision_System;
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class PlayerLavaCollisionCommand : ICollisionCommand
    {
        private const int LavaDamage = 100;

        public void Execute(CollisionInfo info)
        {
            var player = info.A as IPlayer ?? info.B as IPlayer;
            var lava = info.A as LavaBlock ?? info.B as LavaBlock;
            if (player == null || lava == null) return;

            if (!player.IsInvulnerable)
            {
                player.TakeDamage(LavaDamage);
            }
        }
    }
}
