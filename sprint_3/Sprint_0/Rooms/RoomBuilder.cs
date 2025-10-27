// Noah Ruppert - 10/17/2025
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Blocks;
using System;
using System.Collections.Generic;
using System.IO;
using Sprint_0.Interfaces;
using Microsoft.Xna.Framework.Content;
using Sprint_0.Collision_System;

namespace Sprint_0.Rooms
{
    public class RoomBuilder 
    {
        private const int TILE = 16;
        private List<List<string>> _layout; //CSV keys
        private List<List<IBlock>> _blocks;//Visuals

        public RoomBuilder(string csvPath, Texture2D spriteSheet)
        {
            _layout = LoadLayout(csvPath);
            _blocks = BuildVisualBlocks(_layout, spriteSheet);
        }

        public void PopulateRoom(Room room)
        {

            // Adds all blocks from this builder to a given Room object
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

            bool[,] solids = BuildSolidityGrid(_layout);
            var merged = MergeSolidsToRects(solids, TILE);

            room.ReplaceStaticBlockColliders(merged);
        }


        private static List<List<String>> LoadLayout(string filePath)
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

        private static List<List<IBlock>> BuildVisualBlocks(List<List<string>> layout, Texture2D spriteSheet)
        {
            var blockSprites = SpriteFactory.CreateBlockTextures(spriteSheet);
            var rows = new List<List<IBlock>>(layout.Count);

            for (int y = 0; y < layout.Count; y++)
            {
                var row = new List<IBlock>(layout[y].Count);
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
                        var position = new Vector2(x * TILE, y * TILE);
                        bool isSolid = BlockSolidity.TryGetValue(key, out bool solid) && solid;
                        row.Add(new Block(sprite, position, isSolid));
                    }
                    else
                    {
                        Console.WriteLine($"[RoomBuilder] Unknown block key '{key}' at ({x},{y})");
                        row.Add(null);
                    }
                }
                rows.Add(row);
            }
            return rows;
        }

        private static bool[,] BuildSolidityGrid(List<List<string>> layout)
        {
            int height = layout.Count;
            int width = layout[0].Count;
            var grid = new bool[height, width];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    string key = layout[y][x].ToLowerInvariant();
                    grid[y, x] = BlockSolidity.TryGetValue(key, out bool solid) && solid;
                }
            return grid;
        }

        public static List<IStaticCollider> MergeSolidsToRects(bool[,] solid, int tile)
        {
            int h = solid.GetLength(0);
            int w = solid.GetLength(1);


            var runs = new List<(int y, int x0, int x1)>();
            for (int y = 0; y < h; y++)
            {
                int x = 0;
                while (x < w)
                {
                    if (!solid[y, x]) { x++; continue; }
                    int x0 = x;
                    while (x + 1 < w && solid[y, x + 1]) x++;
                    int x1 = x;
                    runs.Add((y, x0, x1));
                    x++;
                }
            }

            var colliders = new List<IStaticCollider>();
            int i = 0;
            while (i < runs.Count)
            {
                var (y0, rx0, rx1) = runs[i];
                int y1 = y0;
                int j = i + 1;

                while (j < runs.Count && runs[j].x0 == rx0 && runs[j].x1 == rx1 && runs[j].y == y1 + 1)
                {
                    y1 = runs[j].y;
                    j++;
                }


                int px = rx0 * tile;
                int py = y0 * tile;
                int pw = (rx1 - rx0 + 1) * tile;
                int ph = (y1 - y0 + 1) * tile;

                colliders.Add(new StaticRectangleCollider(new Rectangle(px, py, pw, ph)));

                i = j;
            }
            return colliders;
        }

        public List<ICollidable> GetCollidables()
        {
            // Returns all collidable blocks from the layout
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

        private static readonly Dictionary<string, bool> BlockSolidity = new()
        {
            // Blocks are solid if true
            { "ebr", true }, { "sk", false }, { "febr", true },  
            { "gr", true }, { "etcol", false }, { "col", false },
            { "est", false }, { "esb", false }, { "tcol", false },  
            { "st", false }, { "sb", false }, { "eb", false }, 
            { "brbg", false }, { "c1", false }, { "c2", false }, 
            { "c3", false }, { "tw", false }, { "bw", false },
            { "fl", false }, { "flb", false }, { "t1", false }, 
            { "t2", false }, { "t3", false }, { "t4", false },
            { "t5", false }, { "t6", false }, { "t7", false }, 
            { "t8", false }, { "t9", false }, { "t10", false }, 
            { "t11", false }, { "t12", false }, { "gh", false },
            { "br", true }, { "pl", true }, { "pbr", true }, 
            { "cbr", true }, { "bb2", true }, { "bb3", true }, 
            { "bb4", true }, { "tla", false }, { "la", false }
        };
    }
}