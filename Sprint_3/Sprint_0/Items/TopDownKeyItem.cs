// Items/TopDownKeyItem.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.Items
{
    public class TopDownKeyItem : ICollectible, IResettable
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
            if (animations.ContainsKey("TopDownKey"))
                this.animation = animations["TopDownKey"];
        }

        public void Update(GameTime gameTime)
        {
            if (!IsCollected)
                animation?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
                animation?.Draw(spriteBatch, Position, SpriteEffects.None);
        }

        public Rectangle GetBoundingBox() => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        public Rectangle BoundingBox => GetBoundingBox();
        public Rectangle Bounds => GetBoundingBox();
        public Texture2D Texture => texture;
        public Rectangle Source => animation?.CurrentFrame ?? new Rectangle(0, 0, 0, 0);
        public bool Celebration => true;

        public void Collect(IPlayer player)
        {
            if (IsCollected) return;
            IsCollected = true;
            if (player is Sprint_0.Player_Namespace.Player p)
            {
                p.HasTopDownKey = true;
            }
        }

        public void ResetState()
        {
            IsCollected = false;
        }
    }
}
