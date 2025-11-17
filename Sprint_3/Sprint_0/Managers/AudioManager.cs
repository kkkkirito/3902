using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Sprint_0.Managers
{
    public static class AudioManager
    {
        public static SoundEffect AttackSound;
        public static SoundEffect JumpSound;
        public static SoundEffect HurtSound;
        public static SoundEffect FireballSound;
        public static SoundEffect PickupSound;
        public static SoundEffect RunningSound;
        public static SoundEffect BeamSound;
        public static SoundEffect DeathSound;

        public static SoundEffect EnemyDieSound;
        public static SoundEffect EnemyAttackSound;

        public static SoundEffect GameoverSound;

        private static SoundEffectInstance _runningInstance;
        public static SoundEffectInstance BgmInstance;
        private static bool bgmMuted = false;

        private static double bgmDuration = 96.0;
        private static DateTime bgmStartTime;
        public static void Load(ContentManager content)
        {
            AttackSound = content.Load<SoundEffect>("Audio/sword_01");
            JumpSound = content.Load<SoundEffect>("Audio/jump_01");
            HurtSound = content.Load<SoundEffect>("Audio/hurt_01");
            FireballSound = content.Load<SoundEffect>("Audio/fireball_01");
            PickupSound = content.Load<SoundEffect>("Audio/pickup_01");
            RunningSound = content.Load<SoundEffect>("Audio/running_01");
            BeamSound = content.Load<SoundEffect>("Audio/beam_01");
            DeathSound = content.Load<SoundEffect>("Audio/death_01");

            EnemyDieSound = content.Load<SoundEffect>("Audio/enemyDie_01");
            EnemyAttackSound = content.Load<SoundEffect>("Audio/enemyAttack_01");

            GameoverSound = content.Load<SoundEffect>("Audio/gameover_01");

            var bgmEffect = content.Load<SoundEffect>("Audio/BGM_01");
            BgmInstance = bgmEffect.CreateInstance();
            BgmInstance.Volume = 0.5f;
            BgmInstance.IsLooped = true;
        }
        public static void PlayBgm(float volume = 0.5f)
        {
            if (BgmInstance.State != SoundState.Playing)
            {
                BgmInstance.Play();
                bgmStartTime = DateTime.Now;
            }
        }

        public static void StopBgm()
        {
            if (BgmInstance.State == SoundState.Playing)
                BgmInstance.Stop();
        }
        public static void ToggleBgmMute()
        {
            if (BgmInstance == null)
                return;

            bgmMuted = !bgmMuted;
            BgmInstance.Volume = bgmMuted ? 0f : 0.5f;
        }

        public static void PlayRunningSound()
        {
            if (_runningInstance == null)
            {
                _runningInstance = RunningSound.CreateInstance();
                _runningInstance.IsLooped = true;
                _runningInstance.Volume = 0.7f;
                _runningInstance.Play();
            }
            else if (_runningInstance.State != SoundState.Playing)
            {
                _runningInstance.Play();
            }
        }

        public static void StopRunningSound()
        {
            if (_runningInstance != null && _runningInstance.State == SoundState.Playing)
                _runningInstance.Stop();
        }

        public static void PlaySound(SoundEffect sound, float volume = 0.8f)
        {
            if (sound == null) return;
            var instance = sound.CreateInstance();
            instance.Volume = volume;
            instance.Play();
        }
    }
}