using Sprint_0.Interfaces;

namespace Sprint_0.Commands
{
    internal class UseSlotCommand : ICommand
    {
        private readonly IPlayer _player;
        //private readonly IWorld _world;
        private readonly int _index;
        private readonly IHotbar _hotbar;

        public UseSlotCommand(IHotbar hotbar, IPlayer player, int index)
        {
            _hotbar = hotbar;
            _player = player;
            //_world = world; 
            _index = index;
        }

        public void Execute()
        {
            _hotbar.UseSlot(_index, _player);
        }
    }
}
