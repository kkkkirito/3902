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

            Texture2D texture = Content.Load<Texture2D>("MarioSprites");
            Rectangle standing = new Rectangle(276, 43, 13, 17);

            drawnSprite = new StaticStillSprite(texture, standing);

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
            drawnSprite.Update(gameTime);
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
            string credits = "Credits\nProgram Made By: Tyler Carpenter\nSprites from: \nhttps://www.mariouniverse.com/wp-content/img/sprites/nes/smb/characters.gif";

            _spriteBatch.Begin();

            drawnSprite.Draw(_spriteBatch, spritePosition);

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
            Texture2D texture = Content.Load<Texture2D>("MarioSprites");

            switch (sprite)
            {

                case 0:

                    Exit();
                    break;

                case 1:

                    Rectangle standing = new Rectangle(276, 43, 13, 17);

                    drawnSprite = new StaticStillSprite(texture, standing);

                    break;

                case 2:

                    Rectangle[] animated = new Rectangle[] { new Rectangle(291, 44, 13, 16),
                                                             new Rectangle(306, 43, 12, 17),
                                                             new Rectangle(320, 43, 16, 17) };

                    drawnSprite = new StaticAnimatedSprite(texture, animated);

                    break;

                case 3:

                    Rectangle standingReversed = new Rectangle(223, 43, 13, 17);
                    Vector2 position3 = new Vector2(viewport.Width / 2, viewport.Height / 2);
                    Vector2 speed = new Vector2(0, 1);

                    drawnSprite = new MovingStillSprite(texture, standingReversed, position3, speed);

                    break;

                case 4:

                    Rectangle[] animatedMoving = new Rectangle[] { new Rectangle(291, 44, 13, 16),
                                                             new Rectangle(306, 43, 12, 17),
                                                             new Rectangle(320, 43, 16, 17) };
                    Vector2 position4 = new Vector2(viewport.Width / 2, viewport.Height / 2);
                    Vector2 speed4 = new Vector2(1, 0);

                    drawnSprite = new MovingAnimatedSprite(texture, animatedMoving, position4, speed4);

                    break;



            };

        }
    }
}
