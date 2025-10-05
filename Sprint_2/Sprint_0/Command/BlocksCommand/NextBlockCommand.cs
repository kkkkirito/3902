using Sprint_0.Interfaces;

namespace Sprint_0.Command.BlocksCommand
{
    public class NextBlockCommand : ICommand
    {
        private readonly Blocks.BlockSelector blockSelector;

        public NextBlockCommand(Blocks.BlockSelector blockSelector)
        {
            this.blockSelector = blockSelector;
        }

        public void Execute()
        {
            blockSelector?.Next();
        }
    }
}
