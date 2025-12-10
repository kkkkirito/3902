using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using System.Collections.Generic;

namespace Sprint_0.States
{
    public class GameStateManager
    {
        private static GameStateManager instance;
        public static GameStateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameStateManager();
                }
                return instance;
            }
        }

        private Dictionary<string, IGameState> states;
        private IGameState currentState;

        private GameStateManager()
        {
            states = new Dictionary<string, IGameState>();
        }

        public void AddState(string name, IGameState state)
        {
            states[name] = state;
        }

        public void ChangeState(string name)
        {
            if (states.ContainsKey(name))
            {
                currentState?.Exit();
                currentState = states[name];
                currentState.Enter();
            }
        }

        public void Update(GameTime gameTime)
        {
            currentState?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentState?.Draw(spriteBatch);
        }
    }
}
