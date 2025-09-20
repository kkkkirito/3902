using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project1.Commands;
using Project1.Controllers;
using Project1.Interfaces;
using SpritesDemo.Sprites;
using System;
using System.Collections.Generic;

namespace SpritesDemo
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Controllers (via interfaces)
        private readonly List<IController> _controllers = new();

        // Sprites
        private Texture2D _sheet;
        private ISprite _current;
        private ISprite _textSprite;

        // Frames cut from the sprite sheet
        private List<Rectangle> _idleFrames;
        private Rectangle _singleFrame;

        // Font
        private SpriteFont _font;

        // Cached credits text
        private string _creditsText;

        // Constants for slicing your sheet; tweak if your sheet differs
        private const int FRAME_W = 29;
        private const int IDLE_ROW = 52; // top row
        private const int IDLE_START_COL = 7; // start at left-most frame
        private const int IDLE_FRAME_COUNT = 4; // number of frames to animate

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 540;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load assets
            _sheet = Content.Load<Texture2D>("mario"); // Content/mario.png
            _font = Content.Load<SpriteFont>("DefaultSpriteFont");

            // Build a few frames from the sheet (simple grid slice)
            _idleFrames = new List<Rectangle>();
            for (int i = 0; i < IDLE_FRAME_COUNT; i++)
            {
                _idleFrames.Add(new Rectangle((IDLE_START_COL + i) * FRAME_W, IDLE_ROW, FRAME_W, 32));
            }
            _singleFrame = _idleFrames[0];

            // Credits text (three lines)
            _creditsText =
                "Credits\n" +
                "Program Made By: Haoyu Shi\n" +
                "Sprites from: Course Files";

            // place credits near bottom-left with padding
            const int pad = 16;
            Vector2 creditSize = _font.MeasureString(_creditsText);
            Vector2 creditPos = new Vector2(
                pad,
                _graphics.PreferredBackBufferHeight - creditSize.Y - pad
            );

            // Create the informational text sprite
            _textSprite = new TextSprite(
                _font,
                _creditsText,
                creditPos,
                Color.Black
            );

            // Commands
            var quitCmd = new QuitCommand(() => Exit());
            var setCmd = new Action<SpriteChoice>(ApplySpriteChoice);

            var setStatic = new SetSpriteCommand(setCmd, SpriteChoice.StaticStanding);
            var setAnimated = new SetSpriteCommand(setCmd, SpriteChoice.AnimatedIdle);
            var setFloat = new SetSpriteCommand(setCmd, SpriteChoice.FloatingUpDown);
            var setRun = new SetSpriteCommand(setCmd, SpriteChoice.RunningLeftRight);

            // Controllers
            _controllers.Add(new KeyboardInputController(quitCmd, setStatic, setAnimated, setFloat, setRun));
            _controllers.Add(new MouseInputController(_graphics, quitCmd, setStatic, setAnimated, setFloat, setRun));

            // Initial state = key 1 (static standing)
            ApplySpriteChoice(SpriteChoice.StaticStanding);
        }

        private void ApplySpriteChoice(SpriteChoice choice)
        {
            // Position sprites near center
            float scale = 4f;
            var start = new Vector2(
                _graphics.PreferredBackBufferWidth / 2f - (FRAME_W * scale) / 2f,
                _graphics.PreferredBackBufferHeight / 2f - (32 * scale) / 2f);

            switch (choice)
            {
                case SpriteChoice.StaticStanding:
                    _current = new StaticSprite(_sheet, _singleFrame, start, scale);
                    break;
                case SpriteChoice.AnimatedIdle:
                    _current = new AnimatedSprite(_sheet, _idleFrames, start, scale, 0.12);
                    break;
                case SpriteChoice.FloatingUpDown:
                    _current = new MovingStaticSprite(
                        _sheet,
                        _singleFrame,
                        start,
                        minY: 100,
                        maxY: _graphics.PreferredBackBufferHeight - 100,
                        pixelsPerSecond: 60f,
                        scale: scale);
                    break;
                case SpriteChoice.RunningLeftRight:
                    _current = new MovingAnimatedSprite(
                        _sheet,
                        _idleFrames,
                        new Vector2(0, start.Y),
                        _graphics.PreferredBackBufferWidth,
                        pixelsPerSecond: 120f,
                        scale: scale,
                        frameSeconds: 0.1);
                    break;
            }
        }

        public void SetCurrentSprite(ISprite sprite) => _current = sprite;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            foreach (var c in _controllers)
                c.Update(gameTime);

            _current?.Update(gameTime);
            _textSprite?.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _current?.Draw(_spriteBatch);
            _textSprite?.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}


