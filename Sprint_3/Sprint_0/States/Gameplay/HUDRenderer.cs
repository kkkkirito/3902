using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Rooms;
using System;

namespace Sprint_0.States.Gameplay
{
    public sealed class HudRenderer
    {
        private const int Margin = 10;

        private const int RowGap = 10;
        private const int HeartSize = 10;
        private const int HeartGap  = 0;
        private const int HeartPad = 1;
        private const int HpPerHeart = 20;
        private const int MaxHearts = 5;
        private const int MagicBarWidth = 90;
        private const int MagicBarHeight = 10;
        private const int MagicBarPad = 1;
        private const int MapX = 678;
        private const int MapY = 5;
        private const int MapRoomSize = 9;
        

        private const int TopHudStripHeight = 30;

        private static readonly Color HeartFillColor = Color.Red;
        private static readonly Color HeartEmptyColor = Color.DimGray;
        private static readonly Color HeartFrameColor = Color.Black;

        private static readonly Color MagicFillColor = Color.DodgerBlue;
        private static readonly Color MagicEmptyColor = Color.DimGray;
        private static readonly Color MagicFrameColor = Color.Black;
        private readonly Texture2D _pixel;
        private readonly Texture2D _hudTexture;


        public HudRenderer(Texture2D pixel, Texture2D hudTexture)
        {
            _pixel = pixel;
            _hudTexture = hudTexture;
        }

        public void Draw(SpriteBatch sb, SpriteFont font, GraphicsDevice graphicsDevice, Room currentRoom, IPlayer player)
        {
            if (font == null || sb == null || player == null) return;

            var vp = graphicsDevice.Viewport;
            DrawFilledRect(sb, new Rectangle(0, 0, vp.Width, TopHudStripHeight), Color.Black * 0.75f);

            var cursor = new Vector2(Margin, Margin);


            int heartsWidth = DrawHearts(sb, cursor, player.CurrentHealth, player.MaxHealth);
            cursor.X += heartsWidth + RowGap;

            int magicWidth = DrawMagicBar(sb, cursor, player.CurrentMagic, player.MaxMagic);
            cursor.X += magicWidth + RowGap;

            string xp = $"NEXT {player.CurrentXP:D4}/{player.NextLevelXP:D4}";
            sb.DrawString(font, xp, cursor, Color.White);
            cursor.X += font.MeasureString(xp).X + RowGap;

            string lives = $"LIVES {player.Lives}";
            sb.DrawString(font, lives, cursor, Color.White);

            DrawMap(sb, MapX, MapY);


            if (currentRoom != null)
            {
                string room = currentRoom.Name ?? "Unknown";
                var roomSize = font.MeasureString(room);
                sb.DrawString(font, room, new Vector2(Margin, sb.GraphicsDevice.Viewport.Height - roomSize.Y - Margin), Color.White);
            }
            //purely for testing purposes. can be removed once damanges are tuned.
            if (player != null)
            {
                string hp = $"Health: {player.CurrentHealth}/{player.MaxHealth}";
                sb.DrawString(font, hp, new Vector2(Margin, Margin + 20), Color.White);
            }

            string controls = "Mouse: L/R prev/next | R: Reset | M: MuteBGM";


            Vector2 size = font.MeasureString(controls);
            sb.DrawString(font, controls,
                new Vector2(Margin, 2 * sb.GraphicsDevice.Viewport.Height - size.Y - Margin), Color.White);
        }

        private void DrawFilledRect(SpriteBatch sb, Rectangle rect, Color color)
        {
            sb.Draw(_pixel, rect, color);
        }

        private int DrawHearts(SpriteBatch sb, Vector2 pos, int hp, int maxHp)
        {
            for (int i = 0; i < MaxHearts; i++)
            {
                int heartStartHp = i * HpPerHeart;
                int hpInThisHeart = Math.Clamp(hp - heartStartHp, 0, HpPerHeart);
                float fill = hpInThisHeart / (float)HpPerHeart; 

                int x = (int)pos.X + i * (HeartSize + HeartGap);
                int y = (int)pos.Y;
                var bounds = new Rectangle(x, y, HeartSize, HeartSize);

                DrawBlockWithFrame(sb, bounds, fill, HeartFillColor, HeartEmptyColor, HeartFrameColor, HeartPad);
            }
            return MaxHearts * (HeartSize + HeartGap) - HeartGap; // total width of hearts drawn
        }

        private int DrawMagicBar(SpriteBatch sb, Vector2 pos, int currentMagic, int maxMagic)
        {
            maxMagic = Math.Max(1, maxMagic); // Div by 0
            float fill01 = Math.Clamp(currentMagic / (float)maxMagic, 0f, 1f);

            var outer = new Rectangle((int)pos.X, (int)pos.Y, MagicBarWidth, MagicBarHeight);
            DrawBlockWithFrame(sb, outer, fill01, MagicFillColor, MagicEmptyColor, MagicFrameColor, MagicBarPad);

            return MagicBarWidth;
        }
        private void DrawBlockWithFrame(SpriteBatch sb, Rectangle r, float fill01, Color fillColor, Color emptyColor, Color frameColor, int innerPad)
        {
            // frame
            sb.Draw(_pixel, new Rectangle(r.X, r.Y, r.Width, 1), frameColor);
            sb.Draw(_pixel, new Rectangle(r.X, r.Y + r.Height - 1, r.Width, 1), frameColor);
            sb.Draw(_pixel, new Rectangle(r.X, r.Y, 1, r.Height), frameColor);
            sb.Draw(_pixel, new Rectangle(r.X + r.Width - 1, r.Y, 1, r.Height), frameColor);

            int innerW = Math.Max(0, r.Width - 2 * innerPad);
            int innerH = Math.Max(0, r.Height - 2 * innerPad);
            var inner = new Rectangle(r.X + innerPad, r.Y + innerPad, innerW, innerH);

            sb.Draw(_pixel, inner, emptyColor);

            int filledW = (int)Math.Round(innerW * Math.Clamp(fill01, 0f, 1f));
            if (filledW > 0)
            {
                var filled = new Rectangle(inner.X, inner.Y, filledW, innerH);
                sb.Draw(_pixel, filled, fillColor);
            }
        }

        private void DrawMap(SpriteBatch sb, int startingX, int startingY)
        {
            sb.Draw(_pixel, new Rectangle(startingX, startingY, 117, 45), Color.Beige);

            sb.Draw(_pixel, new Rectangle(startingX + 9 * 8, startingY, MapRoomSize, MapRoomSize), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 9, startingY + 3, 9, 3), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 10, startingY, MapRoomSize, MapRoomSize), Color.Black);

            sb.Draw(_pixel, new Rectangle(startingX + 9 * 8 + 3, startingY + 9 * 1, 3, 9), Color.Black);

            sb.Draw(_pixel, new Rectangle(startingX, startingY + 9 * 2, MapRoomSize, MapRoomSize), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 1, startingY + 9 * 2 + 3, 9, 3), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 2, startingY + 9 * 2, MapRoomSize, MapRoomSize), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 3, startingY + 9 * 2 + 3, 9, 3), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 4, startingY + 9 * 2, MapRoomSize, MapRoomSize), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 5, startingY + 9 * 2 + 3, 9, 3), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 6, startingY + 9 * 2, MapRoomSize, MapRoomSize), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 7, startingY + 9 * 2 + 3, 9, 3), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 8, startingY + 9 * 2, MapRoomSize, MapRoomSize), Color.Black);

            sb.Draw(_pixel, new Rectangle(startingX + 9 * 6 + 3, startingY + 9 * 3, 3, 9), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 8 + 3, startingY + 9 * 3, 3, 9), Color.Black);

            sb.Draw(_pixel, new Rectangle(startingX + 9 * 2, startingY + 9 * 4, MapRoomSize, MapRoomSize), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 3, startingY + 9 * 4 + 3, 9, 3), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 4, startingY + 9 * 4, MapRoomSize, MapRoomSize), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 5, startingY + 9 * 4 + 3, 9, 3), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 6, startingY + 9 * 4, MapRoomSize, MapRoomSize), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 8, startingY + 9 * 4, MapRoomSize, MapRoomSize), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 9, startingY + 9 * 4 + 3, 9, 3), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 10, startingY + 9 * 4, MapRoomSize, MapRoomSize), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 11, startingY + 9 * 4 + 3, 9, 3), Color.Black);
            sb.Draw(_pixel, new Rectangle(startingX + 9 * 12, startingY + 9 * 4, MapRoomSize, MapRoomSize), Color.Black);
        }
    }
}
