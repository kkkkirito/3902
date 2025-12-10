using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Sprint_0.Enemies
{
    public class EnemySelector
    {

        private KeyboardState prevKeyboard;
        private int currentIndex;
        private BotEnemy botEnemy;
        private StalfosEnemy stalfosEnemy;
        private OctorokEnemy octorokEnemy;
        private TopDownBotEnemy TopDownBotEnemy;
        private TopDownManEnemy TopDownManEnemy;

        internal EnemySelector(Texture2D enemyTextures, Texture2D overworldEnemyTextures)
        {
            this.currentIndex = 0;
            this.prevKeyboard = Keyboard.GetState();
            this.botEnemy = new BotEnemy(enemyTextures, new Vector2(500, 100));
            this.stalfosEnemy = new StalfosEnemy(enemyTextures, new Vector2(500, 0));
            this.octorokEnemy = new OctorokEnemy(overworldEnemyTextures, enemyTextures, new Vector2(500, 200));
            this.TopDownBotEnemy = new TopDownBotEnemy(overworldEnemyTextures, new Vector2(500, 300));
            this.TopDownManEnemy = new TopDownManEnemy(overworldEnemyTextures, new Vector2(500, 400));

        }

        public void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();


            if (kb.IsKeyDown(Keys.O) && prevKeyboard.IsKeyUp(Keys.O))
            {
                currentIndex--;
                if (currentIndex < 0)
                    currentIndex = 4;
            }


            if (kb.IsKeyDown(Keys.P) && prevKeyboard.IsKeyUp(Keys.P))
            {
                currentIndex++;
                if (currentIndex > 4)
                    currentIndex = 0;
            }

            switch (currentIndex)
            {
                case 0:
                    botEnemy.Update(gameTime);
                    break;
                case 1:
                    stalfosEnemy.Update(gameTime);
                    break;
                case 2:
                    octorokEnemy.Update(gameTime);
                    break;
                case 3:
                    TopDownBotEnemy.Update(gameTime);
                    break;
                case 4:
                    TopDownManEnemy.Update(gameTime);
                    break;
            }

            prevKeyboard = kb;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (currentIndex)
            {
                case 0:
                    botEnemy.Draw(spriteBatch);
                    break;
                case 1:
                    stalfosEnemy.Draw(spriteBatch);
                    break;
                case 2:
                    octorokEnemy.Draw(spriteBatch);
                    break;
                case 3:
                    TopDownBotEnemy.Draw(spriteBatch);
                    break;
                case 4:
                    TopDownManEnemy.Draw(spriteBatch);
                    break;
            }
        }

    }
}