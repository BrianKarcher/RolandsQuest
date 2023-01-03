using RQ.Audio;
using RQ.Messaging;
using RQ.Model.Audio;
using UnityEngine;

namespace RQ.FSM.V2.States
{
    public class PlaySoundState : StateBase
    {
        protected AudioComponent _audioComponent;

        [SerializeField]
        PlaySoundInfo _soundInfo = null;

        public override void SetupState()
        {
            base.SetupState();
            if (_audioComponent == null)
                _audioComponent = _spriteBase.Components.GetComponent<AudioComponent>();
        }

        public override void Enter()
        {
            base.Enter();

            if (_soundInfo.AudioClips.Count == 0)
                return;

            var currentClipIndex = UnityEngine.Random.Range(0, _soundInfo.AudioClips.Count - 1);
            var playSoundData = new PlaySoundData()
            {
                AudioClip = _soundInfo.AudioClips[currentClipIndex],
                PlaySound = _soundInfo.PlaySound,
                //PlayOnMusicTrack = _soundInfo.PlayOnMusicTrack,
                //PlayAsOneShot = _soundInfo.PlayAsOneShot,
                Volume = _soundInfo.Volume
            };

            PlaySound(playSoundData);

            //_audioClipPlayer = new AudioClipPlayer();
            //_audioClipPlayer.SetAudioClips(_audioClips);
            //if (_playInOrder)
            //    _audioClipPlayer.PlayClipsInSequence();
            //else
            //    _audioClipPlayer.PlayAllAtOnce();
        }

        protected virtual void PlaySound(PlaySoundData soundInfo)
        {
            MessageDispatcher2.Instance.DispatchMsg("PlaySoundOnSoundEffectTrack", 0f, this.UniqueId,
                "Game Controller", soundInfo);
        }

        public override void Exit()
        {
            base.Exit();

            if (_soundInfo.StopAudioOnExit)
            {
                //var playSoundData = new PlaySoundData()
                //{
                //    //AudioClip = _soundInfo.AudioClips[currentClipIndex],
                //    PlaySound = _soundInfo.PlaySound,
                //    //PlayOnMusicTrack = _soundInfo.PlayOnMusicTrack,
                //    //PlayAsOneShot = _soundInfo.PlayAsOneShot,
                //    Volume = _soundInfo.Volume
                //};
                //var uniqueId = GetAudioReceiverId(playSoundData);
                MessageDispatcher2.Instance.DispatchMsg("StopAudio", 0f, this.UniqueId, 
                    _audioComponent.UniqueId, null);
            }
                //_audioClipPlayer.Stop();
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
    }
}
