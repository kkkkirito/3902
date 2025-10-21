using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Blocks;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;

using Sprint_0;

namespace Sprint_0.Rooms
{
    public class RoomLoader
    {
        private readonly ContentManager content;
        private readonly BlockFactory blockFactory;
        private readonly Texture2D enemyTextures;
        private readonly Texture2D overworldEnemyTextures;
        private readonly Texture2D itemTextures;
        private readonly Texture2D linkTextures;

        public RoomLoader(ContentManager content, BlockFactory blockFactory,
            Texture2D enemyTextures, Texture2D overworldEnemyTextures,
            Texture2D itemTextures, Texture2D linkTextures)
        {
            this.content = content;
            this.blockFactory = blockFactory;
            this.enemyTextures = enemyTextures;
            this.overworldEnemyTextures = overworldEnemyTextures;
            this.itemTextures = itemTextures;
            this.linkTextures = linkTextures;
        }

        public Room LoadRoom(string xmlFilePath, IController controller)
        {
            XDocument doc = XDocument.Load(xmlFilePath);
            XElement roomElement = doc.Root;

            // Get room attributes
            var idAttr = roomElement.Attribute("id");
            var nameAttr = roomElement.Attribute("name");
            var widthAttr = roomElement.Attribute("width");
            var heightAttr = roomElement.Attribute("height");

            int roomId = idAttr != null ? int.Parse(idAttr.Value) : 0;
            string roomName = nameAttr != null ? nameAttr.Value : "Unnamed Room";
            int width = widthAttr != null ? int.Parse(widthAttr.Value) : 800;
            int height = heightAttr != null ? int.Parse(heightAttr.Value) : 480;

            Room room = new Room(roomId, roomName, width, height);

            // Load player
            XElement playerElement = roomElement.Element("Player");
            if (playerElement != null)
            {
                var xAttr = playerElement.Attribute("x");
                var yAttr = playerElement.Attribute("y");
                float x = xAttr != null ? float.Parse(xAttr.Value) : 100;
                float y = yAttr != null ? float.Parse(yAttr.Value) : 100;
                Player player = new Player(linkTextures, new Vector2(x, y), controller);
                room.SetPlayer(player);
            }

            // Load blocks
            XElement blocksElement = roomElement.Element("Blocks");
            if (blocksElement != null)
            {
                foreach (XElement blockElement in blocksElement.Elements("Block"))
                {
                    var indexAttr = blockElement.Attribute("index");
                    var xAttr = blockElement.Attribute("x");
                    var yAttr = blockElement.Attribute("y");
                    var solidAttr = blockElement.Attribute("solid");

                    int index = indexAttr != null ? int.Parse(indexAttr.Value) : 0;
                    float x = xAttr != null ? float.Parse(xAttr.Value) : 0;
                    float y = yAttr != null ? float.Parse(yAttr.Value) : 0;
                    bool isSolid = solidAttr != null ? bool.Parse(solidAttr.Value) : true;

                    IBlock block = blockFactory.CreateByIndex(index, new Vector2(x, y), isSolid);
                    if (block != null)
                    {
                        room.AddBlock(block);
                    }
                }
            }

            // Load enemies
            XElement enemiesElement = roomElement.Element("Enemies");
            if (enemiesElement != null)
            {
                foreach (XElement enemyElement in enemiesElement.Elements("Enemy"))
                {
                    var typeAttr = enemyElement.Attribute("type");
                    var xAttr = enemyElement.Attribute("x");
                    var yAttr = enemyElement.Attribute("y");

                    string type = typeAttr != null ? typeAttr.Value : "Bot";
                    float x = xAttr != null ? float.Parse(xAttr.Value) : 0;
                    float y = yAttr != null ? float.Parse(yAttr.Value) : 0;

                    Enemy enemy = CreateEnemy(type, new Vector2(x, y));
                    if (enemy != null)
                    {
                        room.AddEnemy(enemy);
                    }
                }
            }

            // Load items
            XElement itemsElement = roomElement.Element("Items");
            if (itemsElement != null)
            {
                foreach (XElement itemElement in itemsElement.Elements("Item"))
                {
                    var typeAttr = itemElement.Attribute("type");
                    var xAttr = itemElement.Attribute("x");
                    var yAttr = itemElement.Attribute("y");

                    string type = typeAttr != null ? typeAttr.Value : "Heart";
                    float x = xAttr != null ? float.Parse(xAttr.Value) : 0;
                    float y = yAttr != null ? float.Parse(yAttr.Value) : 0;

                    RoomItem item = CreateItem(type, new Vector2(x, y));
                    if (item != null)
                    {
                        room.AddItem(item);
                    }
                }
            }

            return room;
        }

        private Enemy CreateEnemy(string type, Vector2 position)
        {
            switch (type)
            {
                case "Bot":
                    return new BotEnemy(enemyTextures, position);
                case "Stalfos":
                    return new StalfosEnemy(enemyTextures, position);
                case "Octorok":
                    return new OctorokEnemy(overworldEnemyTextures, enemyTextures, position);
                case "OverworldBot":
                    return new OverworldBotEnemy(overworldEnemyTextures, position);
                case "OverworldMan":
                    return new OverworldManEnemy(overworldEnemyTextures, position);
                default:
                    return new BotEnemy(enemyTextures, position);
            }
        }

        private RoomItem CreateItem(string type, Vector2 position)
        {
            // Create items based on the animations from SpriteFactory
            var itemAnimations = SpriteFactory.CreateItemAnimations(itemTextures);

            if (itemAnimations.ContainsKey(type))
            {
                // Use internal constructor
                return new RoomItem(type, itemAnimations[type], position);
            }

            // Default to Heart if type not found
            if (itemAnimations.ContainsKey("Heart"))
            {
                return new RoomItem("Heart", itemAnimations["Heart"], position);
            }

            return null;
        }
    }

    // Simple item class for room items - follows encapsulation pattern
    public class RoomItem
    {
        public string Type { get; }
        public Vector2 Position { get; set; }
        public bool IsCollected { get; set; }

        private Animation animation;  // Hidden implementation detail

        internal RoomItem(string type, Animation animation, Vector2 position)  // internal constructor
        {
            Type = type;
            this.animation = animation;
            Position = position;
            IsCollected = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsCollected)
            {
                animation?.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
            {
                animation?.Draw(spriteBatch, Position, SpriteEffects.None);
            }
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        }
    }
}
