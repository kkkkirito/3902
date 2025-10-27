//Dillon Brigode AU 25
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;

namespace Sprint_0.Collision_System
{
    public class CollisionSystem
    {
        private readonly CollisionDetectionV2 _detector = new();
        private readonly CommandProvider _provider = new();
        private readonly List<ICollidable> _collidables = new();

        public CommandProvider Provider => _provider;

        public void RegisterCollidables(IEnumerable<ICollidable> collidables)
        {
            _collidables.Clear();
            _collidables.AddRange(collidables);
        }

        public void Update()
        {
            for (int i = 0; i < _collidables.Count; i++)
            for (int j = i + 1; j < _collidables.Count; j++)
            {
                var a = _collidables[i];
                var b = _collidables[j];

                var info = _detector.GetInfo(a, b);
                if (!info.HasCollision) continue;

                var cmd = _provider.Resolve(a, b);
                cmd?.Execute(info);
            }
        }
    }
}