using RQ.Model.Audio;
using System;
using UnityEngine;
using RQ.Entity.Components;
using RQ.Audio;
using RQ.Messaging;
using RQ.AI;

namespace RQ.Entity.AtomAction.Action
{
    [Obsolete]
    [Serializable]
    public class PlayRandomSoundOneShotAtom : AtomActionBase
    {
        public bool _playOnMusicTrack;
        public bool _playOnSoundEffectTrack;
        [Range(0, 1)]
        public float _volume = 1f;
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

            var currentClipIndex = UnityEngine.Random.Range(0, _audioClips.Length - 1);

            var soundInfo = new PlaySoundData()
            {
                AudioClip = _audioClips[currentClipIndex],
                PlaySound = true,
                //PlayOnMusicTrack = _playOnMusicTrack,
                //PlayAsOneShot = true,
                Volume = _volume
            };
            PlaySound(soundInfo);
        }

        protected virtual void PlaySound(PlaySoundData soundInfo)
        {
            //if (_audioComponent == null)
            //    return;
            //var uniqueId = GetAudioReceiverId(soundInfo);
            //if (uniqueId == null)
            //    return;
            //MessageDispatcher2.Instance.DispatchMsg("PlaySoundInfo", 0f, _entity.UniqueId,
            //    uniqueId, soundInfo);
            MessageDispatcher2.Instance.DispatchMsg("PlaySoundOnSoundEffectTrack", 0f, string.Empty, "Game Controller",
                soundInfo);
        }

        //private string GetAudioReceiverId(PlaySoundData playSoundInfo)
        //{
        //    if (playSoundInfo.PlayOnMusicTrack)
        //        return "Game Controller";
        //    else
        //    {
        //        if (_audioComponent == null)
        //            return null;
        //        return _audioComponent.UniqueId;
        //    }
        //}

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }
    }
}
