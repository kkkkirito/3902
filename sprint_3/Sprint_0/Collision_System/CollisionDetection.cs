//Dillon Brigode AU 25
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint_0;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;

public class CollisionDetection
{
    private static CollisionResponse _responder;
    private static List<ICollidable> _collidables;

    private Player _player;

    private Room _room;

    public CollisionDetection(Player player, Room room, Game1 game)
    {
        _player = player;
        _room = room;
        _responder = new CollisionResponse(game);
        _collidables = new List<ICollidable>();
    }
    public Boolean CheckCollision(Rectangle item, Rectangle target)
        {
            return item.Intersects(target);
        }

    public CollisionDirection GetCollisionDirection(Rectangle item, Rectangle target)
    {
        float dx = item.Center.X - target.Center.X;
        float dy = item.Center.Y - target.Center.Y;

        CollisionDirection xDir = dx > 0 ? CollisionDirection.Right : CollisionDirection.Left;
        CollisionDirection yDir = dy > 0 ? CollisionDirection.Bottom : CollisionDirection.Top;

        float gapX = Math.Abs(dx) - (item.Width / 2 + target.Width / 2);
        float gapY = Math.Abs(dy) - (item.Height / 2 + target.Height / 2);

        float overlapX = Math.Max(0, -gapX);
        float overlapY = Math.Max(0, -gapY);

        return overlapX < overlapY ? xDir : yDir;
    }

    public void Update()
    {
        /*
            * Get all collidables that are in the room and collidables that are part of the player.
            *using the dobule for loop in class, bad complexity but protoyping.

            */
        
        _collidables.AddRange(_room.GetCollidables());
        _collidables.AddRange(_player.GetCollidables());
        //at this point we have _collidables from the room(blocks, walls, enemies, etc) and the
        //player(player, projectiles, etc).
        Rectangle itembox, targetbox;

        foreach (ICollidable item in _collidables)
        {
            foreach (ICollidable target in _collidables)
            {
                if (item == target) { continue; }

                itembox = item.BoundingBox;
                targetbox = target.BoundingBox;

                if (CheckCollision(itembox, targetbox))
                {
                    CollisionDirection direction = GetCollisionDirection(itembox, targetbox);
                    //output for testing.
                    Console.WriteLine("{0} hit {1} side of {2}", target, direction, item);
                    //can change based on implementation of collision handling.
                    _responder.HandleCollision(item, target, direction);
                }
            }
        }
    }
}

