using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public interface IConsumable : IItem
{
    void Consume(IPlayer player);
    bool IsCollected { get; }
    Rectangle GetBoundingBox();
}