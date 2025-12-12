using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Sprint_0.Managers
{
    
    public class AudioManager : IAudioManager
    {
        
        private readonly SoundEffect _attackSound;
        private readonly SoundEffect _jumpSound;
        private readonly SoundEffect _hurtSound;
        private readonly SoundEffect _fireballSound;
        private readonly SoundEffect _pickupSound;
        private readonly SoundEffect _runningSound;
        private readonly SoundEffect _beamSound;
        private readonly SoundEffect _deathSound;

        private readonly SoundEffect _enemyDieSound;
        private readonly SoundEffect _enemyAttackSound;

        private readonly SoundEffect _gameoverSound;

        private readonly SoundEffectInstance _bgmInstance;
        private SoundEffectInstance _runningInstance;

        private bool _bgmMuted = false;
        private const float DefaultBgmVolume = 0.5f;
        private const float DefaultSfxVolume = 0.8f;
        private const float RunningVolume = 0.7f;

        public AudioManager(ContentManager content)
        {
            
            _attackSound = content.Load<SoundEffect>("Audio/sword_01");
            _jumpSound = content.Load<SoundEffect>("Audio/jump_01");
            _hurtSound = content.Load<SoundEffect>("Audio/hurt_01");
            _fireballSound = content.Load<SoundEffect>("Audio/fireball_01");
            _pickupSound = content.Load<SoundEffect>("Audio/pickup_01");
            _runningSound = content.Load<SoundEffect>("Audio/running_01");
            _beamSound = content.Load<SoundEffect>("Audio/beam_01");
            _deathSound = content.Load<SoundEffect>("Audio/death_01");

            _enemyDieSound = content.Load<SoundEffect>("Audio/enemyDie_01");
            _enemyAttackSound = content.Load<SoundEffect>("Audio/enemyAttack_01");

            _gameoverSound = content.Load<SoundEffect>("Audio/gameover_01");

            var bgmEffect = content.Load<SoundEffect>("Audio/BGM_01");
            _bgmInstance = bgmEffect.CreateInstance();
            _bgmInstance.Volume = DefaultBgmVolume;
            _bgmInstance.IsLooped = true;
        }

        

        public void PlayAttack() => PlaySound(_attackSound);
        public void PlayJump() => PlaySound(_jumpSound);
        public void PlayHurt() => PlaySound(_hurtSound);
        public void PlayFireball() => PlaySound(_fireballSound);
        public void PlayPickup() => PlaySound(_pickupSound);
        public void PlayBeam() => PlaySound(_beamSound);
        public void PlayDeath() => PlaySound(_deathSound);
        public void PlayEnemyDie() => PlaySound(_enemyDieSound);
        public void PlayEnemyAttack() => PlaySound(_enemyAttackSound);
        public void PlayGameOver() => PlaySound(_gameoverSound);

        public void PlayRunning()
        {
            if (_runningInstance == null)
            {
                _runningInstance = _runningSound.CreateInstance();
                _runningInstance.IsLooped = true;
                _runningInstance.Volume = RunningVolume;
                _runningInstance.Play();
            }
            else if (_runningInstance.State != SoundState.Playing)
            {
                _runningInstance.Play();
            }
        }

        public void StopRunning()
        {
            if (_runningInstance != null &&
                _runningInstance.State == SoundState.Playing)
            {
                _runningInstance.Stop();
            }
        }

        

        public void PlayBgm(float volume = DefaultBgmVolume)
        {
            if (_bgmInstance == null)
                return;

            if (_bgmInstance.State != SoundState.Playing)
            {
                _bgmInstance.Volume = _bgmMuted ? 0f : volume;
                _bgmInstance.Play();
            }
        }

        public void StopBgm()
        {
            if (_bgmInstance != null &&
                _bgmInstance.State == SoundState.Playing)
            {
                _bgmInstance.Stop();
            }
        }

        public void ToggleBgmMute()
        {
            if (_bgmInstance == null)
                return;

            _bgmMuted = !_bgmMuted;
            _bgmInstance.Volume = _bgmMuted ? 0f : DefaultBgmVolume;
        }

        

        private void PlaySound(SoundEffect sound, float volume = DefaultSfxVolume)
        {
            if (sound == null)
                return;

            var instance = sound.CreateInstance();
            instance.Volume = volume;
            instance.Play();
        }
    }
}