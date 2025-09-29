//Dillon Brigode AU25
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
public interface IPlayer
{
    Vector2 Position { get; set; }
    Direction FacingDirection { get; }
    int CurrentHealth { get; }
    int MaxHealth { get; }
    bool IsInvulnerable { get; }
    
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    void TakeDamage(int damage);
    bool CanMove(Vector2 direction);
}
