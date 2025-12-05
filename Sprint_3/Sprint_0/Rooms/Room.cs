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

        private Player player;
        private Vector2 playerStartPosition;
        private readonly List<IBlock> blocks = new();
        private readonly List<IBlock> originalEntityBlocks = new();
        private readonly List<Enemy> enemies = new();
        private readonly List<IItem> items = new();
        private readonly List<ICollidable> collidables = new();
        private readonly List<IStaticCollider> mergedStatics = new();

        public Room(int id, string name, int width, int height)
        {
            Id = id;
            Name = name;
            Width = width;
            Height = height;
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

        public IEnumerable<IItem> GetItems() => items;

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
            enemy.StartPosition = enemy.Position;

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