// Command/PlayerCommand/FireballCommand.cs
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.PlayerCommands
{
    public sealed class FireballCommand : ICommand
    {
        private readonly IPlayer _player;
        private readonly IProjectileManager _proj;

        public FireballCommand(IPlayer player, IProjectileManager proj)
        {
            _player = player;
            _proj = proj;
        }

        public void Execute()
        {
            _player.Attack(_player.FacingDirection, AttackMode.Fireball);
            _proj.TrySpawnFireball(_player); // capped at 2
        }
    }
}
