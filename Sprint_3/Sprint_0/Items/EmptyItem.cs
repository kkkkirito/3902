//Dillon Brigode AU 2025
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public sealed class EmptyItem : IUsableItem
{
    private Vector2 Position { get; set; }
    
    public static EmptyItem Instance = new();
    public string Name => "Empty";
    public bool IsConsumable => false;
    public bool CanUse(IPlayer player) => false;
    public void Use(IPlayer player) { /* no-op */ }
    public EmptyItem() { }

    public void Update(GameTime gameTime) { /* no-op */ }
    public void Draw(SpriteBatch spriteBatch) { /* no-op */ }
    public Rectangle BoundingBox => new((int)Position.X, (int)Position.Y, 16, 16);
}