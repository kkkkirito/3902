//Dillon Brigode AU25
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.Commands.PlayerCommands
{
    public class JumpCommand : ICommand
    {
        private readonly IPlayer _player;
        private readonly IAudioManager _audio;
        public JumpCommand(IPlayer player, IAudioManager audio)
        {
            _player = player;
            _audio = audio;
        }
        public void Execute()
        {
            if (_player.IsGrounded)
            {
                _player.Jump();
                _audio.PlayJump();
            }
        }
    }
}