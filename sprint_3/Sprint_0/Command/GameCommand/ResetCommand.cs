using Sprint_0.Interfaces;

namespace Sprint_0.Command.GameCommand
{
    public class ResetCommand : ICommand
    {
        private readonly IGameState gameState;

        public ResetCommand(IGameState gameState)
        {
            this.gameState = gameState;
        }

        public void Execute()
        {
            gameState?.Reset();
        }
    }
}
