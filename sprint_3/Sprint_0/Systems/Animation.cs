using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Sprint_0
{
    internal class Animation : ISprite
    {
        private Texture2D spriteSheet;
        private List<Rectangle> frames;
        private List<Vector2> offsets;
        private float frameTime;
        private bool isLooping;

        private int currentFrame;
        private float timer;


        public Animation(Texture2D spriteSheet, List<Rectangle> frames, float frameTime, bool isLooping, List<Vector2>? offsets = null)
        {
            this.spriteSheet = spriteSheet;
            this.frames = frames;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
            this.offsets = offsets ?? Enumerable.Repeat(Vector2.Zero, frames.Count).ToList();

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
            Rectangle source = frames[currentFrame];
            Vector2 offset = offsets[currentFrame];
            if ((effects & SpriteEffects.FlipHorizontally) != 0)
            {
                offset = new Vector2(-offset.X, offset.Y);
            }
            spriteBatch.Draw(spriteSheet, position + offset, source, Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
        }

        public void Reset()
        {
            currentFrame = 0;
            timer = 0f;
        }
        public Animation Clone()
        {
            return new Animation(
                this.spriteSheet,
                new List<Rectangle>(this.frames),
                this.frameTime,
                this.isLooping,
                new List<Vector2>(this.offsets)
            );
        }
        public int FrameWidth => frames[currentFrame].Width;
        public int FrameHeight => frames[currentFrame].Height;
        public Rectangle CurrentFrame => frames[currentFrame];
    }

}
