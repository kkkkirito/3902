// Command/PlayerCommand/FireballCommand.cs
using System;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

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
            if (_player == null || _proj == null)
                return;
            if (!CooldownManager.CanExecute("Fireball", 0.5))
                return;
            AudioManager.PlaySound(AudioManager.FireballSound, 0.7f);
            _player.Attack(_player.FacingDirection, AttackMode.Fireball);
            _proj.TrySpawnFireball(_player); // capped at 2
        }
    }
}
