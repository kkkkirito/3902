using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Items;
using Sprint_0.Player_Namespace;
using System.Collections.Generic;
using Sprint_0.States.LinkStates;

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
            player?.Update(gameTime);

            foreach (var block in blocks)
            {
                block.Update(gameTime);
            }

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
                player.Position = playerStartPosition;
                player.CurrentHealth = player.MaxHealth;
                player.IsInvulnerable = false;
                player.Velocity = Vector2.Zero;

                if (player.CurrentState != null)
                {
                    player.ChangeState(new IdleState());
                }
            }

            foreach (var item in items)
            {
                if (item is HeartItem heart) heart.IsCollected = false;
                else if (item is KeyItem key) key.IsCollected = false;
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
                        enemy.BoundingBox = new Rectangle((int)enemy.Position.X,(int)enemy.Position.Y,anim.FrameWidth,anim.FrameHeight);
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