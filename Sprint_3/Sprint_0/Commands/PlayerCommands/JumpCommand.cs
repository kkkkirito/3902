//Dillon Brigode AU25
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.PlayerCommands
{
    public class JumpCommand : ICommand
    {
        private readonly IPlayer _player;
        public JumpCommand(IPlayer player) => _player = player;
        public void Execute() => _player.Jump();
    }
}