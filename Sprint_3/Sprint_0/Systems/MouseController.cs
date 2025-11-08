using Microsoft.Xna.Framework.Input;
using Sprint_0.Interfaces;

namespace Sprint_0
{
    public class MouseController(Game1 game) : IController
    {

        private readonly Game1 game = game;
        public int BlockSwitch { get; set; } = 0;

        public void Update()
        {

            var mouseState = Mouse.GetState();

            if (ButtonState.Pressed == mouseState.RightButton)
            {

            }
            else if (ButtonState.Pressed == mouseState.LeftButton)
            {

                int height = game.Window.ClientBounds.Height;
                int width = game.Window.ClientBounds.Width;


            }
        }
    }
}

