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

                //player.Move(new Microsoft.Xna.Framework.Vector2(1, 0));
                //Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}

