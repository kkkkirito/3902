using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Sprint_0.Interfaces;

public enum ProjectileType { SwordBeam, Fireball }

public interface IProjectileManager
{
    bool TrySpawnSwordBeam(IPlayer player);
    bool TrySpawnFireball(IPlayer player);
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    IEnumerable<ICollidable> GetCollidables();
    IEnumerable<ILightSource> GetActiveLightSources();
}
