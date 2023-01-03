using UnityEngine;

namespace RQ.Model.Audio
{
    public struct PlaySoundData
    {
        public AudioClip AudioClip { get; set; }
        public bool ClearPreviousClips { get; set; }
        public bool StopAudioOnExit { get; set; }
        public bool PlaySound { get; set; }
        public float DelaySound { get; set; }
        //public bool PlayAsOneShot { get; set; }
        public bool LoopSound { get; set; }
        //public bool PlayOnMusicTrack { get; set; }
        public bool ForcePlay { get; set; }
        public float Volume { get; set; }
    }
}
