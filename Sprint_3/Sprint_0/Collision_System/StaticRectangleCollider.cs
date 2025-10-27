using Microsoft.Xna.Framework;
using Sprint_0.Interfaces;

namespace Sprint_0.Collision_System
{
        public class StaticRectangleCollider : IStaticCollider
    {
        public Rectangle BoundingBox { get; private set; }
        public bool IsSolid { get; }
        public StaticRectangleCollider(Rectangle rectangle, bool isSolid = true)
        {
            BoundingBox = rectangle;
            IsSolid = isSolid;
        }
    }
}
