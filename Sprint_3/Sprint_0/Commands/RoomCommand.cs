using Sprint_0.Interfaces;
using Sprint_0.States;

namespace Sprint_0.Commands.RoomCommand
{
    public class SwitchRoomCommand : ICommand
    {
        private readonly GameplayState gameState;
        private readonly int roomIndex;

        public SwitchRoomCommand(GameplayState gameState, int roomIndex)
        {
            this.gameState = gameState;
            this.roomIndex = roomIndex;
        }

        public void Execute()
        {
            gameState.SwitchToRoom(roomIndex);
        }
    }
}