using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint_0;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;

public class CollisionDetectionV2
{
    public enum CollisionDirection
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }
    public CollisionDirection GetCollisionDirection(Rectangle item, Rectangle target)
    {

        bool colide = item.Intersects(target);

        if (!colide)
        {
            return CollisionDirection.None;
        }

        float dx = (item.Center.X - target.Center.X);
        float dy = (item.Center.Y - target.Center.Y);

        float overlapX = (item.Width / 2f + target.Width / 2f) - Math.Abs(dx);
        float overlapY = (item.Height / 2f + target.Height / 2f) - Math.Abs(dy);

        if (overlapX < overlapY)
        {
            return dx > 0 ? CollisionDirection.Right : CollisionDirection.Left;
        }
        else
        {
            return dy > 0 ? CollisionDirection.Bottom : CollisionDirection.Top;
        }
    }
}

