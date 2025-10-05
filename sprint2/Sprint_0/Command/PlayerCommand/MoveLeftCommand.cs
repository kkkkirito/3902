using Sprint_0.Interfaces;
using Microsoft.Xna.Framework;  

namespace Sprint_0.Command.PlayerCommand
{
    public class MoveLeftCommand : ICommand
    {
        private readonly IPlayer player;

        public MoveLeftCommand(IPlayer player)
        {
            this.player = player;
        }

        public void Execute()
        {
            if (player != null)
            {
                player?.Move(new Vector2(-1f, 0f));
                
            }
        }
    }
}
