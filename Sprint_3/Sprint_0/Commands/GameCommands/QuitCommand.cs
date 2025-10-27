using Sprint_0.Interfaces;

namespace Sprint_0.Commands.GameCommands
{
    public class QuitCommand : ICommand
    {
        private readonly Game1 game;

        public QuitCommand(Game1 game)
        {
            this.game = game;
        }

        public void Execute()
        {
            game.Exit();
        }
    }
}
