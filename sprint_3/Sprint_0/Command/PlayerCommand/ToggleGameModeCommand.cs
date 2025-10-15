//Dillon Brigode AU25

using Sprint_0.Interfaces;

namespace Sprint_0.Command.PlayerCommand
{
    public class ToggleGameModeCommand : ICommand
    {
        private IPlayer player;

        public ToggleGameModeCommand(IPlayer player)
        {
            this.player = player;
        }

        public void Execute()
        {
            player.GameMode = player.GameMode == GameModeType.Platformer
                ? GameModeType.TopDown
                : GameModeType.Platformer;
        }
    }
}