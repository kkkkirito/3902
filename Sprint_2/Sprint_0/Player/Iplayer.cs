//Dillon Brigode AU25
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Sprint_0.Interfaces;
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
    int CurrentHealth { get; set;  }
    int MaxHealth { get; }
    bool IsInvulnerable { get; }

    int CurrentFrame { get; set; }  
    
    IPlayerState CurrentState { get; set;  }



    Vector2 Velocity { get; set; }
    Texture2D SpriteSheet { get; }

    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    void TakeDamage(int damage);
    bool CanMove(Vector2 direction);
    
    void Attack();
}
