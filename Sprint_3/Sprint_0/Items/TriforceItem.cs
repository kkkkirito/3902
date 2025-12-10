using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.States;

namespace Sprint_0.Items
{
	public class TriforceItem : ICollectible
	{
		public string Name => "Triforce";
		public bool IsConsumable => false;
		public Vector2 Position { get; set; }
		public bool IsCollected { get; set; }
		private Animation animation;
		private Texture2D texture;

        // No GameStateManager field needed!
        public TriforceItem(Vector2 position, Texture2D itemTextures)
		{
			Position = position;
			IsCollected = false;
			texture = itemTextures;

			var animations = SpriteFactory.CreateItemAnimations(itemTextures);
			if (animations.ContainsKey("Triforce"))
			{
				this.animation = animations["Triforce"];
			}
		}

		public void Consume(IPlayer player)
		{

			if (!IsCollected)
			{
				IsCollected = true;
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
			return new Rectangle((int)Position.X, (int)Position.Y, 14, 13);
		}

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

				GameStateManager.Instance.ChangeState("victory");
			}
		}
	}
}