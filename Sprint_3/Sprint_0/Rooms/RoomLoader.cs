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
                new Room(1, "Palace Exterior", 1024, 480){AmbientLightLevel = 0f},
                new Room(2, "Dungeon Room 1", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(3, "Dungeon Room 2", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(4, "Dungeon Room 3", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(5, "Dungeon Room 4", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(6, "Dungeon Room 5", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(7, "Dungeon Room 6", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(8, "Dungeon Room 7", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(9, "Dungeon Room 8", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(10, "Dungeon Room 9", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(11, "Dungeon Room 10", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(12, "Dungeon Room 11", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(13, "Dungeon Room 12", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(14, "Dungeon Room 13", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(15, "Dungeon Room 14", 1024, 480){AmbientLightLevel = 0.02f},
                new Room(16, "Maze Room 1", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(17, "Maze Room 2", 1024, 480){AmbientLightLevel = 0.45f},
                new Room(18, "Maze Room 3", 1024, 480){AmbientLightLevel = 0.45f}

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
            AddCsv(rooms[8], "Content/Dungeon Room 8.csv", game.BlockTextures);
            AddCsv(rooms[9], "Content/Dungeon Room 9.csv", game.BlockTextures);
            AddCsv(rooms[10], "Content/Dungeon Room 10.csv", game.BlockTextures);
            AddCsv(rooms[11], "Content/Dungeon Room 11.csv", game.BlockTextures);
            AddCsv(rooms[12], "Content/Dungeon Room 12.csv", game.BlockTextures);
            AddCsv(rooms[13], "Content/Dungeon Room 13.csv", game.BlockTextures);
            AddCsv(rooms[14], "Content/Dungeon Room 14.csv", game.BlockTextures);
            AddCsv(rooms[15], "Content/Maze Room 1.csv", game.BlockTextures);
            AddCsv(rooms[16], "Content/Maze Room 2.csv", game.BlockTextures);
            AddCsv(rooms[17], "Content/Maze Room 3.csv", game.BlockTextures);


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
            entities.LoadEntities(rooms[8], "Content/Dungeon Room 8 entities.csv");
            entities.LoadEntities(rooms[9], "Content/Dungeon Room 9 entities.csv");
            entities.LoadEntities(rooms[10], "Content/Dungeon Room 10 entities.csv");
            entities.LoadEntities(rooms[11], "Content/Dungeon Room 11 entities.csv");
            entities.LoadEntities(rooms[12], "Content/Dungeon Room 12 entities.csv");
            entities.LoadEntities(rooms[13], "Content/Dungeon Room 13 entities.csv");
            entities.LoadEntities(rooms[14], "Content/Dungeon Room 14 entities.csv");
            entities.LoadEntities(rooms[15], "Content/Maze Room 1 entities.csv");
            entities.LoadEntities(rooms[16], "Content/Maze Room 2 entities.csv");
            entities.LoadEntities(rooms[17], "Content/Maze Room 3 entities.csv");
        }

        private static void AddCsv(Room room, string csvPath, Texture2D sheet)
        {
            var builder = new RoomBuilder(csvPath, sheet);
            builder.PopulateRoom(room);
        }
    }
}