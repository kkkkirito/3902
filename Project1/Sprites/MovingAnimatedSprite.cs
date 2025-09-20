using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Sprites;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

namespace SpritesDemo.Sprites
{
    // Animated + horizontal ping-pong movement (left <-> right)
    public sealed class MovingAnimatedSprite : BaseSprite
    {
        private readonly float _speed;
        private readonly float _minX;
        private readonly float _maxX;
        private int _dir = 1;            // 1 = right, -1 = left
        private bool _facingRight = true;

        public MovingAnimatedSprite(
            Texture2D tex,
            List<Rectangle> frames,
            Vector2 startPos,
            int screenWidth,
            float pixelsPerSecond = 90f,
            float scale = 3f,
            double frameSeconds = 0.1)
            : base(tex, frames, startPos, scale)
        {
            _speed = pixelsPerSecond;
            FrameDuration = frameSeconds;

            // Keep sprite fully on-screen; leave a small margin if you like (e.g., 0)
            float spriteWidth = frames[0].Width * Scale;
            _minX = 0f;
            _maxX = screenWidth - spriteWidth;
        }

        public override void Update(GameTime gameTime)
        {
            AdvanceFrame(gameTime);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = new Vector2(Position.X + _dir * _speed * dt, Position.Y);

            // Bounce at edges and flip facing
            if (Position.X <= _minX)
            {
                Position = new Vector2(_minX, Position.Y);
                _dir = 1;
                _facingRight = true;
            }
            else if (Position.X >= _maxX)
            {
                Position = new Vector2(_maxX, Position.Y);
                _dir = -1;
                _facingRight = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            var src = Frames[FrameIndex];
            var effects = _facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Texture, Position, src, Color.White, 0f, Vector2.Zero, Scale, effects, 0f);
        }
    }
}




