using Microsoft.Xna.Framework;
using System;

namespace Sprint_0.Systems.Lighting{
    public readonly struct LightSourceData(Vector2 position, float radius, Color color, float intensity)
    {
        public Vector2 Position { get; } = position;
        public float Radius { get; } = Math.Max(radius, 1f);
        public Color Color { get; } = color;
        public float Intensity { get; } = Math.Max(intensity, 0f);
    }
}
