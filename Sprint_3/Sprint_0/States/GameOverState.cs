using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.States
{
    public class GameOverState : IGameState
    {
        private readonly Game1 _game;
        private readonly SpriteFont _font;
        private readonly GameStateManager _stateManager;

        private KeyboardState _prevKbd;
        private GamePadState _prevPad;

        public GameOverState(Game1 game, SpriteFont font, GameStateManager stateManager)
        {
            _game = game;
            _font = font;
            _stateManager = stateManager;
        }

        public void Enter()
        {
            AudioManager.StopBgm();
            _prevKbd = Keyboard.GetState();
            _prevPad = GamePad.GetState(PlayerIndex.One);
        }

        public void Exit()
        {
            AudioManager.PlayBgm();
        }

        public void Update(GameTime gameTime)
        {
            var k = Keyboard.GetState();
            var pad = GamePad.GetState(PlayerIndex.One);

           bool restart =
                (k.IsKeyDown(Keys.R) && !_prevKbd.IsKeyDown(Keys.R)) ||
                (k.IsKeyDown(Keys.Enter) && !_prevKbd.IsKeyDown(Keys.Enter)) ||
                (pad.IsButtonDown(Buttons.A) && !_prevPad.IsButtonDown(Buttons.A));

            bool quit =
                (k.IsKeyDown(Keys.Q) && !_prevKbd.IsKeyDown(Keys.Q)) ||
                (k.IsKeyDown(Keys.Escape) && !_prevKbd.IsKeyDown(Keys.Escape));

            if (restart)
            {
                _stateManager.ChangeState("gameplay");
                return;
            }

            if (quit)
            {
                _game.Exit();
                return;
            }

            _prevKbd = k;
            _prevPad = pad;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            var vp = _game.GraphicsDevice.Viewport;
            using (var overlay = new Texture2D(_game.GraphicsDevice, 1, 1)){
            overlay.SetData(new[] { Color.White });
            spriteBatch.Draw(overlay, new Rectangle(0, 0, vp.Width, vp.Height), Color.Red * 0.7f);
            }

            string title = "GAME OVER";
            string hint = "Press R / A (Controller) to Restart\nPress Q / Esc to Quit";

            Vector2 titleSize = _font.MeasureString(title);
            Vector2 hintSize = _font.MeasureString(hint);

            Vector2 titlePos = new Vector2((vp.Width - titleSize.X) / 2f, (vp.Height - titleSize.Y) / 2f - 20);
            Vector2 hintPos = new Vector2((vp.Width - hintSize.X) / 2f, titlePos.Y + titleSize.Y + 10);

            spriteBatch.DrawString(_font, title, titlePos, Color.White);
            spriteBatch.DrawString(_font, hint, hintPos, Color.White);

            spriteBatch.End();
        }

        public void Reset()
        {

        }
    }
}
