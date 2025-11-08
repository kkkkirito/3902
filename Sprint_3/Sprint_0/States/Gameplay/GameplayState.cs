using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Collision_System;
using Sprint_0.Interfaces;
using Sprint_0.Rooms;
using Sprint_0.Systems;

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
        private readonly HudRenderer _hud = new();
        private readonly RoomEntityManager _entityManager;
        private RoomNavigator _navigator;
        private IPlayer _player;
        private IProjectileManager _projectiles;
        private IHotbar _hotbar;
        private MouseState _prevMouse;

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

            _entityManager = new RoomEntityManager(_game.LinkTextures, _game.EnemyTextures, _game.OverworldEnemyTextures, _game.ItemTextures, _keyboard);
        }

        public void Enter()
        {
            var loader = new RoomLoader(_game, _entityManager);
            var (rooms, startIndex) = loader.LoadAllRooms();

            _navigator = new RoomNavigator(rooms, startIndex);
            _navigator.RoomChanged += OnRoomChanged;

            _projectiles = new ProjectileManager(_game.LinkTextures, _game);
            _hotbar = new Hotbar(3);
            OnRoomChanged(_navigator.Current);
        }

        public void Exit() { }

        public void Update(GameTime gameTime)
        {
            _keyboard.Update();
            _mouse.Update();
            _gamepad.Update();

            _navigator.Current.Update(gameTime);
            _projectiles?.Update(gameTime);
            _collisions.Step(_navigator.Current, _player, _projectiles);

            var ms = Mouse.GetState();
            if (_prevMouse.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed) _navigator.Previous();
            if (_prevMouse.RightButton == ButtonState.Released && ms.RightButton == ButtonState.Pressed) _navigator.Next();
            _prevMouse = ms;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _navigator.Current.Draw(spriteBatch);
            _projectiles?.Draw(spriteBatch);
            HudRenderer.Draw(spriteBatch, _game.Font, _game.GraphicsDevice, _navigator.Current, _navigator.Index, _navigator is null ? 0 : (_navigator.Current is null ? 0 : 1 + 0), _player);

            spriteBatch.End();
        }

        public void Reset()
        {
            _navigator.Current.Reset();
            _projectiles = new ProjectileManager(_game.LinkTextures, _game);
            if (_player != null) _inputBinder.BindFor(_player, _projectiles, _hotbar, _game);
        }

        private void OnRoomChanged(Room room)
        {
            _player = room?.GetPlayer();
            if (_player != null)
            {
                GamepadController.ControlPlayer = _player;
                _inputBinder.BindFor(_player, _projectiles, _hotbar, _game);
            }
        }
    }
}