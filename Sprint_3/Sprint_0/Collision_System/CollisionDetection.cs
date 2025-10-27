using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Sprint_0.Interfaces;
using Sprint_0.Rooms;

namespace Sprint_0.Collision_System
{
    public class CollisionDetectionV2
    {
        public CollisionInfo GetInfo(ICollidable a, ICollidable b)
        {
            var ra = a.BoundingBox; var rb = b.BoundingBox;
            if (!ra.Intersects(rb)) return new CollisionInfo(a, b, CollisionDirection.None, Vector2.Zero);

            float dx = ra.Center.X - rb.Center.X;
            float dy = ra.Center.Y - rb.Center.Y;

            float overlapX = (ra.Width / 2f + rb.Width / 2f) - Math.Abs(dx);
            float overlapY = (ra.Height / 2f + rb.Height / 2f) - Math.Abs(dy);
            const float StepTolerance = 3f;
            

            bool nearFloor = ra.Bottom <= rb.Top + StepTolerance && ra.Bottom >= rb.Top - StepTolerance;
            if(nearFloor)
            {
                var mtv = new Vector2(0f, rb.Top - ra.Bottom);
                return new CollisionInfo(a, b, CollisionDirection.Top, mtv);
            }

            if (overlapX < overlapY)
            {
                var dir = dx > 0 ? CollisionDirection.Right : CollisionDirection.Left;
                var mtv = new Vector2(dx > 0 ? overlapX : -overlapX, 0f);
                return new CollisionInfo(a, b, dir, mtv);
            }
            else
            {
                var dir = dy > 0 ? CollisionDirection.Bottom : CollisionDirection.Top;
                var mtv = new Vector2(0f, dy > 0 ? overlapY : -overlapY);
                return new CollisionInfo(a, b, dir, mtv);
            }
        }
    }
}

