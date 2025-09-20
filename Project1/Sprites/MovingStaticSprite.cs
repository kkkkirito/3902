using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Sprites;
using System.Collections.Generic;

namespace SpritesDemo.Sprites
{
    public sealed class MovingStaticSprite : BaseSprite
    {
        private readonly float _minY;
        private readonly float _maxY;
        private float _speed;
        private int _dir = 1;

        public MovingStaticSprite(Texture2D tex, Rectangle frame, Vector2 startPos, float minY, float maxY, float pixelsPerSecond = 50f, float scale = 3f)
            : base(tex, new List<Rectangle> { frame }, startPos, scale)
        {
            _minY = minY; _maxY = maxY; _speed = pixelsPerSecond;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = new Vector2(Position.X, Position.Y + _dir * _speed * dt);
            if (Position.Y < _minY) { Position = new Vector2(Position.X, _minY); _dir = 1; }
            if (Position.Y > _maxY) { Position = new Vector2(Position.X, _maxY); _dir = -1; }
        }
    }
}
