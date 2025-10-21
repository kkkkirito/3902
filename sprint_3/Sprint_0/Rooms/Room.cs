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
        private List<IBlock> blocks;
        private List<Enemy> enemies;
        private List<IConsumableItem> consumables;
        private List<ICollidable> collidables;

        public Room(int id, string name, int width, int height)
        {
            Id = id;
            Name = name;
            Width = width;
            Height = height;

            blocks = new List<IBlock>();
            enemies = new List<Enemy>();
            consumables = new List<IConsumableItem>();
            collidables = new List<ICollidable>();
        }

        public void SetPlayer(Player p)
        {
            player = p;
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
                if (item is HeartItem heart)
                {
                    heart.Update(gameTime);
                }
            }

            // Check item collection (simple collision with player)
            if (player != null)
            {
                Rectangle playerBounds = new Rectangle((int)player.Position.X, (int)player.Position.Y, 18, 42);
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
                if (item is HeartItem heart)
                {
                    heart.Draw(spriteBatch);
                }
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
                player.Position = new Vector2(100, 300);
                player.CurrentHealth = player.MaxHealth;
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


        }
    }
}