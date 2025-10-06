using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Blocks;
using Sprint_0.Command.BlocksCommand;
using Sprint_0.Command.PlayerCommand;
using Sprint_0.Commands.PlayerCommand;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;

using System;
using System.Collections.Generic;

using Sprint_0.States;
using Sprint_0.Command.GameCommand;


namespace Sprint_0
{
    public class Game1 : Game
    {
        // ===== Add state manager =====
        private GameStateManager stateManager;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IController keyboardController;
        private IController mouseController;
        private ISprite drawnSprite;

        private Vector2 spritePosition;
        private SpriteFont font;
        private Vector2 fontPosition;

        private BlockFactory blockFactory;
        private BlockSelector blockSelector;
        private IBlock currentBlock;
        private Vector2 blockPosition = new Vector2(240, 200);

        private Texture2D blockTextures;
        private Texture2D itemTextures;
        private Texture2D enemyTextures;
        private Texture2D linkTextures;
        private Texture2D overworldEnemyTextures;

        private ItemSelector itemSelector;
        private EnemySelector enemySelector;


        private IPlayer player;

        private BotEnemy botEnemy;
        private OverworldBotEnemy overworldBotEnemy;
        private OverworldManEnemy overworldManEnemy;
        private StalfosEnemy stalfosEnemy;
        private OctorokEnemy octorokEnemy;

        private IProjectileManager projectileManager;

        // ===== Add Q and R command handling =====
        private Dictionary<Keys, ICommand> gameCommands;
        private KeyboardState previousGameKeyState;

        public Game1()
        {
            Console.WriteLine("Game1");
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Viewport viewport = _graphics.GraphicsDevice.Viewport;

            Console.WriteLine(viewport.Width);

            spritePosition = new Vector2(viewport.Width / 2, viewport.Height / 2);

            // ===== Add: Initialize state manager =====
            stateManager = new GameStateManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            keyboardController = new KeyboardController(this);
            mouseController = new MouseController(this);
            blockFactory = new BlockFactory(Content);
            blockSelector = new BlockSelector(blockFactory);
            currentBlock = blockSelector.CreateCurrent(blockPosition);

            blockTextures = Content.Load<Texture2D>("Clear_Zelda_2_Palace_Blocks 2");
            itemTextures = Content.Load<Texture2D>("Clear_Zelda_2_Items 2");
            enemyTextures = Content.Load<Texture2D>("Clear_Zelda_2_Palace_Enemies 2");
            linkTextures = Content.Load<Texture2D>("Clear_LinkSprite 2");
            overworldEnemyTextures = Content.Load<Texture2D>("Zelda_2_Overworld_Enemies");

            player = new Player(linkTextures, new Vector2(100, 100), keyboardController);

            projectileManager = new ProjectileManager(linkTextures);





            ((KeyboardController)keyboardController).BindHold(Keys.W, new MoveUpCommand(player));
            ((KeyboardController)keyboardController).BindHold(Keys.S, new MoveDownOrCrouchOnCommand(player));
            ((KeyboardController)keyboardController).BindRelease(Keys.S, new CrouchOffCommand(player));
            ((KeyboardController)keyboardController).BindHold(Keys.A, new MoveLeftCommand(player));
            ((KeyboardController)keyboardController).BindHold(Keys.D, new MoveRightCommand(player));
            ((KeyboardController)keyboardController).Press(Keys.E, new DamagePlayerCommand(player));
            ((KeyboardController)keyboardController).Press(Keys.Z, new AttackCommand(player));
            ((KeyboardController)keyboardController).Press(Keys.N, new AttackCommand(player));
            ((KeyboardController)keyboardController).Press(Keys.Space, new JumpCommand(player));
            ((KeyboardController)keyboardController).Press(Keys.Tab, new ToggleGameModeCommand(player));
            ((KeyboardController)keyboardController).Press(Keys.P, new PickupCommand(player));
            ((KeyboardController)keyboardController).Press(Keys.T, new PreviousBlockCommand(blockSelector));
            ((KeyboardController)keyboardController).Press(Keys.Y, new NextBlockCommand(blockSelector));
            ((KeyboardController)keyboardController).Press(Keys.X, new SwordBeamCommand(player, projectileManager));
            ((KeyboardController)keyboardController).Press(Keys.C, new FireballCommand(player, projectileManager));








            // Enemy initialization
            enemySelector = new EnemySelector(enemyTextures, overworldEnemyTextures);

            // Items
            var itemAnimations = SpriteFactory.CreateItemAnimations(itemTextures);

            itemSelector = new ItemSelector(itemAnimations);

            var blocks = new List<Rectangle>
            {
                new Rectangle(0, 20, 17, 17),  //Parapa Palace Exterior
                new Rectangle(17, 20, 17, 17),
                new Rectangle(34, 20, 17, 17),
                new Rectangle(51, 20, 17, 17),
                new Rectangle(68, 20, 17, 17),
                new Rectangle(85, 20, 17, 17),
                new Rectangle(102, 20, 17, 17),
                new Rectangle(119, 20, 17, 17),
                new Rectangle(136, 20, 17, 17),
                new Rectangle(153, 20, 17, 17),
                new Rectangle(170, 20, 17, 17)
            };

            var items = new List<Rectangle>
            {
                new Rectangle(0, 10, 9, 17),  //General Collectables
                new Rectangle(11, 10, 9, 17),
                new Rectangle(20, 10, 9, 17),
                new Rectangle(29, 10, 9, 17),
                new Rectangle(40, 10, 9, 17),
                new Rectangle(49, 10, 9, 17),
                new Rectangle(58, 10, 9, 17),
                new Rectangle(69, 10, 9, 17),
                new Rectangle(89, 10, 9, 17),
                new Rectangle(100, 10, 9, 17),

                new Rectangle(128, 10, 9, 17),  //Major Collectibles
                new Rectangle(137, 10, 9, 17),
                new Rectangle(146, 10, 9, 17),
                new Rectangle(155, 10, 9, 17),
                new Rectangle(164, 10, 9, 17),
                new Rectangle(173, 10, 9, 17),
                new Rectangle(182, 10, 9, 17),
                new Rectangle(191, 10, 9, 17),
                new Rectangle(202, 10, 17, 17),
                new Rectangle(219, 10, 17, 17),
                new Rectangle(238, 10, 17, 17),
                new Rectangle(255, 10, 17, 17),
                new Rectangle(272, 10, 17, 17)
            };

            Rectangle standing = new Rectangle(276, 43, 13, 17);

            Viewport viewport = _graphics.GraphicsDevice.Viewport;
            font = Content.Load<SpriteFont>("gameFont");
            fontPosition = new Vector2(viewport.Width / 2, (viewport.Height / 2) + 50);




            // ===== Add: Setup state manager =====
            SetupStateManager();


            // ===== Add: Initialize Q and R commands =====
            gameCommands = new Dictionary<Keys, ICommand>();
            gameCommands[Keys.Q] = new QuitCommand(this);
            gameCommands[Keys.R] = new ResetCommand(new GameplayResetHandler(this, player, blockSelector, blockPosition));
        }

        // ===== Add new method: Setup state manager =====
        private void SetupStateManager()
        {
            // Create menu state
            var menuState = new MenuState(this, font, stateManager);

            // Create gameplay state (use wrapper to delegate to Game1's existing logic)
            var gameplayState = new GameplayStateDelegate(this);

            // Add states
            stateManager.AddState("menu", menuState);
            stateManager.AddState("gameplay", gameplayState);

            // Start with menu
            stateManager.ChangeState("menu");
        }

        // ===== Add new method: Game logic update (called by GameplayStateDelegate) =====
        public void UpdateGameplay(GameTime gameTime)
        {
            // Handle Q and R keys
            KeyboardState currentKeyState = Keyboard.GetState();
            foreach (var kvp in gameCommands)
            {
                if (currentKeyState.IsKeyDown(kvp.Key) && !previousGameKeyState.IsKeyDown(kvp.Key))
                {
                    kvp.Value.Execute();
                }
            }
            previousGameKeyState = currentKeyState;

            keyboardController.Update();
            mouseController.Update();
            player?.Update(gameTime);

            // Enemies
            enemySelector.Update(gameTime);

            // Items
            itemSelector.Update(gameTime);

            int blockSwitch = keyboardController.blockSwitch;
            if ((blockSwitch != 0))
            {
                if (blockSwitch > 0) blockSelector.Next();
                else blockSelector.Prev();
                currentBlock = blockSelector.CreateCurrent(blockPosition);
                keyboardController.blockSwitch = 0;
            }
            projectileManager?.Update(gameTime);
        }

        // ===== Add new method: Game drawing (called by GameplayStateDelegate) =====
        public void DrawGameplay(SpriteBatch spriteBatch)
        {
            player?.Draw(spriteBatch);
            //Vector2 origin = font.MeasureString(credits) / 2;
            //_spriteBatch.DrawString(font, credits, fontPosition, Color.Black, 0, origin, 1.0f, SpriteEffects.None, 0.5f);

            // Enemies
            enemySelector.Draw(spriteBatch);
            spriteBatch.Draw(blockFactory.Texture, blockPosition, blockFactory.GetSourceByIndex(blockSelector.Index), Color.White);

            //Items 
            itemSelector.Draw(spriteBatch, new Vector2(100, 200));

            projectileManager?.Draw(spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            stateManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //string credits = "Credits\nProgram Made By: Team 4\nSprites from: \nhttps://www.mariouniverse.com/wp-content/img/sprites/nes/smb/characters.gif";

            stateManager.Draw(_spriteBatch);

            base.Draw(gameTime);
        }

        // ===== Add reset method =====
        public void ResetGame()
        {
            // Reset player
            if (player != null)
            {
                player.CurrentHealth = player.MaxHealth;
                player.Position = new Vector2(100, 100);
                player.CurrentState = new Sprint_0.States.LinkStates.IdleState();
            }

            // Reset block
            currentBlock = blockSelector.CreateCurrent(blockPosition);

            // Reset enemies
            enemySelector = new EnemySelector(enemyTextures, overworldEnemyTextures);

            //Reset items
            itemSelector = new ItemSelector(SpriteFactory.CreateItemAnimations(itemTextures));
        }
    }

    // ===== Add new class: Gameplay state delegate =====
    public class GameplayStateDelegate : IGameState
    {
        private Game1 game;

        public GameplayStateDelegate(Game1 game)
        {
            this.game = game;
        }

        public void Enter() { }
        public void Exit() { }

        public void Update(GameTime gameTime)
        {
            game.UpdateGameplay(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            game.DrawGameplay(spriteBatch);
            spriteBatch.End();
        }

        public void Reset()
        {
            game.ResetGame();
        }
    }

    // ===== Add new class: Reset handler =====
    public class GameplayResetHandler : IGameState
    {
        private Game1 game;
        private IPlayer player;
        private BlockSelector blockSelector;
        private Vector2 blockPosition;

        public GameplayResetHandler(Game1 game, IPlayer player, BlockSelector blockSelector, Vector2 blockPosition)
        {
            this.game = game;
            this.player = player;
            this.blockSelector = blockSelector;
            this.blockPosition = blockPosition;
        }

        public void Enter() { }
        public void Exit() { }
        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch) { }

        public void Reset()
        {
            game.ResetGame();
        }
    }
}