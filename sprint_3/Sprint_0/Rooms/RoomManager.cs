// Rooms/RoomManager.cs - 完全替换
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Enemies;
using Sprint_0.Items;
using Sprint_0.Player_Namespace;

namespace Sprint_0.Rooms
{
    public class RoomManager
    {
        private readonly Texture2D linkTextures;
        private readonly Texture2D enemyTextures;
        private readonly Texture2D overworldEnemyTextures;
        private readonly Texture2D itemTextures;
        private readonly IController controller;

        private const int TILE_SIZE = 16;

        public RoomManager(Texture2D linkTextures, Texture2D enemyTextures,
            Texture2D overworldEnemyTextures, Texture2D itemTextures, IController controller)
        {
            this.linkTextures = linkTextures;
            this.enemyTextures = enemyTextures;
            this.overworldEnemyTextures = overworldEnemyTextures;
            this.itemTextures = itemTextures;
            this.controller = controller;
        }

        public void LoadEntities(Room room, string entitiesCsvPath)
        {
            if (!File.Exists(entitiesCsvPath))
            {
                Console.WriteLine($"Entities CSV file not found: {entitiesCsvPath}");
                return;
            }

            var layout = LoadLayout(entitiesCsvPath);

            for (int y = 0; y < layout.Count; y++)
            {
                for (int x = 0; x < layout[y].Count; x++)
                {
                    string key = layout[y][x];
                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    Vector2 position = new Vector2(x * TILE_SIZE, y * TILE_SIZE);

                    switch (key.ToLower())
                    {
                        case "p":
                        case "player":
                            CreatePlayer(room, position);
                            break;

                        case "h":
                        case "heart":
                            CreateHeart(room, position);
                            break;

                        case "bot":
                            CreateEnemy(room, "Bot", position);
                            break;

                        case "stal":
                        case "stalfos":
                            CreateEnemy(room, "Stalfos", position);
                            break;

                        case "octo":
                        case "octorok":
                            CreateEnemy(room, "Octorok", position);
                            break;

                        case "obot":
                        case "overworldbot":
                            CreateEnemy(room, "OverworldBot", position);
                            break;

                        case "oman":
                        case "overworldman":
                            CreateEnemy(room, "OverworldMan", position);
                            break;
                    }
                }
            }
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

        private void CreatePlayer(Room room, Vector2 position)
        {
            var player = new Player(linkTextures, position, controller);
            room.SetPlayer(player);
            Console.WriteLine($"Player created at ({position.X}, {position.Y})");
        }

        private void CreateHeart(Room room, Vector2 position)
        {
            var heart = new HeartItem(position, itemTextures);
            room.AddConsumable(heart);
            Console.WriteLine($"Heart created at ({position.X}, {position.Y})");
        }

        private void CreateEnemy(Room room, string enemyType, Vector2 position)
        {
            Enemy enemy = null;

            switch (enemyType)
            {
                case "Bot":
                    enemy = new BotEnemy(enemyTextures, position);
                    break;
                case "Stalfos":
                    enemy = new StalfosEnemy(enemyTextures, position);
                    break;
                case "Octorok":
                    enemy = new OctorokEnemy(overworldEnemyTextures, enemyTextures, position);
                    break;
                case "OverworldBot":
                    enemy = new OverworldBotEnemy(overworldEnemyTextures, position);
                    break;
                case "OverworldMan":
                    enemy = new OverworldManEnemy(overworldEnemyTextures, position);
                    break;
            }

            if (enemy != null)
            {
                room.AddEnemy(enemy);
                Console.WriteLine($"{enemyType} created at ({position.X}, {position.Y})");
            }
        }
    }
}