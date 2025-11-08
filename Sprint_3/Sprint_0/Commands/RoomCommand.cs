using Sprint_0.Interfaces;
using Sprint_0.States.Gameplay;

namespace Sprint_0.Commands
{
    public class SwitchRoomCommand(RoomNavigator navigator, int roomIndex) : ICommand
    {
        private readonly RoomNavigator _navigator = navigator;
        private readonly int _roomIndex = roomIndex;

        public void Execute()
        {
            _navigator.SwitchTo(_roomIndex);

        }
    }
}