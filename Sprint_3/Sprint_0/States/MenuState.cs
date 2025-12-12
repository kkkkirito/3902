using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.States
{
    public class MenuState : IGameState
    {
        private Game1 game;
        private SpriteFont font;
        private KeyboardState previousKeyboardState;
        private GamePadState previousGamePadState;
        private GameStateManager stateManager;
        private IAudioManager audio;

        public MenuState(Game1 game, SpriteFont font, GameStateManager stateManager, IAudioManager audio)
        {
            this.game = game;
            this.font = font;
            this.stateManager = stateManager;
            this.audio = audio;

        }

        public void Enter()
        {
            // Initialize menu
        }

        public void Exit()
        {
            // Clean up menu
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            GamePadState pad = GamePad.GetState(PlayerIndex.One);

            if ((currentKeyboardState.IsKeyDown(Keys.Enter) &&
                !previousKeyboardState.IsKeyDown(Keys.Enter)) || 
                (pad.IsButtonDown(Buttons.A) && !previousGamePadState.IsButtonDown(Buttons.A)))
            {
                stateManager.ChangeState("gameplay");
            }

            previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            string menuText = "Press ENTER or Button 'A' to start";
            Vector2 textSize = font.MeasureString(menuText);
            Vector2 position = new Vector2(
                (game.GraphicsDevice.Viewport.Width - textSize.X) / 2,
                (game.GraphicsDevice.Viewport.Height - textSize.Y) / 2
            );

            spriteBatch.DrawString(font, menuText, position, Color.White);

            spriteBatch.End();
        }

        public void Reset()
        {
            // Reset menu if needed
        }
    }
}