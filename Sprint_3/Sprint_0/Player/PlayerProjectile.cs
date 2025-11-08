using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0;
using Sprint_0.Interfaces;

internal class PlayerProjectile : ICollidable
{

    private Game game;
    private Vector2 position;
    private Vector2 velocity;
    private readonly Animation animation;
    public bool IsActive { get; set; } = true;

    public ProjectileType Type { get; }

    public PlayerProjectile(Game game, Animation animation, Vector2 position, Vector2 velocity, ProjectileType type = ProjectileType.SwordBeam)
    {
        this.animation = animation;
        this.position = position;
        this.velocity = velocity;
        this.animation.Reset();
        Type = type;
        this.game = game;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsActive) return;

        position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        animation.Update(gameTime);

        
        if (position.X < 0 || position.X > game.GraphicsDevice.Viewport.Width || position.Y < 0 || position.Y > 500)
            IsActive = false;
    }

    public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
    {
        if (IsActive) animation.Draw(spriteBatch, position, effects);
    }

    public Rectangle BoundingBox
    {
        get
        {
            int w = animation.FrameWidth;
            int h = animation.FrameHeight;
            return new Rectangle((int)position.X, (int)position.Y, w, h);
        }
    }
}
