using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Blocks;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Items;
using Sprint_0.Player_Namespace;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Sprint_0.Rooms
{
    public class RoomEntityManager
    {
        private readonly Texture2D linkTextures;
        private readonly Texture2D enemyTextures;
        private readonly Texture2D bossTextures;
        private readonly Texture2D overworldEnemyTextures;
        private readonly Texture2D itemTextures;
        private readonly Texture2D blockTextures;
        private readonly IController controller;

        private const int TILE_SIZE = 16;

        public RoomEntityManager(Texture2D linkTextures, Texture2D enemyTextures, Texture2D bossTextures,
            Texture2D overworldEnemyTextures, Texture2D itemTextures, Texture2D blockTextures, IController controller)
        {
            this.linkTextures = linkTextures;
            this.enemyTextures = enemyTextures;
            this.bossTextures = bossTextures;
            this.overworldEnemyTextures = overworldEnemyTextures;
            this.itemTextures = itemTextures;
            this.blockTextures = blockTextures;
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

                        case "t":
                        case "triforce":
                        case "tri":
                            CreateTriforce(room, position);
                            break;

                        case "trap":
                        case "bb":
                            CreateTrapBlock(room, position);
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

                        case "tdbot":
                        case "topdownbot":
                            CreateEnemy(room, "TopDownBot", position);
                            break;

                        case "tdman":
                        case "topdownman":
                            CreateEnemy(room, "TopDownMan", position);
                            break;

                        case "boss":
                        case "horse":
                        case "horsehead":
                            CreateEnemy(room, "HorseHead", position);
                            break;

                        case "dr":
                        case "door":
                            CreateDoor(room, position);
                            break;

                        case "tdk":
                        case "topdownkey":
                            CreateTopDownKey(room, position);
                            break;

                        case "tdd":
                        case "topdowndoor":
                            CreateTopDownDoor(room, position);
                            break;

                        case "trophy":
                            CreateTrophy(room, position);
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

        private void CreateDoor(Room room, Vector2 position)
        {
            var door = new LockedDoor(position, itemTextures);
            room.AddBlock(door);
        }

        private void CreateTopDownDoor(Room room, Vector2 position)
        {
            var door = new TopDownDoor(position, itemTextures);
            room.AddBlock(door);
            Debug.WriteLine($"[RoomEntityManager] Added TopDownDoor in room {room?.Id} at {position}");
        }

        private void CreateTopDownKey(Room room, Vector2 position)
        {
            var key = new TopDownKeyItem(position, itemTextures);
            room.AddItem(key);
            Debug.WriteLine($"[RoomEntityManager] Added TopDownKey in room {room?.Id} at {position}");
        }

        private void CreateTrophy(Room room, Vector2 position)
        {
            var trophy = new TrophyItem(position, itemTextures);
            room.AddItem(trophy);
            Debug.WriteLine($"[RoomEntityManager] Added Trophy in room {room?.Id} at {position}");
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

        private void CreateTriforce(Room room, Vector2 position)
        {
            var triforce = new TriforceItem(position, itemTextures);
            room.AddItem(triforce);
        }

        private void CreateTrapBlock(Room room, Vector2 position)
        {
            var trapBlock = new TrapBlock(position, blockTextures);
            room.AddBlock(trapBlock);
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
                case "TopDownBot":
                    enemy = new TopDownBotEnemy(overworldEnemyTextures, position);
                    break;
                case "TopDownMan":
                    enemy = new TopDownManEnemy(overworldEnemyTextures, position);
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