using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0
{
    internal class Animation
    {
        private Texture2D spriteSheet;
        private List<Rectangle> frames;
        private float frameTime;
        private bool isLooping;

        private int currentFrame;
        private float timer;

        public Animation(Texture2D spriteSheet, List<Rectangle> frames, float frameTime, bool isLooping)
        {
            this.spriteSheet = spriteSheet;
            this.frames = frames;
            this.frameTime = frameTime;
            this.isLooping = isLooping;

            currentFrame = 0;
            timer = 0f;
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= frameTime)
            {
                timer -= frameTime;
                currentFrame++;

                if (currentFrame >= frames.Count)
                {
                    if (isLooping)
                        currentFrame = 0;
                    else
                        currentFrame = frames.Count - 1;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects)
        {
            spriteBatch.Draw(
                spriteSheet,
                position,
                frames[currentFrame],
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                effects,
                0f
            );
        }

        public void Reset()
        {
            currentFrame = 0;
            timer = 0f;
        }
    }
}
