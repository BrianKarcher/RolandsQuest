using System;
using UnityEngine;

namespace RQ.Audio
{
    [Serializable]
    public struct AudioClipInfo
    {
        public bool Loop { get; set; }
        //public string Name { get; set; }
        public bool ForcePlay { get; set; }
        // Used for serialization, so we can start the clip where it left off
        public float ClipPosition { get; set; }
        public float Volume;
        public AudioClip AudioClip { get; set; }
    }
}
