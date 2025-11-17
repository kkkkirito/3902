using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sprint_0.Systems
{
    public class Camera
    {
        private Vector2 _position;
        private readonly Viewport _viewport;
        private Rectangle _bounds;
        private float _zoom = 1.0f;
        private float _targetZoom = 1.0f;
        private int _previousScrollValue;

        private const float FOLLOW_SPEED = 8.0f;  
        private const float ZOOM_SMOOTHING = 10.0f; 
        private const float EDGE_MARGIN = 80f;

        private const float ZOOM_SPEED = 0.15f;
        private const float MIN_ZOOM = 0.5f;
        private const float MAX_ZOOM = 3.0f;

        private const int TILE_SIZE = 16;
        private const int CONTENT_START_ROW = 1;  
        private const int CONTENT_END_ROW = 13;   
        private const int CONTENT_START_Y = CONTENT_START_ROW * TILE_SIZE; 
        private const int CONTENT_END_Y = (CONTENT_END_ROW + 1) * TILE_SIZE; 
        private const int CONTENT_HEIGHT = CONTENT_END_Y - CONTENT_START_Y; 

        public Vector2 Position
        {
            get => _position;
            set => _position = ClampToBounds(value);
        }

        public float Zoom
        {
            get => _zoom;
            set
            {
                _targetZoom = MathHelper.Clamp(value, GetMinimumZoom(), MAX_ZOOM);
            }
        }

        public Matrix TransformMatrix
        {
            get
            {
                float scaleToFit = GetScaleToFitContent();
                float effectiveZoom = _zoom * scaleToFit;

                float contentHeightScaled = CONTENT_HEIGHT * effectiveZoom;
                float yOffset = -CONTENT_START_Y * effectiveZoom;

                if (contentHeightScaled < _viewport.Height)
                {
                    yOffset += (_viewport.Height - contentHeightScaled) / 2f;
                }

                return Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y + CONTENT_START_Y, 0)) *
                       Matrix.CreateScale(effectiveZoom) *
                       Matrix.CreateTranslation(new Vector3(_viewport.Width / 2f, _viewport.Height / 2f, 0));
            }
        }

        public Rectangle VisibleArea => new Rectangle(
            (int)_position.X - _viewport.Width / 2,
            (int)_position.Y - _viewport.Height / 2,
            _viewport.Width,
            _viewport.Height
        );

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            _position = new Vector2(viewport.Width / 2f, CONTENT_START_Y + CONTENT_HEIGHT / 2f);
            var mouse = Mouse.GetState();
            _previousScrollValue = mouse.ScrollWheelValue;
            _zoom = 1.0f;
            _targetZoom = 1.0f;
        }

        public void SetBounds(int width, int height)
        {
            _bounds = new Rectangle(0, CONTENT_START_Y, width, CONTENT_HEIGHT);
        }

        public void Update(IPlayer target, GameTime gameTime)
        {
            if (target == null) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Math.Abs(_zoom - _targetZoom) > 0.001f)
            {
                _zoom = MathHelper.Lerp(_zoom, _targetZoom, ZOOM_SMOOTHING * dt);
            }

            HandleMouseZoom();

            Vector2 targetPos = new Vector2(
                target.Position.X + 8,
                target.Position.Y + 16
            );

            UpdateCameraPosition(targetPos, dt);
        }

        private void UpdateCameraPosition(Vector2 targetPos, float dt)
        {
            float scaleToFit = GetScaleToFitContent();
            float effectiveZoom = _zoom * scaleToFit;

            float deadZoneX = Math.Min(100f / effectiveZoom, _viewport.Width / (4f * effectiveZoom));
            float deadZoneY = Math.Min(60f / effectiveZoom, _viewport.Height / (4f * effectiveZoom));

            Vector2 diff = targetPos - _position;

            Vector2 desiredVelocity = Vector2.Zero;

            if (Math.Abs(diff.X) > deadZoneX)
            {
                desiredVelocity.X = (diff.X - Math.Sign(diff.X) * deadZoneX);
            }

            if (Math.Abs(diff.Y) > deadZoneY)
            {
                desiredVelocity.Y = (diff.Y - Math.Sign(diff.Y) * deadZoneY);
            }

            if (desiredVelocity != Vector2.Zero)
            {
                Vector2 newPosition = _position + desiredVelocity * FOLLOW_SPEED * dt;
                _position = ClampToBounds(newPosition);
            }
        }

        private void HandleMouseZoom()
        {
            var mouse = Mouse.GetState();
            int scrollDelta = mouse.ScrollWheelValue - _previousScrollValue;

            if (scrollDelta != 0)
            {
                float zoomChange = (scrollDelta > 0 ? ZOOM_SPEED : -ZOOM_SPEED);
                float newZoom = _targetZoom * (1 + zoomChange);

                Zoom = newZoom;

                _position = ClampToBounds(_position);
            }

            _previousScrollValue = mouse.ScrollWheelValue;
        }

        private float GetScaleToFitContent()
        {
            return (float)_viewport.Height / CONTENT_HEIGHT;
        }

        private float GetMinimumZoom()
        {
            float scaleToFit = GetScaleToFitContent();

            float minZoomX = (float)_viewport.Width / (_bounds.Width * scaleToFit);

            float minZoomY = 1.0f;

            return Math.Max(Math.Max(minZoomX, minZoomY), MIN_ZOOM);
        }

        public void SnapToTarget(IPlayer target)
        {
            if (target == null) return;
            Vector2 targetPos = new Vector2(
                target.Position.X + 8,
                target.Position.Y + 16
            );
            _position = ClampToBounds(targetPos);
        }

        private Vector2 ClampToBounds(Vector2 position)
        {
            float scaleToFit = GetScaleToFitContent();
            float effectiveZoom = _zoom * scaleToFit;

            float viewWidth = _viewport.Width / effectiveZoom;
            float viewHeight = _viewport.Height / effectiveZoom;

            float halfWidth = viewWidth / 2f;
            float halfHeight = viewHeight / 2f;

            float minX = halfWidth;
            float maxX = _bounds.Width - halfWidth;

            if (viewWidth >= _bounds.Width)
            {
                position.X = _bounds.Width / 2f;
            }
            else
            {
                position.X = MathHelper.Clamp(position.X, minX, maxX);
            }

            float minY = CONTENT_START_Y + halfHeight;
            float maxY = CONTENT_END_Y - halfHeight;

            if (viewHeight >= CONTENT_HEIGHT)
            {
                position.Y = CONTENT_START_Y + CONTENT_HEIGHT / 2f;
            }
            else
            {
                position.Y = MathHelper.Clamp(position.Y, minY, maxY);
            }

            return position;
        }

        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, Matrix.Invert(TransformMatrix));
        }

        public Vector2 WorldToScreen(Vector2 worldPos)
        {
            return Vector2.Transform(worldPos, TransformMatrix);
        }

        public void ResetZoom()
        {
            _targetZoom = 1.0f;
        }

        public void ZoomIn()
        {
            Zoom = _targetZoom + ZOOM_SPEED;
        }

        public void ZoomOut()
        {
            Zoom = _targetZoom - ZOOM_SPEED;
        }
    }
}