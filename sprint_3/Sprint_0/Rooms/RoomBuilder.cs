// Noah Ruppert - 10/17/2025
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Blocks;
using System;
using System.Collections.Generic;
using System.IO;
using Sprint_0.Interfaces;

namespace Sprint_0.Rooms
{
    public class RoomBuilder 
    {
        private List<List<IBlock>> _blocks;

        public RoomBuilder(string csvPath, Texture2D spriteSheet)
        {
            _blocks = BuildRoom(csvPath, spriteSheet);
        }

        public void PopulateRoom(Room room)
        {
            foreach (var row in _blocks)
            {
                foreach (var block in row)
                {
                    if (block != null)
                    {
                        room.AddBlock(block);
                    }
                }
            }
        }

        private List<List<IBlock>> BuildRoom(string csvPath, Texture2D spriteSheet)
        {
            var layout = LoadLayout(csvPath);
            var blockSprites = SpriteFactory.CreateBlockTextures(spriteSheet);
            var room = new List<List<IBlock>>();

            for (int y = 0; y < layout.Count; y++)
            {
                var row = new List<IBlock>();
                for (int x = 0; x < layout[y].Count; x++)
                {
                    string key = layout[y][x];
                    if (string.IsNullOrWhiteSpace(key) || key == "sk") 
                    {
                        row.Add(null);
                        continue;
                    }

                    if (blockSprites.TryGetValue(key, out var sprite))
                    {
                        var position = new Vector2(x * 16, y * 16);
                        bool isSolid = key != "cl1" && key != "cl2" && key != "cl3" && key != "ecol"; 
                        var block = new Block(sprite, position, isSolid);
                        row.Add(block);
                    }
                    else
                    {
                        Console.WriteLine($"[RoomBuilder] Unknown block key '{key}' at ({x},{y})");
                        row.Add(null);
                    }
                }
                room.Add(row);
            }
            return room;
        }

        private List<List<string>> LoadLayout(string filePath)
        {
            var layout = new List<List<string>>();
            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var row = new List<string>();
                foreach (var cell in line.Split(','))
                    row.Add(cell.Trim());
                layout.Add(row);
            }
            return layout;
        }

        public List<ICollidable> GetCollidables()
        {
            var collidables = new List<ICollidable>();
            foreach (var row in _blocks)
            {
                foreach (var block in row)
                {
                    if (block != null && block is ICollidable collidable)
                    {
                        collidables.Add(collidable);
                    }
                }
            }
            return collidables;
        }
    }
}