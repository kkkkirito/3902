// Items/HeartItem.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.Items
{
    public class HeartItem : IConsumable, IResettable
    {
        public string Name => "Heart";
        public bool IsConsumable => true;

        public Vector2 Position { get; set; }
        public bool IsCollected { get; set; }

        private Animation animation;
        private int healAmount = 20;
        private Texture2D texture;

        public HeartItem(Vector2 position, Texture2D itemTextures)
        {
            Position = position;
            IsCollected = false;
            texture = itemTextures;

            var animations = SpriteFactory.CreateItemAnimations(itemTextures);
            if (animations.ContainsKey("Heart")) this.animation = animations["Heart"];
        }

        public void Consume(IPlayer player)
        {
            if (!IsCollected)
            {
                player.CurrentHealth = System.Math.Min(player.CurrentHealth + healAmount, player.MaxHealth);
                IsCollected = true;
            }
        }

        // ICollectible implementation
        public Rectangle Bounds => GetBoundingBox();
        public Texture2D Texture => texture;
        public Rectangle Source => animation?.CurrentFrame ?? new Rectangle(0, 0, 0, 0);
        public bool Celebration => false;
        public void Collect(IPlayer player) => Consume(player);

        public void Update(GameTime gameTime) { if (!IsCollected) animation?.Update(gameTime); }
        public void Draw(SpriteBatch spriteBatch) { if (!IsCollected) animation?.Draw(spriteBatch, Position, SpriteEffects.None); }
        public Rectangle GetBoundingBox() => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        public Rectangle BoundingBox => GetBoundingBox();

        // IResettable
        public void ResetState() => IsCollected = false;
    }
}