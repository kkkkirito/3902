using Microsoft.Xna.Framework.Input;

namespace Sprint_0
{
    public class MouseController : IController
    {

        private Game1 game;
        public int blockSwitch { get; set; } = 0;
        public MouseController(Game1 game)
        {

            this.game = game;

        }
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

