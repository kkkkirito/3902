using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Interfaces;
using System;
using System.Collections.Generic;

namespace Project1.Sprites
{
    public abstract class BaseSprite : ISprite
    {
        protected readonly Texture2D Texture;
        protected readonly List<Rectangle> Frames;
        protected readonly float Scale;
        protected int FrameIndex;
        protected double FrameTimer;
        protected double FrameDuration;

        public bool Visible { get; set; } = true;
        public Vector2 Position { get; set; }

        protected BaseSprite(Texture2D tex, List<Rectangle> frames, Vector2 position, float scale = 3f)
        {
            Texture = tex;
            Frames = frames;
            Position = position;
            Scale = scale;
            FrameIndex = 0;
            FrameDuration = 0.12; // seconds per frame by default
        }

        protected virtual void AdvanceFrame(GameTime gameTime)
        {
            FrameTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (FrameTimer >= FrameDuration)
            {
                FrameTimer = 0;
                FrameIndex = (FrameIndex + 1) % Frames.Count;
            }
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;
            var src = Frames[FrameIndex];
            spriteBatch.Draw(Texture, Position, src, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
    }
}
