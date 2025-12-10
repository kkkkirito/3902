using Microsoft.Xna.Framework;

namespace Sprint_0.Interfaces
{
    public interface ILightSource
    {
        Vector2 LightPosition { get; }
        float LightRadius { get; }
        float Intensity { get; }
        Color LightColor { get; }
        bool IsLightActive { get; }
    }
}
