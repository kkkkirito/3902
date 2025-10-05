using System;
using System.Collections.Generic;
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

namespace Sprint_0
{
    public class Game1 : Game
    {
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

        private IPlayer player;

        private BotEnemy botEnemy;
        private OverworldBotEnemy overworldBotEnemy;
        private OverworldManEnemy overworldManEnemy;
        private StalfosEnemy stalfosEnemy;
        private OctorokEnemy octorokEnemy;

        private IProjectileManager projectileManager;


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
            botEnemy = new BotEnemy(enemyTextures, new Vector2(500, 100));
            stalfosEnemy = new StalfosEnemy(enemyTextures, new Vector2(500, 0));
            octorokEnemy = new OctorokEnemy(overworldEnemyTextures, enemyTextures, new Vector2(500, 200));
            overworldBotEnemy = new OverworldBotEnemy(overworldEnemyTextures, new Vector2(500, 300));
            overworldManEnemy = new OverworldManEnemy(overworldEnemyTextures, new Vector2(500, 300));
            stalfosEnemy = new StalfosEnemy(enemyTextures, new Vector2(500, 0));

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

            

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            keyboardController.Update();
            mouseController.Update();
            player?.Update(gameTime);

            // Enemies
            botEnemy?.Update(gameTime);
            overworldBotEnemy?.Update(gameTime);
            overworldManEnemy?.Update(gameTime);
            stalfosEnemy?.Update(gameTime);
            octorokEnemy?.Update(gameTime);

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

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //string credits = "Credits\nProgram Made By: Team 4\nSprites from: \nhttps://www.mariouniverse.com/wp-content/img/sprites/nes/smb/characters.gif";

            _spriteBatch.Begin();

            //drawnSprite.Draw(_spriteBatch, spritePosition);

            player?.Draw(_spriteBatch);
            //Vector2 origin = font.MeasureString(credits) / 2;
            //_spriteBatch.DrawString(font, credits, fontPosition, Color.Black, 0, origin, 1.0f, SpriteEffects.None, 0.5f);

            // Enemies
            botEnemy.Draw(_spriteBatch);
            overworldBotEnemy.Draw(_spriteBatch);
            overworldManEnemy.Draw(_spriteBatch);
            stalfosEnemy.Draw(_spriteBatch);
            octorokEnemy.Draw(_spriteBatch);
            currentBlock?.Draw(_spriteBatch);
            _spriteBatch.Draw(blockFactory.Texture, blockPosition, blockFactory.GetSourceByIndex(blockSelector.Index), Color.White);

            //Items 
            itemSelector.Draw(_spriteBatch, new Vector2(100, 200));

            projectileManager?.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}