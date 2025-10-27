using Sprint_0.Interfaces;

namespace Sprint_0.Commands.PlayerCommands
{
    public class DamagePlayerCommand : ICommand
    {
        private readonly IPlayer player;
        private readonly int damageAmount;

        public DamagePlayerCommand(IPlayer player, int damageAmount = 10)
        {
            this.player = player;
            this.damageAmount = damageAmount;
        }

        public void Execute()
        {
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }
    }
}
