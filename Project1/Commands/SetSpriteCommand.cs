using Project1.Interfaces;
using System;

namespace Project1.Commands
{
    public enum SpriteChoice
    {
        StaticStanding = 1,     // one frame, no motion
        AnimatedIdle = 2,       // animated, no motion
        FloatingUpDown = 3,     // single frame, vertical motion
        RunningLeftRight = 4    // animated, horizontal motion
    }

    public sealed class SetSpriteCommand : ICommand
    {
        private readonly Action<SpriteChoice> _setter;
        private readonly SpriteChoice _choice;

        public SetSpriteCommand(Action<SpriteChoice> setter, SpriteChoice choice)
        {
            _setter = setter;
            _choice = choice;
        }

        public void Execute() => _setter?.Invoke(_choice);
    }
}

