using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprint_0.Systems
{
    public class Camera
    {
        private Vector2 _position;
        private readonly Viewport _viewport;
        private Rectangle _bounds;
        private float _zoom = 1.0f;

        // Smooth follow parameters
        private const float FOLLOW_SPEED = 5.0f;
        private const float EDGE_MARGIN = 200f; // Distance from edge before camera starts moving

        public Vector2 Position
        {
            get => _position;
            set => _position = ClampToBounds(value);
        }

        public float Zoom
        {
            get => _zoom;
            set => _zoom = MathHelper.Clamp(value, 0.5f, 2.0f);
        }

        public Matrix TransformMatrix =>
            Matrix.CreateTranslation(new Vector3(-_position, 0)) *
            Matrix.CreateScale(_zoom) *
            Matrix.CreateTranslation(new Vector3(_viewport.Width / 2f, _viewport.Height / 2f, 0));

        public Rectangle VisibleArea => new Rectangle(
            (int)_position.X - _viewport.Width / 2,
            (int)_position.Y - _viewport.Height / 2,
            _viewport.Width,
            _viewport.Height
        );

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            _position = Vector2.Zero;
        }

        public void SetBounds(int width, int height)
        {
            _bounds = new Rectangle(0, 0, width, height);
        }

        public void Update(IPlayer target, GameTime gameTime)
        {
            if (target == null) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 targetPos = target.Position + new Vector2(8, 16); // Center on player

            // Smooth follow with dead zone
            Vector2 screenPos = WorldToScreen(targetPos);
            Vector2 center = new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
            Vector2 offset = screenPos - center;

            // Only move camera if player is near edges
            if (Math.Abs(offset.X) > EDGE_MARGIN || Math.Abs(offset.Y) > EDGE_MARGIN)
            {
                Vector2 desiredPos = targetPos;
                _position = Vector2.Lerp(_position, desiredPos, FOLLOW_SPEED * dt);
                _position = ClampToBounds(_position);
            }
        }

        public void SnapToTarget(IPlayer target)
        {
            if (target == null) return;
            _position = ClampToBounds(target.Position + new Vector2(8, 16));
        }

        private Vector2 ClampToBounds(Vector2 position)
        {
            float halfWidth = _viewport.Width / (2f * _zoom);
            float halfHeight = _viewport.Height / (2f * _zoom);

            float x = MathHelper.Clamp(position.X, halfWidth, Math.Max(_bounds.Width - halfWidth, halfWidth));
            float y = MathHelper.Clamp(position.Y, halfHeight, Math.Max(_bounds.Height - halfHeight, halfHeight));

            return new Vector2(x, y);
        }

        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, Matrix.Invert(TransformMatrix));
        }

        public Vector2 WorldToScreen(Vector2 worldPos)
        {
            return Vector2.Transform(worldPos, TransformMatrix);
        }
    }
}