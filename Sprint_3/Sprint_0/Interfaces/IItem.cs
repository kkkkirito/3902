//Dillon Brigode AU 2025

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IItem
{
    string Name { get; }
    bool IsConsumable { get; }
}

public interface IUsableItem : IItem
{
    void Use(IPlayer player);
    bool CanUse(IPlayer player);
}
public interface IConsumableItem : IItem
{
    void Consume(IPlayer player); 
    bool IsCollected { get; }
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    Rectangle GetBoundingBox();
}