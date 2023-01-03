using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.Audio
{
    [AddComponentMenu("RQ/Manager/Audio")]
    [Obsolete]
    public class AudioController : MessagingObject
    {
        //public AudioController()
        //{
        //    SetupAudioList();
        //}

        //private Dictionary<string, AudioClip> _audioClips = new Dictionary<string,AudioClip>();

        //[HideInInspector]
        //public static AudioController Instance; // = new AudioController();

        //public override void Awake()
        //{
        //    base.Awake();

        //    if (Instance == null)
        //        Instance = this;
        //    //otherwise, if we do, kill this thing
        //    else
        //    {
        //        return;
        //    }

        //    SetupAudioList();

        //    if (!Application.isPlaying)
        //        return;
        //}

        //public void SetupAudioList()
        //{
        //    _audioClips.Clear();
        //    //var audioClips = Resources.FindObjectsOfTypeAll<AudioClip>();
        //    var audioClips = Resources.LoadAll<AudioClip>("Audio");

        //    foreach (var audio in audioClips)
        //    {
        //        if (audio == null)
        //            continue;
        //        _audioClips.Add(audio.name, audio);
        //    }
        //}

        //public AudioClip GetAudioClip(string clipName)
        //{
        //    if (string.IsNullOrEmpty(clipName) || !_audioClips.ContainsKey(clipName))
        //        return null;
        //    return _audioClips[clipName];
        //}
    }
}
