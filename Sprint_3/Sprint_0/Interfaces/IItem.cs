//Dillon Brigode AU 2025
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

public interface IItem: ICollidable
{
    string Name { get; }
    bool IsConsumable { get; }
    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);
}

public interface IUsableItem : IItem
{
    void Use(IPlayer player);
    bool CanUse(IPlayer player);
}
