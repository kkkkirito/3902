using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using Sprint_0.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint_0.States.LinkStates;


namespace Sprint_0
{
    public class KeyboardController : IController
    {

        private Game1 game;
        private IPlayer player; // Add player reference
        public int blockSwitch { get; set; } = 0;
        private readonly Dictionary<Keys, ICommand> Map = new Dictionary<Keys, ICommand>();
        private KeyboardState _prev;

        public KeyboardController(Game1 game, IPlayer player = null)
        {

            this.game = game;
            this.player = player; // Store player reference
            _prev = Keyboard.GetState(); 
        }

        public void Press(Keys key, ICommand command)
        {
                Map[key] = command;
        }

        
        public void Update()
        {

            var currentState = Keyboard.GetState();
            foreach (var k in Map)
            {
                Keys key = k.Key;
                ICommand command = k.Value;
                if (currentState.IsKeyDown(key) && _prev.IsKeyDown(key))
                {
                    command.Execute();
                }
            }
            _prev = currentState;

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.T))
            {
                blockSwitch = -1;
            }
            else if (state.IsKeyDown(Keys.Y))
            {
                blockSwitch = +1;
            }
            else if (state.IsKeyDown(Keys.E) && player != null)
            {
                player.TakeDamage(10); // Simulate 10 damage
            } else if(state.IsKeyDown(Keys.B) && player != null)
            {
                player.Attack(); // Player attack
                
            }
        }
    }

}
