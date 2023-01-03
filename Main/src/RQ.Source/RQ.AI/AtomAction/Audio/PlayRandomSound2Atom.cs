using RQ.Model.Audio;
using System;
using UnityEngine;
using RQ.Entity.Components;
using RQ.Audio;
using RQ.Entity.AtomAction;

namespace RQ.AI.AtomAction.Audio
{
    public enum PlayOnEnum
    {
        Self = 0,
        MusicTrack = 1,
        SoundEffectTrack = 2
    }

    [Serializable]
    public class PlayRandomSound2Atom : AtomActionBase
    {
        public PlayOnEnum PlayOn;
        //public bool _playOnMusicTrack;
        //public bool _playOnSoundEffectTrack;
        [Range(0, 1)]
        public float _volume = 1f;
        public bool OneShot = true;
        public bool Looping = false;

        public AudioClip[] _audioClips;
        protected AudioComponent _audioComponent;
        //[SerializeField]
        //public PlaySoundInfo _soundInfo = null;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_audioComponent == null)
                _audioComponent = entity.Components.GetComponent<AudioComponent>();

            if (_audioClips.Length == 0)
                return;

            var audioComponent = GetAudioReceiver();
            if (audioComponent == null)
                return;
            
            var currentClipIndex = UnityEngine.Random.Range(0, _audioClips.Length - 1);
            if (OneShot)
            {
                PlayOneShot(_audioClips[currentClipIndex], audioComponent);
            }
            else
            {
                PlayAudio(_audioClips[currentClipIndex], audioComponent);
            }
        }

        public void PlayOneShot(AudioClip audioClip, AudioComponent audioComponent)
        {
            var soundInfo = new PlaySoundData()
            {
                AudioClip = audioClip,
                PlaySound = true,
                //PlayOnMusicTrack = _playOnMusicTrack,
                //PlayAsOneShot = OneShot,
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
                //PlayOnMusicTrack = _playOnMusicTrack,
                //PlayAsOneShot = OneShot,
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
