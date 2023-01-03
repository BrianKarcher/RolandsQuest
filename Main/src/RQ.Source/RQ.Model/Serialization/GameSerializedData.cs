using Newtonsoft.Json;
using RQ.Model.Serialization;
using System;

namespace RQ.Serialization
{
    [Serializable]
    public class GameSerializedData
    {
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime SaveDate { get; set; }
        public SceneSnapshot SceneSnapshot { get; set; }
        //public GameData GameData { get; set; }
        //public SequencerLinkData[] SequencerLinkDatas { get; set; }
        //public int SceneId { get; set; }
        //public string SceneName { get; set; }
        //public GameState GameState { get; set; }
        //public EntitySerializedData[] EntityData { get; set; }
        //public EntitySerializedData GameControllerData { get; set; }
        //public Telegram[] MessageData { get; set; }
        //public Telegram2[] MessageData2 { get; set; }

        //public string DialogueSystemData { get; set; }
    }
}
