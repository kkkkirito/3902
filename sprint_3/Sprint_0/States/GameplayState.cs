using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Blocks;
using Sprint_0.Command.BlocksCommand;
using Sprint_0.Command.GameCommand;
using Sprint_0.Command.PlayerCommand;
using Sprint_0.Commands.PlayerCommand;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;
using Sprint_0.States.LinkStates;
using Sprint_0.Systems;
using System;
using System.Collections.Generic;

namespace Sprint_0.States
{
    public class GameplayState : IGameState
    {
        private Game1 game;
        private ContentManager content;

        // Controllers
        private IController keyboardController;
        private IController mouseController;
        private IController gamepadController;

        // Player
        private IPlayer player;

        // Blocks
        private BlockFactory blockFactory;
        private BlockSelector blockSelector;
        private IBlock currentBlock;
        private Vector2 blockPosition = new Vector2(240, 200);

        // Enemies
        private EnemySelector enemySelector;

        // Items
        private ItemSelector itemSelector;

        // Projectiles
        private IProjectileManager projectileManager;

        // Hotbar
        private IHotbar hotbar;

        // Commands
        private Dictionary<Keys, ICommand> commandBindings;
        private KeyboardState previousKeyboardState;

        public GameplayState(Game1 game, ContentManager content)
        {
            this.game = game;
            this.content = content;
        }

        public void Enter()
        {
           
                InitializeGameObjects();
                InitializeCommands();
           
        }

        private void InitializeGameObjects()
        {
            // Create controllers
            keyboardController = new KeyboardController(game);
            mouseController = new MouseController(game);
            gamepadController = new GamepadController(PlayerIndex.One);
            GamepadController.ControlPlayer = player;

            // Create player
            player = new Player(game.LinkTextures, new Vector2(100, 100), keyboardController);

            // Create block system
            blockFactory = new BlockFactory(content);
            blockSelector = new BlockSelector(blockFactory);
            currentBlock = blockSelector.CreateCurrent(blockPosition);

            // Create enemy selector
            enemySelector = new EnemySelector(game.EnemyTextures, game.OverworldEnemyTextures);

            // Create item selector
            var itemAnimations = SpriteFactory.CreateItemAnimations(game.ItemTextures);
            itemSelector = new ItemSelector(itemAnimations);

            // Create projectile manager
            projectileManager = new ProjectileManager(game.LinkTextures);

            // Create hotbar
            hotbar = new Hotbar(3);
        }

        private void InitializeCommands()
        {
            var kb = (KeyboardController)keyboardController;

            // Movement commands (hold)
            kb.BindHold(Keys.W, new MoveUpCommand(player));
            kb.BindHold(Keys.Up, new MoveUpCommand(player));
            kb.BindHold(Keys.S, new MoveDownOrCrouchOnCommand(player));
            kb.BindHold(Keys.Down, new MoveDownOrCrouchOnCommand(player));
            kb.BindHold(Keys.A, new MoveLeftCommand(player));
            kb.BindHold(Keys.Left, new MoveLeftCommand(player));
            kb.BindHold(Keys.D, new MoveRightCommand(player));
            kb.BindHold(Keys.Right, new MoveRightCommand(player));

            // Release commands
            kb.BindRelease(Keys.S, new CrouchOffCommand(player));
            kb.BindRelease(Keys.Down, new CrouchOffCommand(player));

            // Action commands (press)
            kb.Press(Keys.Z, new AttackCommand(player));
            kb.Press(Keys.N, new AttackCommand(player));
            kb.Press(Keys.E, new DamagePlayerCommand(player));
            kb.Press(Keys.Space, new JumpCommand(player));
            kb.Press(Keys.Tab, new ToggleGameModeCommand(player));
            kb.Press(Keys.P, new PickupCommand(player));
            kb.Press(Keys.X, new SwordBeamCommand(player, projectileManager));
            kb.Press(Keys.C, new FireballCommand(player, projectileManager));

            // Block commands
            kb.Press(Keys.T, new PreviousBlockCommand(blockSelector));
            kb.Press(Keys.Y, new NextBlockCommand(blockSelector));

            // Hotbar commands
            for (int i = 1; i <= 3; i++)
            {
                int slot = i - 1;
                kb.Press((Keys)((int)Keys.D0 + i), new Commands.UseSlotCommand(hotbar, player, slot));
            }

            // Game commands
            kb.Press(Keys.Q, new QuitCommand(game));
            kb.Press(Keys.R, new ResetCommand(this));

            // O and P are handled by EnemySelector
            // U and I are handled by ItemSelector

            //Gamepad command
            var pad = (GamepadController)gamepadController;

            pad.BindHold(Buttons.DPadUp, new MoveUpCommand(player));
            pad.BindHold(Buttons.DPadDown, new MoveDownOrCrouchOnCommand(player));
            pad.BindHold(Buttons.DPadLeft, new MoveLeftCommand(player));
            pad.BindHold(Buttons.DPadRight, new MoveRightCommand(player));
            pad.Press(Buttons.X, new AttackCommand(player));
            pad.Press(Buttons.A, new JumpCommand(player));

            pad.BindRelease(Buttons.DPadDown, new CrouchOffCommand(player));

            pad.BindJoystick(player);

            gamepadController = pad;
        }

        public void Exit()
        {
            // Cleanup if needed
        }

        public void Update(GameTime gameTime)
        {
            // Update controllers
            keyboardController.Update();
            mouseController.Update();
            gamepadController.Update();

            // Update game objects
            player?.Update(gameTime);
            currentBlock?.Update(gameTime);
            enemySelector?.Update(gameTime);
            itemSelector?.Update(gameTime);
            projectileManager?.Update(gameTime);

            // Handle block switching (legacy support for controller.blockSwitch)
            int blockSwitch = keyboardController.blockSwitch;
            if (blockSwitch != 0)
            {
                if (blockSwitch > 0)
                    blockSelector.Next();
                else
                    blockSelector.Prev();

                currentBlock = blockSelector.CreateCurrent(blockPosition);
                keyboardController.blockSwitch = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw all game objects
            player?.Draw(spriteBatch);
            currentBlock?.Draw(spriteBatch);
            enemySelector?.Draw(spriteBatch);
            itemSelector?.Draw(spriteBatch, new Vector2(100, 200));
            projectileManager?.Draw(spriteBatch);

            // Draw block preview
            spriteBatch.Draw(
                blockFactory.Texture,
                blockPosition,
                blockFactory.GetSourceByIndex(blockSelector.Index),
                Color.White
            );

            spriteBatch.End();
        }

        public void Reset()
        {
            // Reset player
            if (player != null)
            {
                player.CurrentHealth = player.MaxHealth;
                player.Position = new Vector2(100, 100);
                player.CurrentState = new IdleState();
            }

            // Reset block
            blockSelector = new BlockSelector(blockFactory);
            currentBlock = blockSelector.CreateCurrent(blockPosition);

            // Reinitialize enemies and items
            enemySelector = new EnemySelector(game.EnemyTextures, game.OverworldEnemyTextures);
            itemSelector = new ItemSelector(SpriteFactory.CreateItemAnimations(game.ItemTextures));

            // Reset projectiles
            projectileManager = new ProjectileManager(game.LinkTextures);


            var kb = (KeyboardController)keyboardController;
            kb.Press(Keys.X, new SwordBeamCommand(player, projectileManager));
            kb.Press(Keys.C, new FireballCommand(player, projectileManager));
            kb.Press(Keys.T, new PreviousBlockCommand(blockSelector));
            kb.Press(Keys.Y, new NextBlockCommand(blockSelector));
  
        }
    }
}