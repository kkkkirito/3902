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
        private int MagicCost = 2;
        private readonly IAudioManager _audio;
        public FireballCommand(IPlayer player, IProjectileManager proj, IAudioManager audio)
        {
            _player = player;
            _proj = proj;
            _audio = audio;
        }

        public void Execute()
        {
            if (_player == null || _proj == null)
                return;
            if (!CooldownManager.CanExecute("Fireball", 0.5))
                return;
            if (_player.CurrentMagic < MagicCost)
                return;
            _player.Attack(_player.FacingDirection, AttackMode.Fireball);
            if (_proj.TrySpawnFireball(_player))
            {
                if (_player.TrySpendMagic(MagicCost))
                {
                    _audio.PlayFireball();
                }
            }
        }
    }
}
