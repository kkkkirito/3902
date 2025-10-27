using Microsoft.Xna.Framework.Input;
using Sprint_0.Interfaces;
using System.Collections.Generic;


namespace Sprint_0
{
    public class KeyboardController : IController
    {

        private Game1 game;
        private IPlayer player; // Add player reference
        public int blockSwitch { get; set; } = 0;
        private readonly Dictionary<Keys, ICommand> pressed = new Dictionary<Keys, ICommand>();
        private readonly Dictionary<Keys, ICommand> held = new();

        private readonly Dictionary<Keys, ICommand> released = new();

        private KeyboardState _prev;

        public KeyboardController(Game1 game, IPlayer player = null)
        {

            this.game = game;
            this.player = player; // Store player reference
            _prev = Keyboard.GetState();
        }

        public void Press(Keys key, ICommand command)
        {
            pressed[key] = command;
        }

        public void BindHold(Keys key, ICommand command)
        {
            held[key] = command;
        }
        public void BindRelease(Keys key, ICommand command)
        {
            released[key] = command;
        }

        public void Update()
        {

            var currentState = Keyboard.GetState();

            //logic for holding keys down. different than other loop.
            foreach (var kvp in held)
            {
                if (currentState.IsKeyDown(kvp.Key))
                    kvp.Value.Execute();
            }

            //executes on transition from up to down.
            foreach (var k in pressed)
            {
                Keys key = k.Key;
                ICommand command = k.Value;
                if (currentState.IsKeyDown(key) && !_prev.IsKeyDown(key))
                {
                    command.Execute();
                }
            }

            foreach (var (key, cmd) in released)
            {
                if (_prev.IsKeyDown(key) && currentState.IsKeyUp(key))
                    cmd.Execute();
            }
            _prev = currentState;
            // Damage (E) and Attack (B) are handled via command bindings in Game1
        }
    }

}
