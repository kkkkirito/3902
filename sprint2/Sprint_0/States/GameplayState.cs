using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Blocks;
using Sprint_0.Command.BlocksCommand;
using Sprint_0.Command.GameCommand;
using Sprint_0.Command.PlayerCommand;
using Sprint_0.Interfaces;
using Sprint_0.States.LinkStates;
using System.Collections.Generic;

namespace Sprint_0.States
{
    public class GameplayState : IGameState
    {
        private Game1 game;
        private IPlayer player;
        private BlockSelector blockSelector;
        private IBlock currentBlock;
        private Dictionary<Keys, ICommand> keyboardCommands;
        private KeyboardState previousKeyboardState;

        public GameplayState(Game1 game, IPlayer player, BlockSelector blockSelector)
        {
            this.game = game;
            this.player = player;
            this.blockSelector = blockSelector;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            keyboardCommands = new Dictionary<Keys, ICommand>();

            // Movement commands
            keyboardCommands[Keys.Up] = new MoveUpCommand(player);
            keyboardCommands[Keys.W] = new MoveUpCommand(player);
            keyboardCommands[Keys.Down] = new MoveDownCommand(player);
            keyboardCommands[Keys.S] = new MoveDownCommand(player);
            keyboardCommands[Keys.Left] = new MoveLeftCommand(player);
            keyboardCommands[Keys.A] = new MoveLeftCommand(player);
            keyboardCommands[Keys.Right] = new MoveRightCommand(player);
            keyboardCommands[Keys.D] = new MoveRightCommand(player);

            // Attack commands
            keyboardCommands[Keys.Z] = new AttackCommand(player);
            keyboardCommands[Keys.N] = new AttackCommand(player);

            // Damage command
            keyboardCommands[Keys.E] = new DamagePlayerCommand(player);

            // Block cycling
            keyboardCommands[Keys.T] = new PreviousBlockCommand(blockSelector);
            keyboardCommands[Keys.Y] = new NextBlockCommand(blockSelector);

            // Item slot commands
            for (int i = 1; i <= 9; i++)
            {
                keyboardCommands[(Keys)((int)Keys.D0 + i)] = new UseItemCommand(player, i);
            }

            // Game commands
            keyboardCommands[Keys.Q] = new QuitCommand(game);
            keyboardCommands[Keys.R] = new ResetCommand(this);
        }

        public void Enter()
        {
            // Initialize gameplay state
            currentBlock = blockSelector.CreateCurrent(new Vector2(240, 200));
        }

        public void Exit()
        {
            // Clean up gameplay state
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            // Process keyboard commands
            foreach (var kvp in keyboardCommands)
            {
                if (currentKeyboardState.IsKeyDown(kvp.Key) &&
                    !previousKeyboardState.IsKeyDown(kvp.Key))
                {
                    kvp.Value.Execute();
                }
            }

            // Update game objects
            player?.Update(gameTime);
            currentBlock?.Update(gameTime);

            previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw game objects
            player?.Draw(spriteBatch);
            currentBlock?.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Reset()
        {
            // Reset player state
            if (player != null)
            {
                player.CurrentHealth = player.MaxHealth;
                player.Position = new Vector2(400, 240); // Starting position
                player.CurrentState = new IdleState();
            }

            // Reset block
            currentBlock = blockSelector.CreateCurrent(new Vector2(240, 200));
        }
    }
}
