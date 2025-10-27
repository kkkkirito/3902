using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_0
{
    public class StaticStillSprite : ISprite
    {
        private Texture2D texture;
        private Rectangle frame;
        public StaticStillSprite(Texture2D texture, Rectangle frame)
        {
            this.texture = texture;
            this.frame = frame;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects)
        {
            spriteBatch.Draw(texture, position, frame, Color.White);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
