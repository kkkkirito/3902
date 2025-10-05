using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0;

internal class Projectile
{
    private Vector2 position;
    private Vector2 velocity;
    private Animation animation;
    public bool IsActive { get; private set; } = true;

    public Projectile(Animation animation, Vector2 position, Vector2 velocity)
    {
        this.animation = animation;
        this.position = position;
        this.velocity = velocity;
        this.animation.Reset(); // start animation from frame 0
    }

    public void Update(GameTime gameTime)
    {
        // Move projectile
        position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update animation
        animation.Update(gameTime);

        // Example deactivate if off screen (hardcoded for now, you can use viewport)
        if (position.X < 0 || position.X > 800 || position.Y < 0 || position.Y > 480)
            IsActive = false;
    }

    public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
    {
        if (IsActive)
        {
            animation.Draw(spriteBatch, position, effects);
        }
    }
}