
using Microsoft.Xna.Framework;
using Sprint_0.Interfaces;

namespace Sprint_0.Collision_System
{
    public readonly struct CollisionInfo
    {
        public CollisionInfo(ICollidable a, ICollidable b, CollisionDirection dir, Vector2 mtv)
        {
            A = a; B = b; Direction = dir; MinimumTranslationVector = mtv;
        }
        public ICollidable A { get; }
        public ICollidable B { get; }
        public CollisionDirection Direction { get; }
        // Push 'A' out of 'B' by this vector
        public Vector2 MinimumTranslationVector { get; }
        public bool HasCollision => Direction != CollisionDirection.None;
    }
}
