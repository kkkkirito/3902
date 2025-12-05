using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_0.Items
{
    public class TopDownKeyItem : ICollectible
    {
        public string Name => "TopDownKey";
        public bool IsConsumable => false;

        public Vector2 Position { get; set; }
        public bool IsCollected { get; set; }

        private Animation animation;
        private Texture2D texture;

        public TopDownKeyItem(Vector2 position, Texture2D itemTextures)
        {
            Position = position;
            IsCollected = false;
            texture = itemTextures;

            var animations = SpriteFactory.CreateItemAnimations(itemTextures);
            if (animations.ContainsKey("MagicKey"))
            {
                this.animation = animations["MagicKey"];
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!IsCollected)
            {
                animation?.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
            {
                animation?.Draw(spriteBatch, Position, SpriteEffects.None);
            }
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        }

        public Rectangle BoundingBox => GetBoundingBox();
        public Rectangle Bounds => GetBoundingBox();
        public Texture2D Texture => texture;
        public Rectangle Source => animation?.CurrentFrame ?? new Rectangle(0, 0, 0, 0);
        public bool Celebration => true;

        public void Collect(IPlayer player)
        {
            if (IsCollected) return;
            IsCollected = true;
            if (player != null)
                player.TopDownKeyCount++;
        }
    }
}