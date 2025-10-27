using Microsoft.Xna.Framework;
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.PlayerCommands
{
    public class MoveDownCommand : ICommand
    {
        private readonly IPlayer player;

        public MoveDownCommand(IPlayer player)
        {
            this.player = player;
        }

        public void Execute()
        {
            if (player != null)
            {
                player.Move(new Vector2(0f, 1f));
            }
        }
    }
}
