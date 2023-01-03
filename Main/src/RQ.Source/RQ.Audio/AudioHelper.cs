using RQ.Messaging;
using RQ.Model.Audio;
using RQ.Model.Serialization;

namespace RQ.Audio
{
    public static class AudioHelper
    {
        private static GamePrefsData _gamePrefsData = null;

        public static GamePrefsData GamePrefsData
        {
            get
            {
                if (_gamePrefsData == null)
                {
                    // This will only ever get called once, not sure if I care about the delegate allocation
                    MessageDispatcher2.Instance.DispatchMsg("GetGamePrefsData", 0f, string.Empty, "Game State Controller", (gamePrefsData) =>
                    {
                        _gamePrefsData = gamePrefsData as GamePrefsData;
                    });
                }
                return _gamePrefsData;
            }
        }

        public static void PlaySound(string playerUniqueId, PlaySoundInfo playSoundInfo, AudioComponent audioComponent)
        {
            if (playSoundInfo.AudioClips == null || playSoundInfo.AudioClips.Count == 0)
                return;
            AudioClipInfo audioClipInfo = new AudioClipInfo();
            var currentClipIndex = UnityEngine.Random.Range(0, playSoundInfo.AudioClips.Count);
            var clip = playSoundInfo.AudioClips[currentClipIndex];
            if (clip == null)
            {
                //Debug.LogError(this.name + " could not find audio clip to play.");
                return;
            }

            audioClipInfo.AudioClip = clip;
            audioClipInfo.Loop = playSoundInfo.LoopSound;
            audioClipInfo.Volume = playSoundInfo.Volume;
            audioClipInfo.ForcePlay = playSoundInfo.ForcePlay;
            var uniqueId = GetAudioReceiverId(playSoundInfo, audioComponent);

            MessageDispatcher2.Instance.DispatchMsg("PlaySound", 0f, playerUniqueId,
                uniqueId, audioClipInfo);
        }

        private static string GetAudioReceiverId(PlaySoundInfo playSoundInfo, AudioComponent audioComponent)
        {
            return playSoundInfo.PlayOnMusicTrack ? "Game Controller" : audioComponent.UniqueId;
        }
    }
}
