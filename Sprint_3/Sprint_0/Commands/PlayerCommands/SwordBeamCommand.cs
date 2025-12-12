using System;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.Commands.PlayerCommands
{
    public sealed class SwordBeamCommand : ICommand
    {
        private readonly IPlayer _player;
        private readonly IProjectileManager _proj;
        private int MagicCost = 1;
        private readonly IAudioManager _audio;

        public SwordBeamCommand(IPlayer player, IProjectileManager proj, IAudioManager audio)
        {
            _player = player;
            _proj = proj;
            _audio = audio;
        }

        public void Execute()
        {
            if (_player == null || _proj == null)
                return;
            if (!CooldownManager.CanExecute("Beam", 0.5))
                return;
            if (_player.CurrentMagic < MagicCost)
                return;
            _player.Attack(_player.FacingDirection, AttackMode.Swordbeam);
            if (_proj.TrySpawnSwordBeam(_player))
            {
                if (_player.TrySpendMagic(MagicCost))
                {
                    _audio.PlayBeam();
                }
            }
        }
    }
}
