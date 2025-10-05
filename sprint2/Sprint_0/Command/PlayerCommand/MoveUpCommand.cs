using Sprint_0.Interfaces;
using Microsoft.Xna.Framework;
namespace Sprint_0.Command.PlayerCommand
{
    public class MoveUpCommand : ICommand
    {
        private readonly IPlayer player;

        public MoveUpCommand(IPlayer player)
        {
            this.player = player;
        }

        public void Execute()
        {
            if (player != null)
            {
                player?.Move(new Vector2(0f, -1f));
            }
        }
    }
}