using RQ.Model.Audio;
using System;
using UnityEngine;
using RQ.Entity.Components;
using RQ.Audio;
using RQ.Entity.AtomAction;
using RQ.Physics.Components;

namespace RQ.AI.AtomAction.Action
{
    public enum EntitySound
    {
        Damaged = 0,
        Killed = 1
    }
    public enum PlayOnEnum
    {
        Self = 0,
        MusicTrack = 1,
        SoundEffectTrack = 2
    }

    [Serializable]
    public class PlayEntitySound2Atom : AtomActionBase
    {
        public PlayOnEnum PlayOn;
        public bool OneShot = true;
        public bool Looping = false;
        [Range(0, 1)]
        public float _volume = 1f;
        public EntitySound EntitySound;
        protected AudioComponent _audioComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_audioComponent == null)
                _audioComponent = entity.Components.GetComponent<AudioComponent>();
            var damageComponent = entity.Components.GetComponent<DamageComponent>();
            AudioClip audioClip = null;
            switch (EntitySound)
            {
                case EntitySound.Damaged:
                    audioClip = damageComponent.GetDamageSound();
                    break;
                case EntitySound.Killed:
                    audioClip = damageComponent.GetKillSound();
                    break;
            }
            var audioComponent = GetAudioReceiver();

            if (OneShot)
            {
                PlayOneShot(audioClip, audioComponent);
            }
            else
            {
                PlayAudio(audioClip, audioComponent);
            }
        }

        public void PlayOneShot(AudioClip audioClip, AudioComponent audioComponent)
        {
            var soundInfo = new PlaySoundData()
            {
                AudioClip = audioClip,
                PlaySound = true,
                Volume = _volume
            };

            audioComponent.PlayOneShot(soundInfo);
        }

        public void PlayAudio(AudioClip audioClip, AudioComponent audioComponent)
        {
            var soundInfo = new AudioClipInfo()
            {
                AudioClip = audioClip,
                Loop = Looping,
                ClipPosition = 0,
                ForcePlay = true,
                Volume = _volume
            };

            audioComponent.PlayAudioClip(soundInfo);
        }

        private AudioComponent GetAudioReceiver()
        {
            switch (PlayOn)
            {
                case PlayOnEnum.MusicTrack:
                    return GameController.Instance.GetMusicTrack();
                case PlayOnEnum.SoundEffectTrack:
                    return GameController.Instance.GetSoundEffectTrack();
                default:
                    return _audioComponent;
            }
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }
    }
}
