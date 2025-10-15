//Dillon Brigode AU25
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0;
using Sprint_0.Interfaces;
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
public enum GameModeType
{
    Platformer,
    TopDown
}
public interface IPlayer
{
    Vector2 Position { get; set; }
    Direction FacingDirection { get; }
    int CurrentHealth { get; set; }
    int MaxHealth { get; }
    bool IsInvulnerable { get; }

    int CurrentFrame { get; set; }

    IPlayerState CurrentState { get; set; }

    Vector2 Velocity { get; set; }
    Texture2D SpriteSheet { get; }

    bool IsGrounded { get; }
    bool IsCrouching { get; }

    GameModeType GameMode { get; set; }

    ICollectible HeldItem { get; }


    void Jump();
    void Pickup(ICollectible item);
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    void TakeDamage(int damage);
    bool CanMove(Vector2 direction);

    void Move(Vector2 direction);

    void Attack(Direction direction, AttackMode mode = AttackMode.Normal);
    void SetCrouch(bool crouch);
}
