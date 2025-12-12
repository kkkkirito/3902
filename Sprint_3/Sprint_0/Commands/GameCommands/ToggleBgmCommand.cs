using Sprint_0.Managers;
using Sprint_0.Interfaces;

namespace Sprint_0.Commands.GameCommands
{
    public class ToggleBgmCommand : ICommand
    {
        private readonly IAudioManager _audio;
        public ToggleBgmCommand(IAudioManager audio) {
            _audio = audio;
        }
        public void Execute()
        {
            _audio.ToggleBgmMute();
        }
    }
}