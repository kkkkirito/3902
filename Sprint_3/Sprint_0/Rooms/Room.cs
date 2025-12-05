using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Items;
using Sprint_0.Blocks;
using Sprint_0.Managers;
using Sprint_0.Player_Namespace;
using System.Collections.Generic;
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
        private List<IBlock> blocks;
        private List<IBlock> originalEntityBlocks;
        private List<Enemy> enemies;
        private Dictionary<Enemy, Vector2> enemyStartPositions;
        private List<IItem> items;
        private List<ICollidable> collidables;
        private readonly List<IStaticCollider> mergedStatics = new();

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
        }

        public void SetPlayer(Player p)
        {
            player = p;
            if (p != null)
            {
                playerStartPosition = p.Position;
            }
        }

        public Player GetPlayer()
        {
            return player;
        }

        public IEnumerable<IItem> GetItems()
        {
            return items;
        }

        public void AddBlock(IBlock block)
        {

            if (block is TrapBlock || block is LockedDoor || block is TopDownDoor)
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
            enemyStartPositions[enemy] = enemy.Position;
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

            foreach (var enemy in enemies)
            {
                if (enemy is OctorokEnemy oct)
                {
                    foreach (var proj in oct.GetActiveProjectiles())
                        yield return proj;
                }
            }
        }

        public IEnumerable<IBlock> GetBlocks()
        {
            return blocks;
        }

        public IEnumerable<Enemy> GetEnemies()
        {
            return enemies;
        }


        public void Update(GameTime gameTime)
        {
            if (PauseState.IsPaused) return;
            player?.Update(gameTime);

            foreach (var block in blocks)
            {
                block.Update(gameTime);
            }

            blocks.RemoveAll(b => b is TrapBlock trap && trap.IsBroken);
            collidables.RemoveAll(c => c is TrapBlock trap && trap.IsBroken);

            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
            }

            foreach (var item in items)
            {
                item.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var block in blocks)
            {
                block.Draw(spriteBatch);
            }

            foreach (var item in items)
            {
                item.Draw(spriteBatch);
            }

            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            player?.Draw(spriteBatch);


        }

        public void Reset()
        {
            if (player != null)
            {
                player.ResetPhysics(playerStartPosition);

                player.CurrentHealth = player.MaxHealth;
                player.IsInvulnerable = false;
                player.CurrentMagic = player.MaxMagic;
                player.CurrentXP = 0;
                player.GameMode = GameModeType.Platformer;

                if (player.CurrentState != null)
                {
                    player.ChangeState(new IdleState());
                }
            }

            foreach (var entityBlock in originalEntityBlocks)
            {
                // Add back if it was removed
                if (!blocks.Contains(entityBlock))
                {
                    blocks.Add(entityBlock);
                }

                if (entityBlock is ICollidable collidable && !collidables.Contains(collidable))
                {
                    collidables.Add(collidable);
                }

                // Reset the block's state
                if (entityBlock is TrapBlock trapBlock)
                {
                    trapBlock.Reset();
                }
                else if (entityBlock is LockedDoor lockedDoor)
                {
                    lockedDoor.Reset();
                }
                else if (entityBlock is TopDownDoor topDownDoor)
                {
                    topDownDoor.Reset();
                }
            }

            foreach (var item in items)
            {
                if (item is HeartItem heart) heart.IsCollected = false;
                else if (item is KeyItem key) key.IsCollected = false;
                else if (item is TopDownKeyItem tdKey) tdKey.IsCollected = false;
                else if (item is TrophyItem trophy) trophy.IsCollected = false;
            }

            foreach (var enemy in enemies)
            {
                if (enemyStartPositions.ContainsKey(enemy))
                {
                    ResetEnemyState(enemy);

                    enemy.SetAnimation("Idle");

                    var anim = enemy.GetAnimation("Idle");
                    if (anim != null)
                    {
                        enemy.BoundingBox = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, anim.FrameWidth, anim.FrameHeight);
                    }
                }
            }
        }
        public void Die()
        {
            if (player != null)
            {
                player.ResetPhysics(playerStartPosition);

                player.CurrentHealth = player.MaxHealth;
                player.IsInvulnerable = false;
                //player.CurrentMagic = player.MaxMagic;
                //player.CurrentXP = 0;

                if (player.CurrentState != null)
                {
                    player.ChangeState(new IdleState());
                }
            }

            foreach (var entityBlock in originalEntityBlocks)
            {
                // Add back if it was removed
                if (!blocks.Contains(entityBlock))
                {
                    blocks.Add(entityBlock);
                }

                if (entityBlock is ICollidable collidable && !collidables.Contains(collidable))
                {
                    collidables.Add(collidable);
                }

                // Reset the block's state
                if (entityBlock is TrapBlock trapBlock)
                {
                    trapBlock.Reset();
                }
                else if (entityBlock is LockedDoor lockedDoor)
                {
                    lockedDoor.Reset();
                }
                else if (entityBlock is TopDownDoor topDownDoor)
                {
                    topDownDoor.Reset();
                }
            }

            foreach (var item in items)
            {
                if (item is HeartItem heart) heart.IsCollected = false;
                else if (item is KeyItem key) key.IsCollected = false;
                else if (item is TopDownKeyItem tdKey) tdKey.IsCollected = false;
                else if (item is TrophyItem trophy) trophy.IsCollected = false;
            }

            foreach (var enemy in enemies)
            {
                if (enemyStartPositions.ContainsKey(enemy))
                {
                    ResetEnemyState(enemy);

                    enemy.SetAnimation("Idle");

                    var anim = enemy.GetAnimation("Idle");
                    if (anim != null)
                    {
                        enemy.BoundingBox = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, anim.FrameWidth, anim.FrameHeight);
                    }
                }
            }
        }

        private void ResetEnemyState(Enemy enemy)
        {
            enemy.Position = enemyStartPositions[enemy];
            enemy.Velocity = Vector2.Zero;
            enemy.IsDead = false;
            enemy.CurrentHealth = enemy.MaxHealth;
            enemy.IsInvulnerable = false;

            // Reset to appropriate initial state based on enemy type
            if (enemy is StalfosEnemy)
            {
                enemy.ChangeState(new Sprint_0.EnemyStateMachine.StalfosFallState());
            }
            else if (enemy is OverworldBotEnemy || enemy is OverworldManEnemy)
            {
                enemy.ChangeState(new Sprint_0.EnemyStateMachine.OverworldIdleState());
            }
            else if (enemy is BubbleEnemy)
            {
                enemy.ChangeState(new Sprint_0.EnemyStateMachine.BubbleState());
            }
            else if (enemy is HorseHeadEnemy)
            {
                enemy.ChangeState(new Sprint_0.EnemyStateMachine.BossIdleState());
            }
            else
            {
                enemy.ChangeState(new Sprint_0.EnemyStateMachine.IdleState());
            }
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