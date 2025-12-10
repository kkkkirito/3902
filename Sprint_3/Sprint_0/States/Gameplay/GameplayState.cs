using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Collision_System;
using Sprint_0.Interfaces;
using Sprint_0.Managers;
using Sprint_0.Player_Namespace;
using Sprint_0.Rooms;
using Sprint_0.States.LinkStates;
using Sprint_0.Systems;
using Sprint_0.Systems.Lighting;
using System;
using static Sprint_0.States.Gameplay.InputBinder;


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
        private readonly LightingRenderer _lighting;

        private readonly InputSelector _inputSelector;

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
                _game.OverworldEnemyTextures, _game.ItemTextures, game.BlockTextures, _keyboard);
            _inputSelector = new InputSelector();
            _lighting = new LightingRenderer(_game.GraphicsDevice);
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
            _player.Lives = 3;
        }

        public void Exit() { }

        public void Update(GameTime gameTime)
        {
            _inputSelector.Update();
            PauseState.Update();
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
            XPManager.UpdateAll(gameTime);

            _navigator.Current.Update(gameTime);
            if (!PauseState.IsPaused)
            {
                _projectiles?.Update(gameTime);
                _collisions.Step(_navigator.Current, _player, _projectiles);

                if (_player != null && _camera != null)
                {
                    var newMode = _player.GameMode == GameModeType.TopDown
                        ? CameraMode.TopDown
                        : CameraMode.Platformer;
                    if (_camera.Mode != newMode)
                    {
                        _camera.Mode = newMode;
                        _camera.SetBounds(_navigator.Current.Width, _navigator.Current.Height);
                        _camera.SnapToTarget(_player);
                    }
                }
                _camera?.Update(_player, gameTime);

                // Check if player has fallen out of bounds
                CheckPlayerFallOutOfBounds();
            }
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
            if (_player != null && _player.CurrentHealth <= 0)
            {

                if (_player != null && _player.CurrentHealth <= 0)
                {
                    if (_player.CurrentState is DeadState || (_player is Player p && p.IsDying))
                    {
                        if (_player is Player pClear) pClear.IsDying = false;

                        if (_player is Player concretePlayer && concretePlayer.LivesAvailable)
                        {
                            _navigator.Current.Die();
                            _projectiles = new ProjectileManager(_game.LinkTextures, _game);
                            _inputBinder.BindFor(_player, _projectiles, _hotbar, _game);
                            _camera?.SnapToTarget(_player);
                            concretePlayer.LivesAvailable = false;
                        }
                        else
                        {
                            _game.StateManager.ChangeState("gameover");
                            return;
                        }
                    }
                }
            }
            var ms = Mouse.GetState();
            if (_prevMouse.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed)
            {
                _navigator.Previous();
                Reset();
            }
            if (_prevMouse.RightButton == ButtonState.Released && ms.RightButton == ButtonState.Pressed)
            {
                _navigator.Next();
                Reset();
            }
            _prevMouse = ms;
        }

        /// <summary>
        /// Checks if the player has fallen out of bounds and handles the fall damage/reset
        /// </summary>
        private void CheckPlayerFallOutOfBounds()
        {
            if (_navigator.Current == null || _player == null) return;

            if (_navigator.Current.IsPlayerOutOfBounds())
            {
                _navigator.Current.HandlePlayerFallOutOfBounds();
                _camera?.SnapToTarget(_player);
            }
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

            bool shouldRenderRoom = !_isTransitioning || _transitionTimer < TRANSITION_DURATION / 2f;

            //ok, sandwich scene with dimming and lighting
            if (shouldRenderRoom)
            {
                // Generate light map before drawing the room
                _lighting?.GenerateLightMap(_camera, _navigator.Current, _player, _projectiles);
            }

            //Draw the actual level
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera?.TransformMatrix);

            XPManager.DrawAll(spriteBatch);

            if (shouldRenderRoom)
            {
                _navigator.Current?.Draw(spriteBatch);
                _projectiles?.Draw(spriteBatch);
            }

            spriteBatch.End();

            //now we draw lighting/shadows on top
            if (shouldRenderRoom)
            {
                _lighting?.DrawShadows(spriteBatch, _navigator.Current);
            }

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

            _hud.Draw(spriteBatch, _game.Font, _game.GraphicsDevice, _navigator.Current, _player);

            spriteBatch.End();
            PauseState.Draw(spriteBatch, _game.Font, _game.GraphicsDevice);
        }

        public void Reset()
        {
            _navigator.Current.Reset();
            _projectiles = new ProjectileManager(_game.LinkTextures, _game);
            _isTransitioning = false;
            _transitionTimer = 0f;
            _pendingTransition = null;
            XPManager.Clear();

            if (_player != null)
            {
                _inputBinder.BindFor(_player, _projectiles, _hotbar, _game);
                _camera?.SnapToTarget(_player);
            }
        }

        private void OnRoomChanged(Room room)
        {
            if (_camera != null && room != null)
            {
                _camera.SetIgnoreBounds(room.Id == 15);
            }
            var previous = _player;
            _player = room?.GetPlayer();

            // Preserve stats across room Player instances
            if (previous != null && _player != null && !ReferenceEquals(previous, _player))
            {
                _player.CurrentHealth = previous.CurrentHealth;
                _player.CurrentMagic = previous.CurrentMagic;
                _player.CurrentXP = previous.CurrentXP;
                _player.Lives = previous.Lives;
                _player.GameMode = previous.GameMode;
            }

            if (_player != null)
            {
                GamepadController.ControlPlayer = _player;
                _inputBinder.BindFor(_player, _projectiles, _hotbar, _game);
            }

            if (room != null && _camera != null)
            {
                _camera.SetBounds(
                room.Width * 16,
                room.Height * 16
                );
                _camera.SnapToTarget(_player);
            }
            if (room.Id == 15)
            {
                _camera.SetFixedZoom(4.0f);
            }
            else
            {
                _camera.SetFixedZoom(-1f);
            }
        }
    }
}