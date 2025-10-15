using Microsoft.Xna.Framework;
using Sprint_0.Enemies;

namespace Sprint_0.Interfaces
{
    public interface IEnemyState
    {
        void Start(Enemy enemy);
        void Update(Enemy enemy, GameTime gameTime);
        void Done(Enemy enemy);
    }
}
