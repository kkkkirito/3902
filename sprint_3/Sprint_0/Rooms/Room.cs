using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Blocks;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;

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
        private List<RoomItem> items;
        private List<ICollidable> collidables;

        public Room(int id, string name, int width, int height)
        {
            Id = id;
            Name = name;
            Width = width;
            Height = height;

            blocks = new List<IBlock>();
            enemies = new List<Enemy>();
            items = new List<RoomItem>();
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

        public void AddItem(RoomItem item)
        {
            items.Add(item);
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

        public IEnumerable<RoomItem> GetItems()
        {
            return items;
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
            foreach (var item in items)
            {
                item.Update(gameTime);
            }

            // Check item collection (simple collision with player)
            if (player != null)
            {
                Rectangle playerBounds = new Rectangle((int)player.Position.X, (int)player.Position.Y, 18, 42);
                foreach (var item in items)
                {
                    if (!item.IsCollected && item.GetBoundingBox().Intersects(playerBounds))
                    {
                        // Mark item as collected (you can add effects/score here)
                        item.IsCollected = true;

                        // If it's a heart, heal the player
                        if (item.Type == "Heart")
                        {
                            player.CurrentHealth = System.Math.Min(player.CurrentHealth + 20, player.MaxHealth);
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
            foreach (var item in items)
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
                player.Position = new Vector2(100, 300);
                player.CurrentHealth = player.MaxHealth;
            }

            // Reset items
            foreach (var item in items)
            {
                item.IsCollected = false;
            }

            // Reset enemies to their starting positions (if needed)


        }
    }
}