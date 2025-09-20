using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project1.Interfaces;
using System.Collections.Generic;

namespace Project1.Controllers
{
    public sealed class KeyboardInputController : IController
    {
        private readonly Dictionary<Keys, ICommand> _bindings = new();
        private KeyboardState _prev;

        public KeyboardInputController(
            ICommand quit,
            ICommand setStatic,
            ICommand setAnimated,
            ICommand setFloat,
            ICommand setRun)
        {
            // D#
            _bindings[Keys.D0] = quit;
            _bindings[Keys.D1] = setStatic;
            _bindings[Keys.D2] = setAnimated;
            _bindings[Keys.D3] = setFloat;
            _bindings[Keys.D4] = setRun;
            // Numpad#
            _bindings[Keys.NumPad0] = quit;
            _bindings[Keys.NumPad1] = setStatic;
            _bindings[Keys.NumPad2] = setAnimated;
            _bindings[Keys.NumPad3] = setFloat;
            _bindings[Keys.NumPad4] = setRun;
        }

        public void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();
            foreach (var kvp in _bindings)
            {
                if (ks.IsKeyDown(kvp.Key) && _prev.IsKeyUp(kvp.Key))
                {
                    kvp.Value.Execute();
                }
            }
            _prev = ks;
        }
    }
}

