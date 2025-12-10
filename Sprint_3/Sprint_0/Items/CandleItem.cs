using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.Items
{
    public class CandleItem : ICollectible, ILightSource, IResettable


    {
        public string Name => "Candle";
        public bool IsConsumable => false;

        public Vector2 Position { get; set; }
        public bool IsCollected { get; set; }

        private Animation animation;
        private Texture2D texture;
        private const float CandleGlowRadius = 200f;
        private const float PlayerTorchRadius = 300f;

        public CandleItem(Vector2 position, Texture2D itemTextures)
        {
            Position = position;
            IsCollected = false;
            texture = itemTextures;

            var animations = SpriteFactory.CreateItemAnimations(itemTextures);
            if (animations.ContainsKey("Candle")) this.animation = animations["Candle"];
        }

        public void Consume(IPlayer player)
        {
            Collect(player);
        }

        public void Update(GameTime gameTime) { if (!IsCollected) animation?.Update(gameTime); }
        public void Draw(SpriteBatch spriteBatch) { if (!IsCollected) animation?.Draw(spriteBatch, Position, SpriteEffects.None); }

        public Rectangle GetBoundingBox() => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        public Rectangle BoundingBox => GetBoundingBox();
        public Rectangle Bounds => GetBoundingBox();
        public Texture2D Texture => texture;
        public Rectangle Source => animation?.CurrentFrame ?? new Rectangle(0, 0, 0, 0);
        public bool Celebration => true;

        public void Collect(IPlayer player)
        {
            if (!IsCollected)
            {
                IsCollected = true;
                player?.EnableTorch(PlayerTorchRadius);
            }
        }

        // ILightSource implementation
        public Vector2 LightPosition => Position + new Vector2(8f, 8f);
        public float LightRadius => CandleGlowRadius;
        public float Intensity => 1.0f;
        public Color LightColor => new Color(1f, 0.85f, 0.6f, 0.6f);
        public bool IsLightActive => !IsCollected;
        public void ResetState() => IsCollected = false;
    }
}
