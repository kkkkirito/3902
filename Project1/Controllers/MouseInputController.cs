using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project1.Interfaces;
using Project1.Commands;
using Project1.Interfaces;

namespace Project1.Controllers
{
    public sealed class MouseInputController : IController
    {
        private readonly GraphicsDeviceManager _gdm;
        private readonly ICommand _quit;
        private readonly ICommand _setStatic;
        private readonly ICommand _setAnimated;
        private readonly ICommand _setFloat;
        private readonly ICommand _setRun;

        private MouseState _prev;

        public MouseInputController(
            GraphicsDeviceManager gdm,
            ICommand quit,
            ICommand setStatic,
            ICommand setAnimated,
            ICommand setFloat,
            ICommand setRun)
        {
            _gdm = gdm;
            _quit = quit;
            _setStatic = setStatic;
            _setAnimated = setAnimated;
            _setFloat = setFloat;
            _setRun = setRun;
        }

        public void Update(GameTime gameTime)
        {
            var ms = Mouse.GetState();

            bool leftClick = ms.LeftButton == ButtonState.Pressed && _prev.LeftButton == ButtonState.Released;
            bool rightClick = ms.RightButton == ButtonState.Pressed && _prev.RightButton == ButtonState.Released;

            if (rightClick)
            {
                _quit.Execute();
            }
            else if (leftClick)
            {
                int w = _gdm.PreferredBackBufferWidth;
                int h = _gdm.PreferredBackBufferHeight;

                bool top = ms.Y < h / 2;
                bool left = ms.X < w / 2;

                if (top && left) _setStatic.Execute();           // Quad 1
                else if (top && !left) _setAnimated.Execute();   // Quad 2
                else if (!top && left) _setFloat.Execute();      // Quad 3
                else _setRun.Execute();                          // Quad 4
            }

            _prev = ms;
        }
    }
}

