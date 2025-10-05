using Sprint_0.Interfaces;
using Sprint_0.States.LinkStates;

namespace Sprint_0.Command.PlayerCommand
{
    public class AttackCommand : ICommand
    {
        private readonly IPlayer player;

        public AttackCommand(IPlayer player)
        {
            this.player = player;
        }

        public void Execute()
        {
            if (player != null)
            {
                
            }
        }
    }
}
