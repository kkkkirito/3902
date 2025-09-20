using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Interfaces;

namespace SpritesDemo.Sprites
{
    public sealed class TextSprite : ISprite
    {
        private readonly SpriteFont _font;
        private readonly string _text;
        private readonly Color _color;

        public bool Visible { get; set; } = true;
        public Vector2 Position { get; set; }

        public TextSprite(SpriteFont font, string text, Vector2 position, Color color)
        {
            _font = font;
            _text = text;
            Position = position;
            _color = color;
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;
            spriteBatch.DrawString(_font, _text, Position, _color);
        }
    }
}

