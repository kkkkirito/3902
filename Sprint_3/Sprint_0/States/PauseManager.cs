using Microsoft.Xna.Framework.Input;

namespace Sprint_0.States
{
    public static class PauseManager
    {
        private static KeyboardState previousState;
        public static bool IsPaused { get; private set; }

        public static void Update()
        {
            var state = Keyboard.GetState();

            if (previousState.IsKeyUp(Keys.P) && state.IsKeyDown(Keys.P))
            {
                IsPaused = !IsPaused;
            }

            previousState = state;
        }

        public static void SetPaused(bool paused) => IsPaused = paused;
        public static void Toggle() => IsPaused = !IsPaused;
    }
}