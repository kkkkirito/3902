using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal class Projectile
{
    private Texture2D texture;
    private Rectangle sourceRect;
    private Vector2 position;
    private Vector2 velocity;
    public bool IsActive { get; private set; } = true;

    public Projectile(Texture2D texture, Rectangle sourceRect, Vector2 position, Vector2 velocity)
    {
        this.texture = texture;
        this.sourceRect = sourceRect;
        this.position = position;
        this.velocity = velocity;
    }

    public void Update(GameTime gameTime)
    {
        position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // deactivate if off-screen or hits something
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White);
        }
            
    }
}