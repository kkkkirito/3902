using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                game.DrawSprite(0);

            }
            else if (ButtonState.Pressed == mouseState.LeftButton)
            {

                int height = game.Window.ClientBounds.Height;
                int width = game.Window.ClientBounds.Width;

                if (mouseState.Y <= height/2  && mouseState.X <= width/2)
                {

                    game.DrawSprite(1);

                }
                else if (mouseState.Y <= height/2 && mouseState.X > width/2)
                {

                    game.DrawSprite(2);

                }
                else if (mouseState.Y > height/2 && mouseState.X <= width/2)
                {

                    game.DrawSprite(3);

                }
                else
                {

                    game.DrawSprite(4);

                }
            }
        }
    }
}
