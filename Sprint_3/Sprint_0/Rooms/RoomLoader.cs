using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
namespace Sprint_0.Rooms
{
    public class RoomLoader(Game1 game, RoomEntityManager entities)
    {
        private readonly Game1 game = game;
        private readonly RoomEntityManager entities = entities;

        public (List<Room> rooms, int startIndex) LoadAllRooms()
        {
            var rooms = CreateRooms();
            PopulateTiles(rooms);
            PopulateEntities(rooms);
            return (rooms, 0);
        }

        private static List<Room> CreateRooms()
        {
            return
            [
                new Room(1, "Palace Exterior", 1024, 480),
                new Room(2, "Dungeon Room 1", 1024, 480),
                new Room(3, "Dungeon Room 2", 1024, 480),
                new Room(4, "Dungeon Room 3", 1024, 480),
                new Room(5, "Dungeon Room 4", 1024, 480),
                new Room(6, "Dungeon Room 5", 1024, 480),
                new Room(7, "Dungeon Room 6", 1024, 480),
                new Room(8, "Dungeon Room 7", 1024, 480)
            ];
        }

        private void PopulateTiles(List<Room> rooms)
        {
            AddCsv(rooms[0], "Content/palace_exterior.csv", game.BlockTextures);
            AddCsv(rooms[1], "Content/Dungeon Room 1.csv", game.BlockTextures);
            AddCsv(rooms[2], "Content/Dungeon Room 2.csv", game.BlockTextures);
            AddCsv(rooms[3], "Content/Dungeon Room 3.csv", game.BlockTextures);
            AddCsv(rooms[4], "Content/Dungeon Room 4.csv", game.BlockTextures);
            AddCsv(rooms[5], "Content/Dungeon Room 5.csv", game.BlockTextures);
            AddCsv(rooms[6], "Content/Dungeon Room 6.csv", game.BlockTextures);
            AddCsv(rooms[7], "Content/Dungeon Room 7.csv", game.BlockTextures);
        }

        private void PopulateEntities(List<Room> rooms)
        {
            entities.LoadEntities(rooms[0], "Content/palace_exterior_entities.csv");
            entities.LoadEntities(rooms[1], "Content/Dungeon Room 1 entities.csv");
            entities.LoadEntities(rooms[2], "Content/Dungeon Room 2 entities.csv");
            entities.LoadEntities(rooms[3], "Content/Dungeon Room 3 entities.csv");
            entities.LoadEntities(rooms[4], "Content/Dungeon Room 4 entities.csv");
            entities.LoadEntities(rooms[5], "Content/Dungeon Room 5 entities.csv");
            entities.LoadEntities(rooms[6], "Content/Dungeon Room 6 entities.csv");
            entities.LoadEntities(rooms[7], "Content/Dungeon Room 7 entities.csv");
            }

        private static void AddCsv(Room room, string csvPath, Texture2D sheet)
        {
            var builder = new RoomBuilder(csvPath, sheet);
            builder.PopulateRoom(room);
        }
    }
}