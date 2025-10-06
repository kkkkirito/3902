using Microsoft.Xna.Framework.Input;
namespace Sprint_0.States.LinkStates
{
    public class InputState
    {
        public bool MoveUp { get; set; }
        public bool MoveDown { get; set; }
        public bool MoveLeft { get; set; }
        public bool MoveRight { get; set; }
        public bool Attack { get; set; }
        public bool UseItem { get; set; }
        public int ItemSlot { get; set; }

        // Instance method that returns a new InputState
        public InputState GetFromController(IController controller)
        {
            var state = new InputState();
            var keyboardState = Keyboard.GetState();

            // Movement
            state.MoveUp = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W);
            state.MoveDown = keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S);
            state.MoveLeft = keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A);
            state.MoveRight = keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D);

            // Attack
            state.Attack = keyboardState.IsKeyDown(Keys.Z) || keyboardState.IsKeyDown(Keys.N);

            // Items
            for (int i = 1; i <= 9; i++)
            {
                if (keyboardState.IsKeyDown((Keys)((int)Keys.D0 + i)))
                {
                    state.UseItem = true;
                    state.ItemSlot = i;
                    break;
                }
            }

            return state;
        }
    }
}