using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Blocks;
using Sprint_0.Collision_System;
using Sprint_0.Commands.BlockCommands;
using Sprint_0.Commands.CollisionCommands;
using Sprint_0.Commands.GameCommands;
using Sprint_0.Commands.PlayerCommands;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;
using Sprint_0.Rooms;
using Sprint_0.Systems;
using System.Collections.Generic;


namespace Sprint_0.States
{
    public class GameplayState : IGameState
    {
        private Game1 game;
        private ContentManager content;
        private RoomEntityManager roomEntityManager;

        // Controllers
        private IController keyboardController;
        private IController mouseController;
        private IController gamepadController;

        // Room management
        private Room currentRoom;
        private List<Room> rooms;
        private int currentRoomIndex;


        // Player reference
        private IPlayer player;

        // Projectiles
        private IProjectileManager projectileManager;

        // Hotbar
        private IHotbar hotbar;

        // Block system (for room editor mode - optional)
        private BlockFactory blockFactory;
        private BlockSelector blockSelector;

        // Mouse state for room switching
        private MouseState previousMouseState;
        private ICollidable currentBlock;
        private CollisionSystem collisionSystem;

        public GameplayState(Game1 game, ContentManager content)
        {
            this.game = game;
            this.content = content;
            rooms = new List<Room>();
        }

        public void Enter()
        {
            InitializeControllers();
            InitializeRoomSystem();
            LoadRooms();
            InitializeCommands();
        }

        private void InitializeControllers()
        {
            keyboardController = new KeyboardController(game);
            mouseController = new MouseController(game);
            gamepadController = new GamepadController(PlayerIndex.One);
        }

        private void InitializeRoomSystem()
        {

            roomEntityManager = new RoomEntityManager(
                game.LinkTextures,
                game.EnemyTextures,
                game.OverworldEnemyTextures,
                game.ItemTextures,
                keyboardController
            );

            // Create block factory
            blockFactory = new BlockFactory(content);
            blockSelector = new BlockSelector(blockFactory);

            collisionSystem = new CollisionSystem();

            // Create projectile manager
            projectileManager = new ProjectileManager(game.LinkTextures);

            // Create hotbar
            hotbar = new Hotbar(3);
            
            var playerBlockCmd = new PlayerBlockCollisionCommand();
            collisionSystem.Provider.Register<IPlayer, IBlock>(playerBlockCmd);

            // Register player <-> enemy collisions
            var playerEnemyCmd = new PlayerEnemyCollisionCommand();
            collisionSystem.Provider.Register<IPlayer, Enemy>(playerEnemyCmd);

            var playerAttackEnemyCmd = new PlayerAttackEnemyCollisionCommand();
            collisionSystem.Provider.Register<PlayerAttackHitbox, Enemy>(playerAttackEnemyCmd);

            var enemyBlockcmd = new EnemyBlockCollisionCommand();
            collisionSystem.Provider.Register<Enemy, IBlock>(enemyBlockcmd);

            
            var playerEnemyProjectileCmd = new PlayerEnemyProjectileCollisionCommand();
            collisionSystem.Provider.Register<IPlayer, IEnemyProjectile>(playerEnemyProjectileCmd);

            collisionSystem.Provider.Register<IPlayer, IStaticCollider>(new PlayerStaticColliderCollisionCommand());
            collisionSystem.Provider.Register<Enemy, IStaticCollider>(new EnemyStaticColliderCollisionCommand());

            collisionSystem.Provider.Register<PlayerProjectile, Enemy>(new PlayerProjectileEnemyCollisionCommand());


        }

        private void LoadRooms()
        {
    
            Room room0 = new Room(0, "Palace Exterior", 1024, 480);
            Room room1 = new Room(1, "Dungeon Room 1", 1024, 480);
            Room room2 = new Room(2, "Dungeon Room 2", 1024, 480);
            Room room3 = new Room(3, "Dungeon Room 3", 1024, 480);
            Room room4 = new Room(4, "Dungeon Room 4", 1024, 480);
            Room room5 = new Room(5, "Dungeon Room 5", 1024, 480);
            Room room6 = new Room(6, "Dungeon Room 6", 1024, 480);
            Room room7 = new Room(7, "Dungeon Room 7", 1024, 480);



            string csvPath = "Content/palace_exterior.csv";
            var roomBuilder = new RoomBuilder(csvPath, game.BlockTextures);
            roomBuilder.PopulateRoom(room0);

            string entitiesCsvPath = "Content/palace_exterior_entities.csv";
            roomEntityManager.LoadEntities(room0, entitiesCsvPath);

            csvPath = "Content/Dungeon Room 1.csv";
            roomBuilder = new RoomBuilder(csvPath, game.BlockTextures);
            roomBuilder.PopulateRoom(room1);

            entitiesCsvPath = "Content/Dungeon Room 1 entities.csv";
            roomEntityManager.LoadEntities(room1, entitiesCsvPath);

            csvPath = "Content/Dungeon Room 2.csv";
            roomBuilder = new RoomBuilder(csvPath, game.BlockTextures);
            roomBuilder.PopulateRoom(room2);

            entitiesCsvPath = "Content/Dungeon Room 2 entities.csv";
            roomEntityManager.LoadEntities(room2, entitiesCsvPath);

            csvPath = "Content/Dungeon Room 3.csv";
            roomBuilder = new RoomBuilder(csvPath, game.BlockTextures);
            roomBuilder.PopulateRoom(room3);

            entitiesCsvPath = "Content/Dungeon Room 3 entities.csv";
            roomEntityManager.LoadEntities(room3, entitiesCsvPath);

            csvPath = "Content/Dungeon Room 4.csv";
            roomBuilder = new RoomBuilder(csvPath, game.BlockTextures);
            roomBuilder.PopulateRoom(room4);

            entitiesCsvPath = "Content/Dungeon Room 4 entities.csv";
            roomEntityManager.LoadEntities(room4, entitiesCsvPath);

            csvPath = "Content/Dungeon Room 5.csv";
            roomBuilder = new RoomBuilder(csvPath, game.BlockTextures);
            roomBuilder.PopulateRoom(room5);

            entitiesCsvPath = "Content/Dungeon Room 5 entities.csv";
            roomEntityManager.LoadEntities(room5, entitiesCsvPath);

            csvPath = "Content/Dungeon Room 6.csv";
            roomBuilder = new RoomBuilder(csvPath, game.BlockTextures);
            roomBuilder.PopulateRoom(room6);

            entitiesCsvPath = "Content/Dungeon Room 6 entities.csv";
            roomEntityManager.LoadEntities(room6, entitiesCsvPath);

            csvPath = "Content/Dungeon Room 7.csv";
            roomBuilder = new RoomBuilder(csvPath, game.BlockTextures);
            roomBuilder.PopulateRoom(room7);

            entitiesCsvPath = "Content/Dungeon Room 7 entities.csv";
            roomEntityManager.LoadEntities(room7, entitiesCsvPath);








            rooms.Add(room0);

            rooms.Add(room1);
            rooms.Add(room2);
            rooms.Add(room3);
            rooms.Add(room4);
            rooms.Add(room5);
            rooms.Add(room6);
            rooms.Add(room7);
            currentRoomIndex = 0;
            currentRoom = rooms[0];
            player = currentRoom.GetPlayer();


            if (player != null)
            {
                GamepadController.ControlPlayer = player;
            }
        }



        private void InitializeCommands()
        {
            if (player == null) return;

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

            // Block commands (for room editor - optional)
            kb.Press(Keys.T, new PreviousBlockCommand(blockSelector));
            kb.Press(Keys.Y, new NextBlockCommand(blockSelector));

            // Hotbar commands
            for (int i = 1; i <= 3; i++)
            {
                int slot = i - 1;
                kb.Press((Keys)((int)Keys.D0 + i), new UseSlotCommand(hotbar, player, slot));
            }

            // Game commands
            kb.Press(Keys.Q, new QuitCommand(game));
            kb.Press(Keys.R, new ResetCommand(this));

            // Quick room switching with number keys (for testing)
            kb.Press(Keys.F1, new Commands.RoomCommand.SwitchRoomCommand(this, 0));
            kb.Press(Keys.F2, new Commands.RoomCommand.SwitchRoomCommand(this, 1));
            kb.Press(Keys.F3, new Commands.RoomCommand.SwitchRoomCommand(this, 2));

            // Gamepad commands
            var pad = (GamepadController)gamepadController;
            pad.BindHold(Buttons.DPadUp, new MoveUpCommand(player));
            pad.BindHold(Buttons.DPadDown, new MoveDownOrCrouchOnCommand(player));
            pad.BindHold(Buttons.DPadLeft, new MoveLeftCommand(player));
            pad.BindHold(Buttons.DPadRight, new MoveRightCommand(player));
            pad.Press(Buttons.X, new AttackCommand(player));
            pad.Press(Buttons.A, new JumpCommand(player));
            pad.BindRelease(Buttons.DPadDown, new CrouchOffCommand(player));
            pad.Press(Buttons.LeftShoulder, new PickupCommand(player));
            pad.Press(Buttons.Y, new SwordBeamCommand(player, projectileManager));
            pad.Press(Buttons.B, new FireballCommand(player, projectileManager));
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

            // Update current room
            currentRoom?.Update(gameTime);

            // Update projectiles
            projectileManager?.Update(gameTime);

            // Update collision system with null checks
            if (collisionSystem != null && player != null)
            {  
                var allCollidables = new List<ICollidable>();
                // player
                allCollidables.AddRange(player.GetCollidables());

                // ADD: all room collidables (blocks, enemies, etc.)
                var roomCollidables = currentRoom?.GetCollidables();
                if (roomCollidables != null)
                    allCollidables.AddRange(roomCollidables);

                if (projectileManager != null)
                {
                    foreach (var c in projectileManager.GetCollidables())
                        allCollidables.Add(c);
                }

                // optional editor-selected block
                if (currentBlock != null)
                    allCollidables.Add((ICollidable)currentBlock);

                collisionSystem.RegisterCollidables(allCollidables);
                collisionSystem.Update();
            }

            // Handle mouse room switching
            MouseState currentMouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Released &&
                currentMouseState.LeftButton == ButtonState.Pressed)
            {
                // Previous room
                PreviousRoom();
            }
            else if (previousMouseState.RightButton == ButtonState.Released &&
                     currentMouseState.RightButton == ButtonState.Pressed)
            {
                // Next room
                NextRoom();
            }
            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp); // Use PointClamp for pixel-perfect rendering

            // Draw the current room
            currentRoom?.Draw(spriteBatch);

            // Draw projectiles
            projectileManager?.Draw(spriteBatch);

            // Draw UI elements
            DrawUI(spriteBatch);

            spriteBatch.End();
        }

        private void DrawUI(SpriteBatch spriteBatch)
        {
            // Draw room info at top of screen
            string roomInfo = $"Room {currentRoomIndex + 1}/{rooms.Count}: {currentRoom?.Name ?? "Unknown"}";
            spriteBatch.DrawString(game.Font, roomInfo, new Vector2(10, 10), Color.White);

            // Draw player health
            if (player != null)
            {
                string healthInfo = $"Health: {player.CurrentHealth}/{player.MaxHealth}";
                spriteBatch.DrawString(game.Font, healthInfo, new Vector2(10, 30), Color.White);
            }

            // Draw controls hint
            string controls = "Mouse: L/R to switch rooms | F1-F3: Quick room select | R: Reset";
            Vector2 textSize = game.Font.MeasureString(controls);
            spriteBatch.DrawString(game.Font, controls,
                new Vector2(10, game.GraphicsDevice.Viewport.Height - textSize.Y - 10), Color.Gray);
        }

        public void Reset()
        {
            // Reset current room
            currentRoom?.Reset();

            // Reset projectiles
            projectileManager = new ProjectileManager(game.LinkTextures);

            // Reinitialize commands with the reset player
            if (player != null)
            {
                InitializeCommands();
            }
        }

        public void SwitchToRoom(int roomIndex)
        {
            if (roomIndex >= 0 && roomIndex < rooms.Count)
            {
                currentRoomIndex = roomIndex;
                currentRoom = rooms[currentRoomIndex];
                player = currentRoom.GetPlayer();

                // Update controllers with new player
                if (player != null)
                {
                    GamepadController.ControlPlayer = player;
                    InitializeCommands();
                }
            }
        }

        private void NextRoom()
        {
            if (rooms.Count > 0)
            {
                currentRoomIndex = (currentRoomIndex + 1) % rooms.Count;
                SwitchToRoom(currentRoomIndex);
            }
        }

        private void PreviousRoom()
        {
            if (rooms.Count > 0)
            {
                currentRoomIndex = (currentRoomIndex - 1 + rooms.Count) % rooms.Count;
                SwitchToRoom(currentRoomIndex);
            }
        }
    }
}