using RQ.Common.Components;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.Audio;
using System;
using System.Collections;
using UnityEngine;

namespace RQ.Audio
{
    [AddComponentMenu("RQ/Components/Audio")]
    [Obsolete]
    public class AudioComponent : ComponentPersistence<AudioComponent>
    {
        [SerializeField]
        private AudioSource _audioSource;
        private readonly AudioData _audioData = new AudioData();
        [SerializeField]
        private float _tweenSpeed;
        [SerializeField]
        private bool _fadeOut = false;
        [SerializeField]
        private float _volume = 1.0f;
        [SerializeField]
        private Model.Audio.AudioType AudioType;
        //private IGameStateController _gameStateController;
        
        //private float _globalVolume;
        //private long _updateVolumeId, _playSoundId, _playSoundInfoId, _stopAudioId, _playOneShotId, _playSoundId2, _playSoundInfoId2, _stopAudioId2;

        //private Action<Telegram2> _updateVolumeDelegate, _playSoundDelegate, _playSoundInfoDelegate, _stopAudioDelegate, _playOneShotDelegate;
        //private float _clipVolume;

        //[SerializeField]
        //private TweenVolume _tweenVolume;

        public override void Awake()
        {
            base.Awake();
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();
            //_updateVolumeDelegate = (data) =>
            //{
            //    SetVolume();
            //    //var setVolumeData = data.ExtraInfo as SetVolumeData;
            //    //if (setVolumeData == null)
            //    //    return;
            //    //if (AudioType == setVolumeData.AudioType)
            //    //{
            //    //    _globalVolume = setVolumeData.Volume;

            //    //}
            //};
            //_playSoundDelegate = (data) =>
            //{
            //    var clip = (AudioClipInfo) data.ExtraInfo;
            //    PlayAudioClip(clip);
            //};
            //_playSoundInfoDelegate = (data) =>
            //{
            //    var soundInfo = (PlaySoundData) data.ExtraInfo;
            //    PlaySoundInfo(soundInfo);
            //};
            //_stopAudioDelegate = (data) =>
            //{
            //    _audioSource.Stop();
            //};
            //_playOneShotDelegate = (data) =>
            //{
            //    var sound = data.ExtraInfo as AudioClip;
            //    _audioSource.PlayOneShot(sound);
            //};
            //_gameStateController = GameObject.FindObjectOfType<IGameStateController>();
        }

        //public override void StartListening()
        //{
        //    base.StartListening();
        //    //_componentRepository.StartListening("FadeVolumeOut", this.UniqueId, (data) =>
        //    //    {
        //    //        //StartCoroutine(FadeOut());
        //    //    });
        //    //_playSoundId = MessageDispatcher2.Instance.StartListening("PlaySound", _componentRepository.UniqueId, _playSoundDelegate);
        //    //_playSoundId2 = MessageDispatcher2.Instance.StartListening("PlaySound", this.UniqueId, _playSoundDelegate);
        //    //_componentRepository.StartListening("PlaySound", this.UniqueId, , true);
        //    //_playSoundInfoId = MessageDispatcher2.Instance.StartListening("PlaySoundInfo", _componentRepository.UniqueId, _playSoundInfoDelegate);
        //    //_playSoundInfoId2 = MessageDispatcher2.Instance.StartListening("PlaySoundInfo", this.UniqueId, _playSoundInfoDelegate);
        //    //_componentRepository.StartListening("PlaySoundInfo", this.UniqueId, , true);
        //    //_stopAudioId = MessageDispatcher2.Instance.StartListening("StopAudio", _componentRepository.UniqueId, _stopAudioDelegate);
        //    //_stopAudioId2 = MessageDispatcher2.Instance.StartListening("StopAudio", this.UniqueId, _stopAudioDelegate);
        //    //_componentRepository.StartListening("StopAudio", this.UniqueId, , true);
        //    //_playOneShotId = MessageDispatcher2.Instance.StartListening("PlayOneShot", _componentRepository.UniqueId, _playOneShotDelegate);
        //    //_componentRepository.StartListening("PlayOneShot", this.UniqueId, );
        //    //_updateVolumeId = MessageDispatcher2.Instance.StartListening("UpdateVolume", this.UniqueId, _updateVolumeDelegate);
        //}

        //public override void StopListening()
        //{
        //    base.StopListening();
        //    //_componentRepository.StopListening("FadeVolumeOut", this.UniqueId);
        //    MessageDispatcher2.Instance.StopListening("PlaySound", _componentRepository.UniqueId, _playSoundId);
        //    MessageDispatcher2.Instance.StopListening("PlaySound", this.UniqueId, _playSoundId2);
        //    //_componentRepository.StopListening("PlaySound", this.UniqueId, true);
        //    MessageDispatcher2.Instance.StopListening("PlaySoundInfo", _componentRepository.UniqueId, _playSoundInfoId);
        //    MessageDispatcher2.Instance.StopListening("PlaySoundInfo", this.UniqueId, _playSoundInfoId2);
        //    //_componentRepository.StopListening("PlaySoundInfo", this.UniqueId, true);
        //    MessageDispatcher2.Instance.StopListening("StopAudio", _componentRepository.UniqueId, _stopAudioId);
        //    MessageDispatcher2.Instance.StopListening("StopAudio", this.UniqueId, _stopAudioId2);
        //    //_componentRepository.StopListening("StopAudio", this.UniqueId, true);
        //    MessageDispatcher2.Instance.StopListening("PlayOneShot", _componentRepository.UniqueId, _playOneShotId);
        //    //_componentRepository.StopListening("PlayOneShot", this.UniqueId);
        //    MessageDispatcher2.Instance.StopListening("UpdateVolume", this.UniqueId, _updateVolumeId);
        //}

        /// <summary>
        ///  TODO Use the AudioHelper's PlaySound instead
        /// </summary>
        /// <param name="playSoundInfo"></param>
        //protected void PlaySoundInfo(PlaySoundData playSoundInfo)
        //{
        //    //if (playSoundInfo.AudioClip == null)
        //    //    return;
        //    if (!playSoundInfo.PlaySound)
        //        return;

        //    //var currentClipIndex = UnityEngine.Random.Range(0, playSoundInfo.AudioClips.Count);
        //    var clip = playSoundInfo.AudioClip;
        //    if (clip == null)
        //    {
        //        //Debug.LogError(this.name + " could not find audio clip to play.");
        //        return;
        //    }
        //    //var uniqueId = GetAudioReceiverId(playSoundInfo);

        //    // This is more for sound effects
        //    if (playSoundInfo.PlayAsOneShot)
        //    {
        //        var playOneShotData = new PlayOneShotData()
        //        {
        //            Clip = clip,
        //            Volume = playSoundInfo.Volume
        //        };
        //        PlayOneShot(playSoundInfo);
        //        //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, uniqueId,
        //        //    Enums.Telegrams.PlayOneShot, playOneShotData);
        //    }
        //    else // This is more for music
        //    {
        //        AudioClipInfo audioClipInfo = new AudioClipInfo()
        //        {
        //            AudioClip = clip,
        //            Loop = playSoundInfo.LoopSound,
        //            Volume = playSoundInfo.Volume,
        //            ForcePlay = playSoundInfo.ForcePlay
        //        };
        //        PlayAudioClip(audioClipInfo);
        //        //MessageDispatcher2.Instance.DispatchMsg("PlaySound", 0f, this.UniqueId,
        //        //    uniqueId, audioClipInfo);
        //    }

        //    //PlayAudioClip(audioClipInfo);
        //}

        //private string GetAudioReceiverId(PlaySoundData playSoundInfo)
        //{
        //    return playSoundInfo.PlayOnMusicTrack ? "Game Controller" : UniqueId;
        //}

        IEnumerator FadeOut()
        {
            //Debug.LogError($"FadeOut called on {this.name}");
            while (_audioSource.volume > 0)
            {
                _audioSource.volume -= (1f / _tweenSpeed) * Time.unscaledDeltaTime;
                yield return null;
            }
            //_audioSource.volume = 0;
            //_audioSource.Stop();
            PlayAudioClip();
        }

        public void PlayOneShot(PlaySoundData soundData)
        {
            _audioSource.PlayOneShot(soundData.AudioClip, CalculateVolume(soundData.Volume));
        }

        public override bool HandleMessage(Messaging.Telegram telegram)
        {
            if (base.HandleMessage(telegram))
                return true;

            switch (telegram.Msg)
            {
                case Enums.Telegrams.PlayOneShot:
                    var sound = telegram.ExtraInfo as PlayOneShotData;
                    if (sound == null)
                        return true;
                    // The reason I use PlayClipAtPoint instead of PlayOneShot is because PlayClipAtPoint creates a new AudioSource
                    // So it persists even if the object is destroyed
                    //AudioSource.PlayClipAtPoint(sound.Clip, transform.position, CalculateVolume(sound.Volume));
                    _audioSource.PlayOneShot(sound.Clip, CalculateVolume(sound.Volume));
                    break;
                case Enums.Telegrams.StopSound:
                    _audioData.AudioClipInfo = new AudioClipInfo();
                    _audioSource.Stop();
                    break;
                case Enums.Telegrams.PlaySound:
                    //_audioSource.Play();
                    var clip = (AudioClipInfo)telegram.ExtraInfo;
                    PlayAudioClip(clip);
                    break;
                //case Enums.Telegrams.AddSoundClips:
                //    var clip = (AudioClipInfo)telegram.ExtraInfo;
                //    _audioData.AudioClipInfo = clip;
                //    break;
                //case Enums.Telegrams.ClearSoundClips:
                //    //_audioData.AudioClips.Clear();
                //    break;
                //case Enums.Telegrams.SoundCompleted:
                //    if (_audioData.Loop)
                //        PlayNextAudioClip();
                //    break;
            }

            return false;
        }

        public void PlayAudioClip(AudioClipInfo audioClipInfo)
        {
            bool playAudio = false;
            if (audioClipInfo.ForcePlay)
                playAudio = true;
            else if (_audioData.AudioClipInfo.AudioClip != audioClipInfo.AudioClip)
            {
                playAudio = true;
                if (_fadeOut)
                {
                    _audioData.AudioClipInfo = audioClipInfo;
                    StartCoroutine(FadeOut());
                    return;
                }
                //_audioData.AudioClipInfo = audioClipInfo;
                //if (_trackChangeFadeSpeed != 0f)
                //{
                //    _changingTracks = true;
                //    playAudio = false;
                //}
                //else
                //{
                //    playAudio = true;
                //}
            }
                
            //else if (!_audioData.AudioClipInfo.Loop)
            //    playAudio = true;

            if (playAudio)
            {
                _audioData.AudioClipInfo = audioClipInfo;
                PlayAudioClip();
            }
        }

        public void PlayAudioClip()
        {
            // Choose the next audio clip to play
            //if (_playSequentially)
            //{
            //    _currentClipIndex++;
            //    if (_currentClipIndex > _audioClips.Count - 1)
            //        _currentClipIndex = 0;
            //}
            //else
            //{
            //    _currentClipIndex = UnityEngine.Random.Range(0, _audioClips.Count - 1);
            //}

            //var clipToPlay = _audioData.AudioClips[_audioData.CurrentClipIndex];
            var clipToPlay = _audioData.AudioClipInfo;
            var clip = clipToPlay.AudioClip;
            if (clip == null)
                return;
            _audioSource.clip = clip;
            _audioSource.loop = clipToPlay.Loop;
            _audioSource.time = clipToPlay.ClipPosition;
            //GameDataController

            SetVolume();

            //Debug.LogError("\ " + clip.name + " volume " + clipToPlay.Volume);
            _audioSource.Play();
            //_audioSource.Play()
            //_audioSource.PlayOneShot()
            //if (clipToPlay.Loop)
            //{
            //    var clipLength = clip.length;
            //    MessageDispatcher.Instance.DispatchMsg(clipLength, this.UniqueId, this.UniqueId,
            //        Enums.Telegrams.PlayNextSound, null);
            //}
        }

        private void SetVolume()
        {
            var audioClipVolume = _audioData.AudioClipInfo.Volume;
            if (_audioSource == null)
            {
                Debug.LogError($"Could not locate Audio Source for {_componentRepository.name}");
                return;
            }
            var volume = CalculateVolume(audioClipVolume);
            //Debug.LogError($"Setting volume on {this.name} to {volume}");
            _audioSource.volume = volume;
        }

        private float CalculateVolume(float clipVolume)
        {
            //var gamePrefsData = GetGamePrefsData();
            var gamePrefsData = AudioHelper.GamePrefsData;
            float globalVolume = 1.0f;
            if (AudioType == Model.Audio.AudioType.Music)
                globalVolume = gamePrefsData.MusicVolume;
            else
                globalVolume = gamePrefsData.SoundEffectVolume;
            return clipVolume * _volume * globalVolume;
        }

        public AudioData GetAudioData()
        {
            return _audioData;
        }

        public AudioSource GetAudioSource()
        {
            return _audioSource;
        }

        /// <summary>
        /// The reason EntityData is a parameter instead of a return type is because multiple layers of derived objects
        /// may populate different parts of this object
        /// </summary>
        /// <param name="entityData"></param>
        //public override void Serialize(EntitySerializedData entityData)
        //{
        //    base.Serialize(entityData);
        //    if (_audioData.AudioClipInfo != null)
        //        _audioData.AudioClipInfo.ClipPosition = _audioSource.time;

        //    base.SerializeComponent(entityData, _audioData);
        //}

        //public override void Deserialize(EntitySerializedData entityData)
        //{
        //    base.Deserialize(entityData);
        //    _audioData = base.DeserializeComponent<AudioData>(entityData);
        //    if (_audioData.AudioClipInfo != null && _audioData.AudioClipInfo.ClipPosition != 0f)
        //    {
        //        PlayAudioClip();
        //    }
        //}

        //public static void PlaySoundInfo(PlaySoundData soundInfo, IComponentRepository repo)
        //{
        //    if (soundInfo.AudioClip != null)
        //    {
        //        var audioComponent = repo.Components.GetComponent<AudioComponent>();
        //        string uniqueid = null;
        //        if (soundInfo.PlayOnMusicTrack)
        //            uniqueid = "Game Controller";
        //        else if (audioComponent != null)
        //            uniqueid = audioComponent.UniqueId;

        //        if (uniqueid != null)
        //            MessageDispatcher2.Instance.DispatchMsg("PlaySoundInfo", 0f, repo.UniqueId, uniqueid, soundInfo);
        //    }
        //}
    }
}
