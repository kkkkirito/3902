using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.Blocks
{
    public class LavaBlock : IBlock, ILightSource
    {
        private readonly ISprite _sprite;
        private readonly float _lightRadius;
        private readonly float _intensity;
        private readonly Color _lightColor;

        private const float DefaultLightRadius = 140f;
        private const float DefaultIntensity = 0.9f;

        public LavaBlock(ISprite sprite, Vector2 position, float lightRadius = DefaultLightRadius, float intensity = DefaultIntensity)
        {
            _sprite = sprite ?? throw new ArgumentNullException(nameof(sprite));
            Position = position;
            _lightRadius = lightRadius;
            _intensity = intensity;
            _lightColor = new Color(1f, 0.25f, 0.15f, 0.8f);
        }

        public Vector2 Position { get; set; }
        public bool IsSolid => false;
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);

        public void Update(GameTime gameTime)
        {
            _sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, Position, SpriteEffects.None);
        }

        public Vector2 LightPosition => Position + new Vector2(8f, 8f);
        public float LightRadius => _lightRadius;
        public float Intensity => _intensity;
        public Color LightColor => _lightColor;
        public bool IsLightActive => true;
    }
}
