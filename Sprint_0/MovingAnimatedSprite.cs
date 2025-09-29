using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Sprint_0
{
    public class MovingAnimatedSprite : ISprite
    {

        private Texture2D texture;
        private Rectangle[] frames;
        private int frame;
        private double timePerFrame = 0.33;
        private double time;
        private Vector2 position;
        private Vector2 speed;

        public MovingAnimatedSprite(Texture2D texture, Rectangle[] frames, Vector2 position, Vector2 speed)
        {

            this.texture = texture;
            this.frames = frames;
            this.position = position;
            this.speed = speed;

        }

        public void Update(GameTime gameTime)
        {

            position = position + speed;
            if (position == new Vector2(800, 240))
            {

                this.position = new Vector2(0, 240);

            }

            time = time + gameTime.ElapsedGameTime.TotalSeconds;

            if (time > timePerFrame)
            {

                if (frame >= (frames.Length - 1))
                {

                    frame = 0;

                }

                else
                {

                    frame++;

                }

                time = 0;

            }

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 positionE)
        {

            spriteBatch.Draw(texture, position, frames[frame], Color.White);

        }
    }
}
