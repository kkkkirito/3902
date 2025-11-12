//Dillon Brigode AU25
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.Commands.PlayerCommands
{
    public class JumpCommand : ICommand
    {
        private readonly IPlayer _player;
        public JumpCommand(IPlayer player) => _player = player;
        public void Execute()
        {
            if (_player.IsGrounded)
            {
                _player.Jump();
                AudioManager.PlaySound(AudioManager.JumpSound, 0.7f);
            }
        }
    }
}