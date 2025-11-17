using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint_0.Managers
{
	public class XPManager
	{
		private static List<XPManager> _allFloatingXPs = new List<XPManager>();

		public Vector2 Position { get; set; }
		public int XPValue { get; set; }
		public float Timer { get; set; }
		private Texture2D spriteSheet;

		private XPManager(Texture2D enemySpriteSheet, Vector2 position, int xpValue)
		{
			spriteSheet = enemySpriteSheet;
			Position = position;
			XPValue = xpValue;
			Timer = 0f;
		}

		public static void Spawn(Texture2D enemySpriteSheet, Vector2 position, int xpValue)
		{
			_allFloatingXPs.Add(new XPManager(enemySpriteSheet, position, xpValue));
		}

		public static void UpdateAll(GameTime gameTime)
		{
			foreach (var xp in _allFloatingXPs)
			{
				xp.Update(gameTime);
			}
			_allFloatingXPs.RemoveAll(xp => xp.ShouldRemove);
		}

		public static void DrawAll(SpriteBatch spriteBatch)
		{
			foreach (var xp in _allFloatingXPs)
			{
				xp.Draw(spriteBatch);
			}
		}

		public static void Clear()
		{
			_allFloatingXPs.Clear();
		}

		private void Update(GameTime gameTime)
		{
			Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			Position = Position - new Vector2(0, 20f * (float)gameTime.ElapsedGameTime.TotalSeconds);
		}

		public bool ShouldRemove => Timer >= 1.5f;

		private void Draw(SpriteBatch spriteBatch)
		{
			string xpText = XPValue.ToString();
			float alpha = 1f - (Timer / 1.5f);

			Vector2 drawPos = Position;
			foreach (char digit in xpText)
			{
				int digitValue = digit - '0';

				Rectangle sourceRect = new Rectangle(448 + (digitValue * 9), 447, 4, 9);
				spriteBatch.Draw(spriteSheet, drawPos, sourceRect, Color.White * alpha);
				drawPos.X += 5;
			}
		}
	}
}