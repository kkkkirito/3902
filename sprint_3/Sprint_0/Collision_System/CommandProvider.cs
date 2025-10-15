using Sprint_0.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0
{
    public class CommandProvider : ICommandProvider
    {
        private readonly Dictionary<(ICollidable, ICollidable), ICommand> _collisionResponse = new();

        public ICommand Resolve(ICollidable a, ICollidable b)
        {
            ICommand command;
            if (_collisionResponse.TryGetValue((a, b), out command)) return command;
            //Build swap method 
            if (_collisionResponse.TryGetValue((b, a), out command)) return command;
            return null;
        }
        public void Register((ICollidable, ICollidable) pair, ICommand command)
        {
            _collisionResponse[pair] = command;
        }

    }
}
