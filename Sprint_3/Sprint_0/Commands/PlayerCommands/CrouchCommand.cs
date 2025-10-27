//Dillon Brigode AU25
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.PlayerCommands
{
    public sealed class CrouchOnCommand : ICommand
    {
        private readonly IPlayer _player;
        public CrouchOnCommand(IPlayer p) => _player = p;
        public void Execute() => _player.SetCrouch(true);
    }
    public sealed class CrouchOffCommand : ICommand
    {
        private readonly IPlayer _player;
        public CrouchOffCommand(IPlayer p) => _player = p;
        public void Execute() => _player.SetCrouch(false);
    }
}