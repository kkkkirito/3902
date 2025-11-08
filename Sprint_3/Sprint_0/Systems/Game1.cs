using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.States;
using Sprint_0.States.Gameplay;

namespace Sprint_0
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameStateManager stateManager;


        // Resources
        public SpriteFont Font { get; private set; }
        public Texture2D LinkTextures { get; private set; }
        public Texture2D BlockTextures { get; private set; }
        public Texture2D ItemTextures { get; private set; }
        public Texture2D EnemyTextures { get; private set; }
        public Texture2D OverworldEnemyTextures { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            stateManager = new GameStateManager();
            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load all textures
            Font = Content.Load<SpriteFont>("gameFont");
            LinkTextures = Content.Load<Texture2D>("Clear_LinkSprite 2");
            BlockTextures = Content.Load<Texture2D>("Clear_Zelda_2_Palace_Blocks 2");
            ItemTextures = Content.Load<Texture2D>("Clear_Zelda_2_Items 2");
            EnemyTextures = Content.Load<Texture2D>("Clear_Zelda_2_Palace_Enemies 2");
            OverworldEnemyTextures = Content.Load<Texture2D>("Zelda_2_Overworld_Enemies");
            SetupStateManager();
        }

        private void SetupStateManager()
        {
            // Create states
            var menuState = new MenuState(this, Font, stateManager);
            var gameplayState = new GameplayState(this);

            // Add states
            stateManager.AddState("menu", menuState);
            stateManager.AddState("gameplay", gameplayState);

            // Start with menu
            stateManager.ChangeState("menu");
        }

        protected override void Update(GameTime gameTime)
        {
            stateManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            stateManager.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}