using Sprint_0.Interfaces;

namespace Sprint_0.Commands.BlockCommands
{
    public class PreviousBlockCommand : ICommand
    {
        private readonly Blocks.BlockSelector blockSelector;

        public PreviousBlockCommand(Blocks.BlockSelector blockSelector)
        {
            this.blockSelector = blockSelector;
        }

        public void Execute()
        {
            blockSelector.Prev();
        }
    }
}
