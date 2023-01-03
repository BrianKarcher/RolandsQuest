using RQ.Model.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using RQ.Entity.Components;
using RQ.Audio;
using RQ.Messaging;
using RQ.AI;

namespace RQ.Entity.AtomAction.Action
{
    [Serializable]
    public class PlaySoundOneShotAtom : AtomActionBase
    {
        public bool _playOnMusicTrack;
        [Range(0, 1)]
        public float _volume = 1f;
        public AudioClip _audioClip;
        protected AudioComponent _audioComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_audioComponent == null)
                _audioComponent = entity.Components.GetComponent<AudioComponent>();

            var soundInfo = new PlaySoundData()
            {
                AudioClip = _audioClip,
                PlaySound = true,
                //PlayOnMusicTrack = _playOnMusicTrack,
                //PlayAsOneShot = true,
                Volume = _volume
            };
            PlaySound(soundInfo);
        }

        protected virtual void PlaySound(PlaySoundData soundInfo)
        {
            MessageDispatcher2.Instance.DispatchMsg("PlaySoundOnSoundEffectTrack", 0f, _entity.UniqueId,
                "Game Controller", soundInfo);
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
