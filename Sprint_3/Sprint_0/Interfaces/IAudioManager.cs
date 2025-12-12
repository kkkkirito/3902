using System;

namespace Sprint_0.Managers
{
    public interface IAudioManager
    {
        
        void PlayAttack();
        void PlayJump();
        void PlayHurt();
        void PlayFireball();
        void PlayPickup();
        void PlayRunning();
        void StopRunning();
        void PlayBeam();
        void PlayDeath();

        void PlayEnemyDie();
        void PlayEnemyAttack();

        void PlayGameOver();

        // Background music controls
        void PlayBgm(float volume = 0.5f);
        void StopBgm();
        void ToggleBgmMute();
    }
}