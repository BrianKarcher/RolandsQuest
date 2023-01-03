using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Model.Audio
{
    [Serializable]
    public class PlaySoundInfo
    {
        public List<AudioClip> AudioClips = null;
        //[SerializeField]
        //private bool _playInOrder;
        //private bool _stopAudioOnEnter;
        public bool ClearPreviousClips = false;
        public bool StopAudioOnExit = false;
        public bool PlaySound = false;
        public float DelaySound = 0f;
        public bool PlayAsOneShot = false;
        public bool LoopSound = false;
        public bool PlayOnMusicTrack = false;
        public bool ForcePlay = true;
        [Range(0f, 1f)]
        public float Volume = 1f;
    }
}
