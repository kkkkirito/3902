using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Collision_System;
using Sprint_0.Interfaces;
using Sprint_0.Rooms;
using Sprint_0.Systems;
using System;


namespace Sprint_0.States.Gameplay
{
    public class GameplayState : IGameState
    {
        private readonly Game1 _game;
        private readonly IController _keyboard;
        private readonly IController _mouse;
        private readonly IController _gamepad;
        private readonly InputBinder _inputBinder;
        private readonly CollisionCoordinator _collisions;
        private HudRenderer _hud;
        private readonly RoomEntityManager _entityManager;
        private readonly RoomTransitionManager _transitionManager;
        private RoomNavigator _navigator;
        private IPlayer _player;
        private IProjectileManager _projectiles;
        private IHotbar _hotbar;
        private MouseState _prevMouse;
        private Camera _camera;


        private bool _isTransitioning = false;
        private float _transitionTimer = 0f;
        private const float TRANSITION_DURATION = 0.5f;
        private RoomTransition _pendingTransition;

        public GameplayState(Game1 game)
        {
            _game = game;
            _keyboard = new KeyboardController(_game);
            _mouse = new MouseController(_game);
            _gamepad = new GamepadController(PlayerIndex.One);
            _inputBinder = new InputBinder(_keyboard, _gamepad, this);
            var collisionSystem = new CollisionSystem();
            CollisionRegistry.Register(collisionSystem);
            _collisions = new CollisionCoordinator(collisionSystem);


            _camera = new Camera(_game.GraphicsDevice.Viewport);
            _transitionManager = new RoomTransitionManager();

            _entityManager = new RoomEntityManager(_game.LinkTextures, _game.EnemyTextures, _game.BossTextures,
                _game.OverworldEnemyTextures, _game.ItemTextures, _keyboard);
        }

        public void Enter()
        {
            var loader = new RoomLoader(_game, _entityManager);
            var (rooms, startIndex) = loader.LoadAllRooms();

            _navigator = new RoomNavigator(rooms, startIndex);
            _navigator.RoomChanged += OnRoomChanged;

            _projectiles = new ProjectileManager(_game.LinkTextures, _game);
            _hotbar = new Hotbar(3);
            _hud = new HudRenderer(_game.PixelHud, _game.HudTexture);
            OnRoomChanged(_navigator.Current);
        }

        public void Exit() { }

        public void Update(GameTime gameTime)
        {
            if (_isTransitioning)
            {
                _transitionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_transitionTimer >= TRANSITION_DURATION)
                {
                    CompleteTransition();
                }
                return; 
            }

            _keyboard.Update();
            _mouse.Update();
            _gamepad.Update();

            _navigator.Current.Update(gameTime);
            _projectiles?.Update(gameTime);
            _collisions.Step(_navigator.Current, _player, _projectiles);
            if (_player != null && _player.CurrentHealth <= 0)
            {
                _game.StateManager.ChangeState("gameover");
                return;
            }
            _camera?.Update(_player, gameTime);

            if (_player != null && !_isTransitioning)
            {
                var transition = _transitionManager.CheckTransition(
                    _navigator.Index,
                    _player.BoundingBox
                );

                if (transition != null)
                {
                    StartTransition(transition);
                }
            }

            var ms = Mouse.GetState();
            if (_prevMouse.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed)
            {
                _navigator.Previous();
            }
            if (_prevMouse.RightButton == ButtonState.Released && ms.RightButton == ButtonState.Pressed)
            {
                _navigator.Next();
            }
            _prevMouse = ms;
        }

        private void StartTransition(RoomTransition transition)
        {
            _isTransitioning = true;
            _transitionTimer = 0f;
            _pendingTransition = transition;

        }

        private void CompleteTransition()
        {
            _isTransitioning = false;

            if (_pendingTransition != null)
            {
                _navigator.SwitchTo(_pendingTransition.TargetRoomId);

                if (_player != null)
                {
                    _player.Position = _pendingTransition.SpawnPosition;
                    _player.Velocity = Vector2.Zero;
                }

                _pendingTransition = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color drawColor = Color.White;
            if (_isTransitioning)
            {
                float fadeAmount = 1.0f - Math.Abs(_transitionTimer - TRANSITION_DURATION / 2f) / (TRANSITION_DURATION / 2f);
                drawColor = Color.Lerp(Color.White, Color.Black, fadeAmount);
            }

            spriteBatch.Begin(
                samplerState: SamplerState.PointClamp,
                transformMatrix: _camera?.TransformMatrix
            );

            if (!_isTransitioning || _transitionTimer < TRANSITION_DURATION / 2f)
            {
                _navigator.Current?.Draw(spriteBatch);
                _projectiles?.Draw(spriteBatch);
            }

            spriteBatch.End();
            if (_isTransitioning)
            {
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);

                var viewport = _game.GraphicsDevice.Viewport;
                var fadeRect = new Rectangle(0, 0, viewport.Width, viewport.Height);

                float fadeAlpha = Math.Min(1.0f, _transitionTimer * 2 / TRANSITION_DURATION);
                if (_transitionTimer > TRANSITION_DURATION / 2f)
                    fadeAlpha = Math.Max(0f, 2.0f - _transitionTimer * 2 / TRANSITION_DURATION);

                using (var pixel = new Texture2D(_game.GraphicsDevice, 1, 1))
                {
                    pixel.SetData(new[] { Color.White });
                    spriteBatch.Draw(pixel, fadeRect, Color.Black * fadeAlpha);
                }

                spriteBatch.End();
            }

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _hud.Draw(
                spriteBatch,
                _game.Font,
                _game.GraphicsDevice,
                _navigator.Current,
                _player
            );

            spriteBatch.End();
        }

        public void Reset()
        {
            _navigator.Current.Reset();
            _projectiles = new ProjectileManager(_game.LinkTextures, _game);
            _isTransitioning = false;
            _transitionTimer = 0f;
            _pendingTransition = null;

            if (_player != null)
            {
                _inputBinder.BindFor(_player, _projectiles, _hotbar, _game);
                _camera?.SnapToTarget(_player);
            }
        }

        private void OnRoomChanged(Room room)
        {
            var previous = _player;
            _player = room?.GetPlayer();

            // Preserve stats across room Player instances
            if (previous != null && _player != null && !ReferenceEquals(previous, _player))
            {
                _player.CurrentHealth = previous.CurrentHealth;
                _player.CurrentMagic  = previous.CurrentMagic;
                _player.CurrentXP     = previous.CurrentXP;
                _player.Lives         = previous.Lives;
            }

            if (_player != null)
            {
                GamepadController.ControlPlayer = _player;
                _inputBinder.BindFor(_player, _projectiles, _hotbar, _game);
            }

            if (room != null && _camera != null)
            {
                _camera.SetBounds(room.Width, room.Height);
                _camera.SnapToTarget(_player);
            }
        }
    }
}