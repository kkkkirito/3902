using Microsoft.Xna.Framework.Input;


namespace Sprint_0
{
    public class KeyboardController : IController
    {

        private Game1 game;
        private Player player; // Add player reference
        public int blockSwitch { get; set; } = 0;

        public KeyboardController(Game1 game, Player player = null)
        {

            this.game = game;
            this.player = player; // Store player reference

        }
        public void Update() {

            KeyboardState state = Keyboard.GetState();
            if(state.IsKeyDown(Keys.T))
            {
                blockSwitch = -1;
            }
            else if (state.IsKeyDown(Keys.Y))
            {
                blockSwitch = +1;
            }
            if (state.IsKeyDown(Keys.NumPad0) || state.IsKeyDown(Keys.D0))
            {

                game.DrawSprite(0);

            }

            else if (state.IsKeyDown(Keys.NumPad1) || state.IsKeyDown(Keys.D1))
            {

                game.DrawSprite(1);

            }

            else if (state.IsKeyDown(Keys.NumPad2) || state.IsKeyDown(Keys.D2))
            {

                game.DrawSprite(2);

            }

            else if (state.IsKeyDown(Keys.NumPad3) || state.IsKeyDown(Keys.D3))
            {

                game.DrawSprite(3);

            }

            else if (state.IsKeyDown(Keys.NumPad4) || state.IsKeyDown(Keys.D4))
            {

                game.DrawSprite(4);

            }else if (state.IsKeyDown(Keys.E) && player != null)
            {
                player.TakeDamage(10); // Simulate 10 damage
            }
        }
    }

}
