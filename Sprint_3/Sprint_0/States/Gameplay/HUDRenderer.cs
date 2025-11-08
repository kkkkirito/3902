using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Rooms;

namespace Sprint_0.States.Gameplay
{
    public sealed class HudRenderer
    {
        private const int Margin = 10;

        public static void Draw(SpriteBatch sb, SpriteFont font, GraphicsDevice gd, Room current, int idx, int total, IPlayer player)
        {
            if (font == null) return;

            string roomInfo = $"Room {idx + 1}/{total}: {current?.Name ?? "Unknown"}";
            sb.DrawString(font, roomInfo, new Vector2(Margin, Margin), Color.White);

            if (player != null)
            {
                string hp = $"Health: {player.CurrentHealth}/{player.MaxHealth}";
                sb.DrawString(font, hp, new Vector2(Margin, Margin + 20), Color.White);
            }

            string controls = "Mouse: L/R prev/next | R: Reset";


            Vector2 size = font.MeasureString(controls);
            sb.DrawString(font, controls,
                new Vector2(Margin, gd.Viewport.Height - size.Y - Margin), Color.White);
        }
    }
}
