using Sprint_0.Interfaces;

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
                //player.Move(new Microsoft.Xna.Framework.Vector2(0, -1));
            }
        }
    }
}