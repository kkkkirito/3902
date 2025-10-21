using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0.Interfaces
{
    public interface IEnemyProjectile : ICollidable
    {
        bool IsActive { get; set; }
        int Damage { get; }
    }
}