using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0;
using Sprint_0.Interfaces;

internal class PlayerProjectile : ICollidable, ILightSource
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
    public Vector2 LightPosition => position + new Vector2(animation.FrameWidth / 2f, animation.FrameHeight / 2f);
    public float LightRadius => Type == ProjectileType.Fireball ? 48f : 0f;
    public float Intensity => Type == ProjectileType.Fireball && IsActive ? 1.0f : 0f;
    public Color LightColor => Type == ProjectileType.Fireball ? new Color(1f, 0.55f, 0.15f, 0.9f) : Color.White;
    public bool IsLightActive => Type == ProjectileType.Fireball && IsActive;
}
