//Dillon Brigode AU25, part of gameplaystate refactor
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Commands.GameCommands;
using Sprint_0.Commands.PlayerCommands;
using Sprint_0.Interfaces;
using Sprint_0.Managers;
using Sprint_0.Systems;
namespace Sprint_0.States.Gameplay
{
    public sealed class InputBinder(IController keyboard, IController gamepad, GameplayState gameplay)
    {
        private readonly IController _keyboard = keyboard;
        private readonly IController _gamepad = gamepad;
        private readonly GameplayState _gameplay = gameplay;

        public void BindFor(IPlayer player, IProjectileManager projectiles, IHotbar hotbar, Game1 game, IAudioManager audio)
        {
                var kb = (KeyboardController)_keyboard;
                var pad = (GamepadController)_gamepad;

                // Movement (hold)
                kb.BindHold(Keys.W, new MoveUpCommand(player));
                kb.BindHold(Keys.S, new MoveDownOrCrouchOnCommand(player));
                kb.BindHold(Keys.A, new MoveLeftCommand(player));
                kb.BindHold(Keys.D, new MoveRightCommand(player));
                kb.BindHold(Keys.Up, new MoveUpCommand(player));
                kb.BindHold(Keys.Down, new MoveDownOrCrouchOnCommand(player));
                kb.BindHold(Keys.Left, new MoveLeftCommand(player));
                kb.BindHold(Keys.Right, new MoveRightCommand(player));
                kb.BindRelease(Keys.S, new CrouchOffCommand(player));
                kb.BindRelease(Keys.Down, new CrouchOffCommand(player));

                // Actions
                kb.Press(Keys.Z, new AttackCommand(player, audio));
                kb.Press(Keys.N, new AttackCommand(player, audio));
                kb.Press(Keys.Space, new JumpCommand(player, audio));
                kb.Press(Keys.Tab, new ToggleGameModeCommand(player));
                kb.Press(Keys.X, new SwordBeamCommand(player, projectiles, audio));
                kb.Press(Keys.C, new FireballCommand(player, projectiles, audio));

                // Hotbar 1–3
                for (int i = 1; i <= hotbar.SlotCount; i++)
                    kb.Press((Keys)((int)Keys.D0 + i), new UseSlotCommand(hotbar, player, i - 1));

                // Game commands
                kb.Press(Keys.Q, new QuitCommand(game));
                kb.Press(Keys.R, new ResetCommand(_gameplay));
                //DELETE ONCE ITEM PICKUP LOGIC IS IMPLEMENTED. USING THIS FOR TESTING PURPOSES
                kb.Press(Keys.V, new VictoryCommand(game));

                // Gamepad
                pad.BindHold(Buttons.DPadUp, new MoveUpCommand(player));
                pad.BindHold(Buttons.DPadDown, new MoveDownOrCrouchOnCommand(player));
                pad.BindHold(Buttons.DPadLeft, new MoveLeftCommand(player));
                pad.BindHold(Buttons.DPadRight, new MoveRightCommand(player));
                pad.Press(Buttons.X, new AttackCommand(player, audio));
                pad.Press(Buttons.A, new JumpCommand(player, audio));
                pad.BindRelease(Buttons.DPadDown, new CrouchOffCommand(player));
                pad.Press(Buttons.Y, new SwordBeamCommand(player, projectiles, audio));
                pad.Press(Buttons.B, new FireballCommand(player, projectiles, audio));
                pad.BindJoystick(player);

                //Sound
                kb.Press(Keys.M, new ToggleBgmCommand(audio));
                pad.Press(Buttons.LeftShoulder, new ToggleBgmCommand(audio));
        }
        public sealed class InputSelector
        {
            private KeyboardState _prevKbd = Keyboard.GetState();
            private GamePadState _prevPad = GamePad.GetState(PlayerIndex.One);

            public InputSelector() { }

            // Call each frame from GameplayState.Update before game logic runs.
            public void Update()
            {
                var k = Keyboard.GetState();
                var pad = GamePad.GetState(PlayerIndex.One);

                bool pausePressed =
                    (k.IsKeyDown(Keys.Escape) && !_prevKbd.IsKeyDown(Keys.Escape)) ||
                    (k.IsKeyDown(Keys.P) && !_prevKbd.IsKeyDown(Keys.P)) ||
                    (pad.IsButtonDown(Buttons.Start) && !_prevPad.IsButtonDown(Buttons.Start));

                if (pausePressed)
                {
                    PauseState.Toggle();
                }

                _prevKbd = k;
                _prevPad = pad;
            }
        }
    }
}
