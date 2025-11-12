using System;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.Commands.PlayerCommands
{
    public sealed class SwordBeamCommand : ICommand
    {
        private readonly IPlayer _player;
        private readonly IProjectileManager _proj;

        public SwordBeamCommand(IPlayer player, IProjectileManager proj)
        {
            _player = player;
            _proj = proj;
        }

        public void Execute()
        {
            if (_player == null || _proj == null)
                return;
            if (!CooldownManager.CanExecute("Beam", 0.5))
                return;
            AudioManager.PlaySound(AudioManager.BeamSound, 0.7f);
            // play the same stab animation regardless; projectile spawns only if allowed
            _player.Attack(_player.FacingDirection, AttackMode.Swordbeam);
            _proj.TrySpawnSwordBeam(_player); 
        }
    }
}
