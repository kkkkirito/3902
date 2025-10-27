using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Blocks;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Items;
using Sprint_0.Player_Namespace;
using System.Collections.Generic;

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
        private List<IConsumableItem> consumables;
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
            consumables = new List<IConsumableItem>();
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

        public IEnumerable<IConsumableItem> GetConsumables()
        {
            return consumables;
        }

        public void AddBlock(IBlock block)
        {
            blocks.Add(block);
            // If the block implements ICollidable, add it to collidables
            if (block is ICollidable collidable)
            {
                collidables.Add(collidable);
            }
        }

        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
            enemyStartPositions[enemy] = enemy.Position;
            // If the enemy implements ICollidable, add it to collidables
            if (enemy is ICollidable collidable)
            {
                collidables.Add(collidable);
            }
        }

        public void AddConsumable(IConsumableItem item)
        {
            consumables.Add(item);
        }

        public IEnumerable<ICollidable> GetCollidables()
        {
            // Return all collidable objects in the room
            return collidables;
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
            // Update player
            player?.Update(gameTime);

            // Update blocks
            foreach (var block in blocks)
            {
                block.Update(gameTime);
            }

            // Update enemies
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
            }

            // Update items
            foreach (var item in consumables)
            {
                item.Update(gameTime);
            }

            // Check item collection (simple collision with player)
            if (player != null)
            {
                Rectangle playerBounds = player.BoundingBox;
                foreach (var item in consumables)
                {
                    if (item is HeartItem heart && !heart.IsCollected)
                    {
                        if (heart.GetBoundingBox().Intersects(playerBounds))
                        {
                            item.Consume(player); 
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw blocks first (background)
            foreach (var block in blocks)
            {
                block.Draw(spriteBatch);
            }

            // Draw items
            foreach (var item in consumables)
            {
                item.Draw(spriteBatch);
            }

            // Draw enemies
            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            // Draw player 
            player?.Draw(spriteBatch);

            
        }

        public void Reset()
        {
            // Reset player position
            if (player != null)
            {
                player.Position = playerStartPosition;
                player.CurrentHealth = player.MaxHealth;
                player.IsInvulnerable = false;
                player.Velocity = Vector2.Zero;

                if (player.CurrentState != null)
                {
                    player.ChangeState(new Sprint_0.States.LinkStates.IdleState());
                }
            }

            // Reset items
            foreach (var item in consumables)
            {
                if (item is HeartItem heart)
                {
                    heart.IsCollected = false;
                }
            }

            // Reset enemies to their starting positions (if needed)
            foreach (var enemy in enemies)
            {
                if (enemyStartPositions.ContainsKey(enemy))
                {
                    enemy.Position = enemyStartPositions[enemy];
                    enemy.Velocity = Vector2.Zero;
                    enemy.IsDead = false;
                    enemy.CurrentHealth = enemy.MaxHealth;
                    enemy.IsInvulnerable = false;

                    // Reset enemy to appropriate initial state based on type
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
            // Reset to appropriate initial state based on enemy type
            if (enemy is StalfosEnemy)
            {
                enemy.ChangeState(new Sprint_0.EnemyStateMachine.IdleState());
            }
            else if (enemy is BotEnemy)
            {
                enemy.ChangeState(new Sprint_0.EnemyStateMachine.IdleState());
            }
            else if (enemy is OctorokEnemy)
            {
                enemy.ChangeState(new Sprint_0.EnemyStateMachine.IdleState());
            }
            else if (enemy is OverworldBotEnemy || enemy is OverworldManEnemy)
            {
                enemy.ChangeState(new Sprint_0.EnemyStateMachine.OverworldIdleState());
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