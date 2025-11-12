using System;
using System.Collections.Generic;

namespace Sprint_0.Managers
{
    public static class CooldownManager
    {
        private static readonly Dictionary<string, double> lastActionTime = new();
        private static readonly DateTime gameStartTime = DateTime.Now;
        public static bool CanExecute(string actionKey, double cooldown)
        {
            double currentTime = (DateTime.Now - gameStartTime).TotalSeconds;
            if (!lastActionTime.ContainsKey(actionKey))
            {
                lastActionTime[actionKey] = currentTime;
                return true;
            }

            double lastTime = lastActionTime[actionKey];
            if (currentTime - lastTime >= cooldown)
            {
                lastActionTime[actionKey] = currentTime;
                return true;
            }

            return false;
        }
    }
}