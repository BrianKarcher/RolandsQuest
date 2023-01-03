using RQ.Common.Components;
using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Messaging;
using System;
using RQ.Model.Interfaces;
using UnityEngine;
using WellFired;

namespace RQ.Controller.Sequencer
{
    [AddComponentMenu("RQ/Sequencer/Sequencer Link")]
    [ExecuteInEditMode]
    public class SequencerLink : ComponentPersistence<SequencerLink>, ISequencerLink
    {
        [SerializeField]
        private USSequencer _sequencer;
        [SerializeField]
        private bool _playMultipleTimes = false;
        [SerializeField]
        private bool _disableMainInput = true;
        [SerializeField]
        private string _sequenceName;
        public event Action OnSequenceComplete;

        private long _playSequenceId;
        private Action<Telegram2> _playSequenceDelegate;

        //private bool _isPlaying = false;

        public override void Awake()
        {
            base.Awake();
            _playSequenceDelegate = (data) =>
            {
                // Quick name check to make sure we are playing the right sequence
                if (data.ExtraInfo != null)
                {
                    if (_sequenceName != (string) data.ExtraInfo)
                        return;
                }
                Play();
            };
        }

        public override void Init()
        {
            base.Init();

            if (_sequencer == null)
                _sequencer = GetComponent<USSequencer>();

            if (!Application.isPlaying)
                return;

            _sequencer.PlaybackFinished = PlaybackFinished;

            //EntityContainer._instance.AddEntity(this);

            //_sequencer.PlaybackStarted = (sequencer) =>
            //{
            //    //if (_isPlaying)
            //    //    return;
            //    Debug.Log("Sequence Started");
            //    GameDataController.Instance.Data.RunningSequenceUniqueId = this.UniqueId;
            //    GameController.Instance.UIManager.CurrentSequence = _sequencer;
            //    //_isPlaying = true;
            //    MessageDispatcher2.Instance.DispatchMsg("StartCutscene", 0f, this.UniqueId, "Game Controller", null);
            //    //MessageDispatcher2.Instance.DispatchMsg("StopSaveRequest", 0f, this.UniqueId, "UI Manager", null);
            //    //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, "Game Controller",
            //    //    Telegrams.SequenceStarted, null);
            //};

            //_sequencer.PlaybackFinished = (sequencer) =>
            //{
            //    Debug.Log("Sequence Finished");
            //    GameDataController.Instance.Data.RunningSequenceUniqueId = null;
            //    GameController.Instance.UIManager.CurrentSequence = null;
            //    _isPlaying = false;
            //    MessageDispatcher2.Instance.DispatchMsg("StopCutscene", 0f, this.UniqueId, "Game Controller", null);
            //    //MessageDispatcher2.Instance.DispatchMsg("ResumeSaveRequest", 0f, this.UniqueId, "UI Manager", null);
            //    //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, "Game Controller",
            //    //    Telegrams.SequenceEnded, null);
            //};
        }

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("PlaySequence", _componentRepository.UniqueId, _playSequenceDelegate);
            //_componentRepository.StartListening("PlaySequence", this.UniqueId, );
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("PlaySequence", _componentRepository.UniqueId, _playSequenceId);
            //_componentRepository.StopListening("PlaySequence", this.UniqueId);
        }

        //public override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    if (!Application.isPlaying)
        //        return;
        //    EntityContainer._instance.RemoveEntity(this);
        //}

        //public override void Serialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);

        //    var sequencerLinkData = new SequencerLinkData();
        //    sequencerLinkData.UniqueId = this.UniqueId;
        //    List<SequencerLinkItemData> itemDatas = new List<SequencerLinkItemData>();
        //    var timelineContainers = GetTimelineContainers();
        //    foreach (var timelineContainer in timelineContainers)
        //    {
        //        if (timelineContainer.Index < 0)
        //            continue;
        //        if (timelineContainer.AffectedObject == null)
        //            continue;
        //        var sprite = timelineContainer.AffectedObject.GetComponent<SpriteBaseComponent>();
        //        if (sprite == null)
        //            continue;
        //        var itemData = new SequencerLinkItemData()
        //        {
        //            AffectedObjectUniqueId = sprite.UniqueId,
        //            //AffectedObjectPath = timelineContainer.AffectedObjectPath,
        //            Index = timelineContainer.Index
        //        };
        //        itemDatas.Add(itemData);
        //    }
        //    sequencerLinkData.SequencerLinkItemDatas = itemDatas;

        //    // In the case of SequencerLink, all child objects belong to this component.
        //    ActionSerializerHelper.SerializeActions(entitySerializedData, gameObject, true);

        //    //entitySerializedData.ComponentData.Add(GetName(), sequencerLinkData);
        //    //{
        //    //    UniqueId
        //    //};
        //    //return sequencerLinkData;

        //    base.SerializeComponent(entitySerializedData, sequencerLinkData);
        //}

        //public SequencerLinkData Serialize()
        //{

        //}

        public USTimelineContainer[] GetTimelineContainers()
        {
            return GetComponentsInChildren<USTimelineContainer>();
        }

        //public override void Deserialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);

        //    var sequencerLinkData = base.DeserializeComponent<SequencerLinkData>(entitySerializedData);

        //    Debug.Log("SequencerLink - Deserialize Called");
        //    //object tempValue;
        //    //if (!entitySerializedData.ComponentData.TryGetValue(GetName(), out tempValue))
        //    //    return;

        //    //var sequencerLinkData = Persistence.DeserializeObject<SequencerLinkData>(tempValue);
        //    var timelineContainers = GetTimelineContainers();
        //    foreach (var item in sequencerLinkData.SequencerLinkItemDatas)
        //    {
        //        var timelineContainer = timelineContainers.FirstOrDefault(i => i.Index == item.Index);
        //        if (timelineContainer == null)
        //            continue;
        //        //timelineContainer.AffectedObjectPath = item.AffectedObjectPath;
        //        timelineContainer.AffectedObject = EntityContainer._instance.GetEntity(item.AffectedObjectUniqueId).transform;

        //    }

        //    ActionSerializerHelper.DeserializeActions(entitySerializedData, gameObject, true);
        //}

        //public override void DeserializeUniqueIds(EntitySerializedData entitySerializedData)
        //{
        //    base.DeserializeUniqueIds(entitySerializedData);
        //    ActionSerializerHelper.DeserializeActionUniqueIds(entitySerializedData, gameObject, true);
        //}

        //public bool IsPlaying()
        //{
        //    return _isPlaying;
        //}

        public void Play()
        {
            Debug.Log($"Sequence {_sequencer.name} Started");
            //if (GameDataController.Instance.Data != null)
                GameDataController.Instance.Data.RunningSequenceUniqueId = this.UniqueId;
            if (GameController.Instance.UIManager.CurrentSequence != null)
            {
                Debug.LogError($"(SequencerLink) Play {_sequencer.name} - Sequence '{GameController.Instance.UIManager.CurrentSequence.name}' already running");
            }
            // If there is a sequence already running, don't replace it
            if (GameController.Instance.UIManager.CurrentSequence == null)
                GameController.Instance.UIManager.CurrentSequence = this;
            if (_disableMainInput)
            {
                var mainCharacter = EntityContainer._instance.GetMainCharacter();
                MessageDispatcher2.Instance.DispatchMsg("EnableInput", 0f, 
                    this.UniqueId, mainCharacter.UniqueId, "0");
                MessageDispatcher2.Instance.DispatchMsg("StopMovement", 0f,
                    this.UniqueId, mainCharacter.UniqueId, "0");
            }
            //_isPlaying = true;
            GameStateController.Instance.PlayCutscene = true;

            //MessageDispatcher2.Instance.DispatchMsg("StartCutscene", 0f, this.UniqueId, "Game Controller", null);
            // Broadcast so both the MainCharacter and Game Controller receive it, and whoever else is listening.
            MessageDispatcher2.Instance.DispatchMsg("StartCutscene", 0f, this.UniqueId, null, null);
            _sequencer.Play();
        }

        private void PlaybackFinished(USSequencer sequencer)
        {
            Debug.LogError("(SequencerLink) Sequence " + sequencer.name + " Finished");
            SequenceComplete();
        }

        public void Stop()
        {
            _sequencer.Stop();
        }

        public bool IsPlaying()
        {
            return _sequencer.IsPlaying;
        }

        public void Pause()
        {
            _sequencer.Pause();
        }

        public USSequencer GetSequencer()
        {
            return _sequencer;
        }

        public void SequenceComplete()
        {
            Debug.Log("(SequencerLink) SequenceComplete");

            OnSequenceComplete?.Invoke();
            //_sequenceComplete();
            if (_disableMainInput)
            {
                var mainCharacter = EntityContainer._instance.GetMainCharacter();
                MessageDispatcher2.Instance.DispatchMsg("EnableInput", 0f,
                    this.UniqueId, mainCharacter.UniqueId, "1");
            }
            MessageDispatcher2.Instance.DispatchMsg("SequenceComplete", 0f, this.UniqueId, null, this._sequencer.name);
        }

        //public USSequencer Sequence()
        //{
        //    return _sequencer;
        //}

        //public void OnSequenceComplete(Action sequenceComplete)
        //{
        //    _sequenceComplete = sequenceComplete;
        //}
    }
}
