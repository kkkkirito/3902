using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_0
{
    public class MovingStillSprite : ISprite
    {

        private Texture2D texture;
        private Rectangle frame;
        private Vector2 position;
        private Vector2 speed;

        public MovingStillSprite(Texture2D texture, Rectangle frame, Vector2 position, Vector2 speed)
        {
            this.texture = texture;
            this.frame = frame;
            this.position = position;
            this.speed = speed;

        }

        public void Update(GameTime gameTime)
        {

            position = position + speed;
            if (position == new Vector2(400, 480))
            {

                this.position = new Vector2(400, 0);

            }

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 positionE)
        {

            spriteBatch.Draw(texture, position, frame, Color.White);

        }
    }
}
