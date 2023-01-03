using RQ.Model.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using RQ.Entity.Components;
using RQ.Audio;
using RQ.Messaging;
using RQ.AI;
using RQ.Physics.Components;

namespace RQ.Entity.AtomAction.Action
{
    public enum EntitySound
    {
        Damaged = 0,
        Killed = 1
    }

    [Obsolete]
    [Serializable]
    public class PlayEntitySoundAtom : AtomActionBase
    {
        public bool _playOnMusicTrack;
        public bool _playOnSoundEffectTrack;
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

            var soundInfo = new PlaySoundData()
            {
                AudioClip = audioClip,
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
