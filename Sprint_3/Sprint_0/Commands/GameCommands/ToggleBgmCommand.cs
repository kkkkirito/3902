using Sprint_0.Managers;
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.GameCommands
{
    public class ToggleBgmCommand : ICommand
    {
        public void Execute()
        {
            AudioManager.ToggleBgmMute();
        }
    }
}