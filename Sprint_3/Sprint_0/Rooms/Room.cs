using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Items;
using Sprint_0.Blocks;
using Sprint_0.Managers;
using Sprint_0.Player_Namespace;
using System.Collections.Generic;
using System.Linq;
using Sprint_0.States.LinkStates;
using System;
using Sprint_0.States;

namespace Sprint_0.Rooms
{
    public class Room
    {
        public int Id { get; }
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }
        //0 = very dark note, do not use 0.00 because it WILL cause issues with multiplication, 1 = fully lit
        public float AmbientLightLevel { get; set; } = 0.5f;

        // Fall damage configuration
        private const int FALL_DAMAGE = 50;
        private const int FALL_BOUNDARY_Y = 560; // Y position that triggers fall damage

        private Player player;
        private Vector2 playerStartPosition;
        private readonly List<IBlock> blocks = new();
        private readonly List<IBlock> originalEntityBlocks = new();
        private readonly List<Enemy> enemies = new();
        private readonly List<IItem> items = new();
        private readonly List<ICollidable> collidables = new();
        private readonly List<IStaticCollider> mergedStatics = new();
        private readonly Dictionary<Enemy, Vector2> enemyStartPositions;

        public Room(int id, string name, int width, int height)
        {
            Id = id;
            Name = name;
            Width = width;
            Height = height;
            blocks = new List<IBlock>();
            originalEntityBlocks = new List<IBlock>();
            enemies = new List<Enemy>();
            enemyStartPositions = new Dictionary<Enemy, Vector2>();
            items = new List<IItem>();
            collidables = new List<ICollidable>();
            AmbientLightLevel = 0.5f;

        }

        public void SetPlayer(Player p)
        {
            player = p;
            if (p != null)
            {
                playerStartPosition = p.Position;
            }
        }

        public Player GetPlayer() => player;

        public Vector2 GetPlayerStartPosition() => playerStartPosition;

        public IEnumerable<IItem> GetItems() => items;

        public IEnumerable<ILightSource> GetActiveLightSources()
        {
            foreach (var item in items)
            {
                if (item is ILightSource light && light.IsLightActive)
                {
                    yield return light;
                }
            }
            foreach (var block in blocks)
            {
                if (block is ILightSource light && light.IsLightActive)
                {
                    yield return light;
                }
            }
        }

        public void AddBlock(IBlock block)
        {
            if (block is TrapBlock || block is LockedDoor)
            {
                originalEntityBlocks.Add(block);
            }

            blocks.Add(block);
            if (block is ICollidable collidable)
            {
                collidables.Add(collidable);
            }
        }

        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
            if (enemy is ICollidable collidable)
            {
                collidables.Add(collidable);
            }
            enemy.OnDeath -= NotifyDeath;
            enemy.OnDeath += NotifyDeath;
        }

        private void NotifyDeath(Enemy enemy)
        {
            player?.AddXP(enemy.XPReward);
            XPManager.Spawn(enemy.SpriteSheet, enemy.Position, enemy.XPReward);
        }

        public void AddItem(IItem item)
        {
            items.Add(item);
            if (item is ICollidable collidable)
            {
                collidables.Add(collidable);
            }
        }

        public IEnumerable<ICollidable> GetCollidables()
        {
            foreach (var c in collidables) yield return c;

            // gather extras from enemies (projectiles etc.) without type checks
            foreach (var extra in enemies.SelectMany(e => e.GetExtraCollidables()))
                yield return extra;
        }

        public IEnumerable<IBlock> GetBlocks() => blocks;
        public IEnumerable<Enemy> GetEnemies() => enemies;

        public void Update(GameTime gameTime)
        {
            if (PauseState.IsPaused) return;

            player?.Update(gameTime);
            UpdateEntities(gameTime);
            CleanupBrokenTrapBlocks();
        }

        private void UpdateEntities(GameTime gameTime)
        {
            foreach (var block in blocks)
                block.Update(gameTime);

            foreach (var enemy in enemies)
                enemy.Update(gameTime);

            foreach (var item in items)
                item.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawEntities(spriteBatch);
            player?.Draw(spriteBatch);
        }

        private void DrawEntities(SpriteBatch spriteBatch)
        {
            foreach (var block in blocks) block.Draw(spriteBatch);
            foreach (var item in items) item.Draw(spriteBatch);
            foreach (var enemy in enemies) enemy.Draw(spriteBatch);
        }

        /// <summary>
        /// Checks if the player has fallen out of bounds (below the map)
        /// </summary>
        /// <returns>True if player is out of bounds</returns>
        public bool IsPlayerOutOfBounds()
        {
            if (player == null) return false;

            // Check if player has fallen below the map boundary
            return player.Position.Y > FALL_BOUNDARY_Y;
        }

        /// <summary>
        /// Applies fall damage to player and resets their position to start
        /// </summary>
        public void HandlePlayerFallOutOfBounds()
        {
            if (player == null) return;
            if (player.GameMode == GameModeType.Platformer)
            {
                player.TakeDamage(FALL_DAMAGE);

                player.Position = playerStartPosition;
                player.Velocity = Vector2.Zero;

                if (player is Player p)
                {
                    p.VerticalVelocity = 0f;
                }

                player.IsGrounded = true;
            }
        }

        private void ResetRoom(bool keepMagic, bool keepXP)
        {
            player?.ResetToStart(playerStartPosition, keepMagic, keepXP);
            foreach (var entityBlock in originalEntityBlocks)
            {
                if (!blocks.Contains(entityBlock))
                    blocks.Add(entityBlock);

                if (entityBlock is ICollidable collidable && !collidables.Contains(collidable))
                    collidables.Add(collidable);

                if (entityBlock is IResettable resettableBlock)
                    resettableBlock.ResetState();
            }
            foreach (var item in items)
            {
                if (item is IResettable resettableItem)
                    resettableItem.ResetState();
            }
            foreach (var enemy in enemies)
            {
                enemy.ResetState();
            }
        }

        public void Reset() => ResetRoom(keepMagic: false, keepXP: false);
        public void Die() => ResetRoom(keepMagic: true, keepXP: true);

        private void CleanupBrokenTrapBlocks()
        {
            bool IsBrokenTrap(ICollidable c) => c is TrapBlock tb && tb.IsBroken;

            blocks.RemoveAll(b => b is TrapBlock tb && tb.IsBroken);
            collidables.RemoveAll(IsBrokenTrap);
        }

        public void ReplaceStaticBlockColliders(IEnumerable<IStaticCollider> merged)
        {
            mergedStatics.Clear();
            mergedStatics.AddRange(merged);

            collidables.RemoveAll(c => c is IBlock);
            collidables.AddRange(mergedStatics);
        }
    }
}
