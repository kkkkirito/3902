using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0
{
    public class TextSprite : ISprite
    {

        private string text;
        private SpriteFont font;
        private Color color;
        private Vector2 position;


        public TextSprite(string text, SpriteFont font, Color color, Vector2 position)
        {
            this.text = text;
            this.font = font;
            this.color = color;
            this.position = position;
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {

            spriteBatch.DrawString(font, text, position, color);

        }

    }
}
