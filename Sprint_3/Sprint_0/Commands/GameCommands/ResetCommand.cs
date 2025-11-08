using Sprint_0.Interfaces;
using Sprint_0.States.Gameplay;

namespace Sprint_0.Commands.GameCommands
{
    public class ResetCommand(GameplayState gameplay) : ICommand
    {
        private readonly GameplayState _gamestate = gameplay;

        public void Execute()
        {
            _gamestate?.Reset();
        }
    }
}
