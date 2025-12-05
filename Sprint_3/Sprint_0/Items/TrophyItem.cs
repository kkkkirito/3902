// Items/TrophyItem.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.Items
{
    public class TrophyItem : ICollectible, IResettable
    {
        public string Name => "Trophy";
        public bool IsConsumable => false;

        public Vector2 Position { get; set; }
        public bool IsCollected { get; set; }

        private Animation animation;
        private Texture2D texture;

        public TrophyItem(Vector2 position, Texture2D itemTextures)
        {
            Position = position;
            IsCollected = false;
            texture = itemTextures;

            var animations = SpriteFactory.CreateItemAnimations(itemTextures);
            if (animations.ContainsKey("Trophy"))
                this.animation = animations["Trophy"];
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

        public Rectangle GetBoundingBox() => new Rectangle((int)Position.X, (int)Position.Y, 14, 15);
        public Rectangle BoundingBox => GetBoundingBox();
        public Rectangle Bounds => GetBoundingBox();
        public Texture2D Texture => texture;
        public Rectangle Source => animation?.CurrentFrame ?? new Rectangle(0, 0, 0, 0);
        public bool Celebration => true;

        public void Collect(IPlayer player)
        {
            if (IsCollected) return;
            IsCollected = true;
            player.GameMode = GameModeType.Platformer;
            if (player is Sprint_0.Player_Namespace.Player p)
            {
                p.HasTopDownKey = false;
            }
        }

        public void ResetState()
        {
            IsCollected = false;
        }
    }
}