using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint_0.States
{
    public static class PauseState
    {
        public static bool IsPaused { get; private set; }
        public static void Update(){}

        public static void setPaused(bool paused) => IsPaused = paused;
        public static void Toggle() => IsPaused = !IsPaused;
        public static void Draw(SpriteBatch spriteBatch, SpriteFont font, GraphicsDevice graphicsDevice)
        {
            if (!IsPaused) return;

            // Begin/end are handled here so callers can simply call PauseState.Draw(...)
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            var vp = graphicsDevice.Viewport;
            var fullRect = new Rectangle(0, 0, vp.Width, vp.Height);

            // Draw translucent black overlay
            using (var pixel = new Texture2D(graphicsDevice, 1, 1))
            {
                pixel.SetData(new[] { Color.White });
                spriteBatch.Draw(pixel, fullRect, Color.Black * 0.6f);
            }

            // Text
            string title = "GAME PAUSED";
            string hint = "Press ESC / P / START to resume";

            Vector2 titleSize = font.MeasureString(title);
            Vector2 hintSize = font.MeasureString(hint);

            Vector2 titlePos = new Vector2((vp.Width - titleSize.X) / 2f, (vp.Height - titleSize.Y) / 2f - 12f);
            Vector2 hintPos = new Vector2((vp.Width - hintSize.X) / 2f, titlePos.Y + titleSize.Y + 8f);

            // Drop shadow for better readability
            var shadowOffset = new Vector2(2f, 2f);
            spriteBatch.DrawString(font, title, titlePos + shadowOffset, Color.Black * 0.7f);
            spriteBatch.DrawString(font, hint, hintPos + shadowOffset, Color.Black * 0.6f);

            // Main text
            spriteBatch.DrawString(font, title, titlePos, Color.White);
            spriteBatch.DrawString(font, hint, hintPos, Color.White * 0.95f);

            spriteBatch.End();
        }
    }
    
}