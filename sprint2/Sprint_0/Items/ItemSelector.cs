using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sprint_0
{
	public class ItemSelector
	{
		private Dictionary<string, Animation> items;
		private List<string> itemKeys;
		private int currentIndex;
		private KeyboardState prevKeyboard;

		internal ItemSelector(Dictionary<string, Animation> items)
		{
			this.items = items;
			this.itemKeys = new List<string>(items.Keys);
			this.currentIndex = 0;
			this.prevKeyboard = Keyboard.GetState();
		}

		public void Update(GameTime gameTime)
		{
			var kb = Keyboard.GetState();

			// Go to previous item
			if (kb.IsKeyDown(Keys.U) && prevKeyboard.IsKeyUp(Keys.U))
			{
				currentIndex--;
				if (currentIndex < 0)
					currentIndex = itemKeys.Count - 1;
			}

			// Go to next item
			if (kb.IsKeyDown(Keys.I) && prevKeyboard.IsKeyUp(Keys.I))
			{
				currentIndex++;
				if (currentIndex >= itemKeys.Count)
					currentIndex = 0;
			}

			// Update the animation of the current item (if animated)
			items[itemKeys[currentIndex]].Update(gameTime);

			prevKeyboard = kb;
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 position)
		{
			var anim = items[itemKeys[currentIndex]];
			anim.Draw(spriteBatch, position, SpriteEffects.None);
		}

		public string CurrentItemName => itemKeys[currentIndex];
	}
}