using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Enemies;
using Sprint_0.Items;
using Sprint_0.Player_Namespace;
using System.Collections.Generic;
using System.IO;
using Sprint_0.Interfaces;

namespace Sprint_0.Rooms
{
    public class RoomEntityManager
    {
        private readonly Texture2D linkTextures;
        private readonly Texture2D enemyTextures;
        private readonly Texture2D bossTextures;
        private readonly Texture2D overworldEnemyTextures;
        private readonly Texture2D itemTextures;
        private readonly IController controller;

        private const int TILE_SIZE = 16;

        public RoomEntityManager(Texture2D linkTextures, Texture2D enemyTextures, Texture2D bossTextures,
            Texture2D overworldEnemyTextures, Texture2D itemTextures, IController controller)
        {
            this.linkTextures = linkTextures;
            this.enemyTextures = enemyTextures;
            this.bossTextures = bossTextures;
            this.overworldEnemyTextures = overworldEnemyTextures;
            this.itemTextures = itemTextures;
            this.controller = controller;
        }

        public void LoadEntities(Room room, string entitiesCsvPath)
        {
            // Loads all entities (player, enemies, items) from a CSV file into the given room
            var layout = LoadLayout(entitiesCsvPath);
            for (int y = 0; y < layout.Count; y++)
            {
                for (int x = 0; x < layout[y].Count; x++)
                {
                    string key = layout[y][x];
                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    Vector2 position = new Vector2(x * TILE_SIZE, y * TILE_SIZE);

                    // determine what entity to spawn based on CSV key
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

                        case "k":
                        case "key":
                            CreateKey(room, position);
                            break;

                        case "c":
                        case "candle":
                            CreateCandle(room, position);
                            break;

                        case "bot":
                            CreateEnemy(room, "Bot", position);
                            break;

                        case "stal":
                        case "stalfos":
                            CreateEnemy(room, "Stalfos", position);
                            break;

                        case "wosu":
                            CreateEnemy(room, "Wosu", position);
                            break;

                        case "octo":
                        case "octorok":
                            CreateEnemy(room, "Octorok", position);
                            break;

                        case "bub":
                        case "bubble":
                            CreateEnemy(room, "Bubble", position);
                            break;

                        case "obot":
                        case "overworldbot":
                            CreateEnemy(room, "OverworldBot", position);
                            break;

                        case "oman":
                        case "overworldman":
                            CreateEnemy(room, "OverworldMan", position);
                            break;

                        case "boss":
                        case "horse":
                        case "horsehead":
                            CreateEnemy(room, "HorseHead", position);
                            break;
                    }
                }
            }
        }

        private List<List<string>> LoadLayout(string filePath)
        {
            // Reads a CSV file and returns a 2D list of strings representing entity keys
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
        }

        private void CreateHeart(Room room, Vector2 position)
        {
            var heart = new HeartItem(position, itemTextures);
            room.AddItem(heart);
        }

        private void CreateKey(Room room, Vector2 position)
        {
            var key = new KeyItem(position, itemTextures);
            room.AddItem(key);

        }

        private void CreateCandle(Room room, Vector2 position)
        {
            var candle = new CandleItem(position, itemTextures);
            room.AddItem(candle);
        }

        private void CreateEnemy(Room room, string enemyType, Vector2 position)
        {
            // Creates an enemy based on its type string and adds it to the room
            Enemy enemy = null;

            switch (enemyType)
            {
                case "Bot":
                    enemy = new BotEnemy(enemyTextures, position);
                    break;
                case "Stalfos":
                    enemy = new StalfosEnemy(enemyTextures, position);
                    //enemy.ChangeState(new Sprint_0.EnemyStateMachine.StalfosFallState());
                    break;
                case "Octorok":
                    enemy = new OctorokEnemy(overworldEnemyTextures, enemyTextures, position);
                    break;
                case "Wosu":
                    enemy = new WosuEnemy(enemyTextures, position);
                    break;
                case "Bubble":
                    enemy = new BubbleEnemy(enemyTextures, position);
                    break;
                case "OverworldBot":
                    enemy = new OverworldBotEnemy(overworldEnemyTextures, position);
                    break;
                case "OverworldMan":
                    enemy = new OverworldManEnemy(overworldEnemyTextures, position);
                    break;
                case "HorseHead":
                    enemy = new HorseHeadEnemy(bossTextures, position);
                    break;
            }

            if (enemy != null)
            {
                room.AddEnemy(enemy);
            }
        }
    }
}