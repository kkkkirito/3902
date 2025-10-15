using System;
using Microsoft.Xna.Framework;
using Sprint_0;
using Sprint_0.Interfaces;
internal class CollisionResponse
{
    private Game1 _game;

/**
response logic in client code, ex methods in player.cs DO NOT DO THIS
encapsulate in playerColisionHandler, player block hander, playerenemyhandler, allcollision handler, etc.
methods = 
void handleblockCollision(block block, Icollision side)
void handelenemyCollision(Ienemy enemy, Icollision side)

assume the class has a reference to player stored in a field. 


Best Design: 
void handleCollision( player player, block block, Icollision side) in the playerblockcollisionhandler class.


most complex and most difficult design:
selecting the right collision handler, given List<IgameObject> objects, you need logic to determine which object is which,
then call the right handler.
allcollisionhandler:
table of use cases in slides
object 1 object 2 side
etc.
*
*/
    public CollisionResponse(Game1 game)
    {
        _game = game;
    }

    internal void HandleCollision(ICollidable item, ICollidable target, CollisionDirection direction)
    {
        throw new NotImplementedException();
    }
}