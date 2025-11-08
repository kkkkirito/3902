//Dillon Brigode AU25
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface ICollectible : IItem
{
    Rectangle Bounds { get; }
    Texture2D Texture { get; }
    Rectangle Source { get; }
    bool IsCollected { get; }
    bool Celebration { get; }
    void Collect(IPlayer player);
}