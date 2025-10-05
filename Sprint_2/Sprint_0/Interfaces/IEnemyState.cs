using Microsoft.Xna.Framework;

namespace Sprint_0.EnemyStateMachine
{
    public interface IEnemyState
    {
        void Start(Enemy enemy);
        void Update(Enemy enemy, GameTime gameTime);
        void Done(Enemy enemy);
    }
}
