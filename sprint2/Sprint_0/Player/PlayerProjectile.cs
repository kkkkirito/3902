// Projectile.cs  (additions marked)
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0;

internal class PlayerProjectile
{
    private Vector2 position;
    private Vector2 velocity;
    private Animation animation;
    public bool IsActive { get; private set; } = true;

    public ProjectileType Type { get; }

    public PlayerProjectile(Animation animation, Vector2 position, Vector2 velocity, ProjectileType type = ProjectileType.SwordBeam)
    {
        this.animation = animation;
        this.position = position;
        this.velocity = velocity;
        this.animation.Reset();
        Type = type;
    }

    public void Update(GameTime gameTime)
    {
        position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        animation.Update(gameTime);

        // TODO: replace hardcoded bounds with your viewport if desired
        if (position.X < 0 || position.X > 800 || position.Y < 0 || position.Y > 480)
            IsActive = false;
    }

    public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
    {
        if (IsActive) animation.Draw(spriteBatch, position, effects);
    }
}
