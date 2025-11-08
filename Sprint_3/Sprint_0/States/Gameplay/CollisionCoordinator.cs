//Dillon Brigdode AU25 part of GameplayState refactor
using System.Collections.Generic;
using Sprint_0.Rooms;
using Sprint_0.Collision_System;
using Sprint_0.Interfaces;

namespace Sprint_0.States.Gameplay
{
    //responsible for coordinating collision detection and response single responsibility.
    public sealed class CollisionCoordinator(CollisionSystem system)
    {
        private readonly CollisionSystem _system = system;

        public void Step(Room room, IPlayer player, IProjectileManager projectiles)
        {
            if (_system == null || room == null) return;

            var all = new List<ICollidable>(64);

            if (player != null)
                all.AddRange(player.GetCollidables());

            var roomCollidables = room.GetCollidables();
            if (roomCollidables != null)
                all.AddRange(roomCollidables);

            if (projectiles != null)
                foreach (var c in projectiles.GetCollidables())
                    all.Add(c);

            _system.RegisterCollidables(all);
            _system.Update();
        }
    }
}
