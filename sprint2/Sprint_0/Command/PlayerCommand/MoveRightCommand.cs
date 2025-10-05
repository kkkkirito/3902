using Microsoft.Xna.Framework;
using Sprint_0.Interfaces;

namespace Sprint_0.Command.PlayerCommand
{
    public class MoveRightCommand : ICommand
    {
        private readonly IPlayer player;

        public MoveRightCommand(IPlayer player)
        {
            this.player = player;
        }

        public void Execute()
        {
            if (player != null)
            {
                player?.Move(new Vector2(1f, 0f));
            }
        }
    }
}

