using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_0
{
    public class StaticAnimatedSprite : ISprite
    {

        private Texture2D texture;
        private Rectangle[] frames;
        private int frame;
        private double timePerFrame;
        private double time;

        public StaticAnimatedSprite(Texture2D texture, Rectangle[] frames)
        {

            this.texture = texture;
            this.frames = frames;
            this.frame = 0;
            this.timePerFrame = 0.33;

        }

        public void Update(GameTime gameTime)
        {

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

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {

            spriteBatch.Draw(texture, position, frames[frame], Color.White);

        }
    }
}
