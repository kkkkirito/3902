using Sprint_0.Interfaces;

namespace Sprint_0.Commands.PlayerCommands
{
    public class UseItemCommand : ICommand
    {
        private readonly IPlayer player;
        private readonly int itemSlot;

        public UseItemCommand(IPlayer player, int itemSlot)
        {
            this.player = player;
            this.itemSlot = itemSlot;
        }

        public void Execute()
        {
            // This will be implemented when item system is ready
            // For now, just a placeholder
            System.Diagnostics.Debug.WriteLine($"Using item in slot {itemSlot}");
        }
    }
}
