using Sprint_0.Interfaces;
using Sprint_0;

namespace Sprint_0.Commands.GameCommands
{
    public class VictoryCommand : ICommand
    {
        private readonly Game1 _game;

        public VictoryCommand(Game1 game)
        {
            _game = game;
        }

        public void Execute()
        {
            _game?.StateManager.ChangeState("victory");
        }
    }
}