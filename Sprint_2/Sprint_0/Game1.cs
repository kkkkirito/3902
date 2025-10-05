using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Blocks;
using Sprint_0.Interfaces;

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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Viewport viewport = _graphics.GraphicsDevice.Viewport;

            spritePosition = new Vector2(viewport.Width / 2, viewport.Height / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            keyboardController = new KeyboardController(this);
            mouseController = new MouseController(this);

            blockTextures = Content.Load<Texture2D>("Clear_Zelda_2_Palace_Blocks 2");  
            itemTextures = Content.Load<Texture2D>("Clear_Zelda_2_Items 2");
            enemyTextures = Content.Load<Texture2D>("Clear_Zelda_2_Palace_Enemies 2");
            linkTextures = Content.Load<Texture2D>("Clear_LinkSprite 2");
            overworldEnemyTextures = Content.Load<Texture2D>("Zelda_2_Overworld_Enemies");




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

            blockFactory = new BlockFactory(Content);
            blockSelector = new BlockSelector(blockFactory);
            currentBlock = blockSelector.CreateCurrent(blockPosition);
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            keyboardController.Update();
            mouseController.Update();
            //drawnSprite.Update(gameTime);
            int blockSwitch = keyboardController.blockSwitch;
            if ((blockSwitch != 0))
            {
                if (blockSwitch > 0) blockSelector.Next();
                else blockSelector.Prev();
                currentBlock = blockSelector.CreateCurrent(blockPosition);
                keyboardController.blockSwitch = 0;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            string credits = "Credits\nProgram Made By: Team 4\nSprites from: \nhttps://www.mariouniverse.com/wp-content/img/sprites/nes/smb/characters.gif";

            _spriteBatch.Begin();

            //drawnSprite.Draw(_spriteBatch, spritePosition);

            Vector2 origin = font.MeasureString(credits) / 2;
            _spriteBatch.DrawString(font, credits, fontPosition, Color.Black, 0, origin, 1.0f, SpriteEffects.None, 0.5f);



            _spriteBatch.End();

            

            base.Draw(gameTime);
            currentBlock.Draw(_spriteBatch);
        }
//Should be in game1, the draw method above should call sprite factory with this logic.
        public void DrawSprite(int sprite)
        {
            Viewport viewport = _graphics.GraphicsDevice.Viewport;
            //Texture2D texture = Content.Load<Texture2D>("MarioSprites");

            

        }
    }
}
