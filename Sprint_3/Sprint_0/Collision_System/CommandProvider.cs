// CommandProvider.cs  (replace content)
using System;
using System.Collections.Generic;
using Sprint_0.Interfaces;
using Sprint_0.Collision_System;

namespace Sprint_0
{
    public class CommandProvider : ICommandProvider
    {
        // Rules are tried in order; first non-null command is used.
        private readonly Dictionary<(Type, Type), ICollisionCommand> _map = new();

        public void Register<TA, TB>(ICollisionCommand command)
            where TA : ICollidable
            where TB : ICollidable
        {
            _map[(typeof(TA), typeof(TB))] = command;
            _map[(typeof(TB), typeof(TA))] = command; // commutative
        }

        public ICollisionCommand Resolve(ICollidable a, ICollidable b)
        {
            var aType = a.GetType();
            var bType = b.GetType();

            foreach (var kv in _map)
            {
                var (keyA, keyB) = kv.Key;
                if (keyA.IsAssignableFrom(aType) && keyB.IsAssignableFrom(bType))
                    return kv.Value;
                if (keyA.IsAssignableFrom(bType) && keyB.IsAssignableFrom(aType))
                    return kv.Value;
            }
            return null;
        }
    

    //left over signature.
        public void Register((ICollidable, ICollidable) pair, ICollisionCommand command) { }
    }
}

