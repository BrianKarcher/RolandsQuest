using RQ.Common.Components;
using RQ.Common.Container;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Model.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired;

namespace RQ.Controller.Sequencer
{
    [AddComponentMenu("RQ/Sequencer/Sequencer Trigger")]
    public class SequencerTrigger : ComponentPersistence<SequencerTrigger>, IEventTrigger
    {
        [SerializeField]
        private Transform _sequence;
        [SerializeField]
        private List<string> _luaConditions = null;

        //private USSequencer _sequencer;
        private SpriteBaseComponent _sequencer;
        public event EventHandler EventEnded;
        private long _endCutsceneId, _endCutsceneId2;
        private Action<Telegram2> _endCutsceneDelegate;

        public override void Awake()
        {
            base.Awake();
            _endCutsceneDelegate = (data) =>
            {
                EventEnded(this, EventArgs.Empty);
            };
        }

        public override void Init()
        {
            base.Init();
            SetupSequencer();
            //if (_sequence == null)
            //    _sequence = GetComponent<IComponentBase>();
        }

        public override void StartListening()
        {
            base.StartListening();
            _endCutsceneId =
                MessageDispatcher2.Instance.StartListening("EndCutscene", _componentRepository.UniqueId, _endCutsceneDelegate);
            _endCutsceneId2 =
                MessageDispatcher2.Instance.StartListening("EndCutscene", this.UniqueId, _endCutsceneDelegate);
            //_componentRepository.StartListening("EndCutscene", this.UniqueId, _endCutsceneDelegate, true);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("EndCutscene", _componentRepository.UniqueId, _endCutsceneId);
            MessageDispatcher2.Instance.StopListening("EndCutscene", this.UniqueId, _endCutsceneId2);
            //_componentRepository.StopListening("EndCutscene", this.UniqueId, true);
        }

        //public override bool HandleMessage(Telegram msg)
        //{
        //    if (base.HandleMessage(msg))
        //        return true;

        //    switch (msg.Msg)
        //    {

        //        //case Enums.Telegrams.SetEnabled:
        //        //    var enabled = (bool)msg.ExtraInfo;

        //        //    break;
        //        //case Enums.Telegrams.:
        //        //    AddEntityToDeathPersistence();
        //        //    //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, "Game Controller",
        //        //    //    Enums.Telegrams.PersistDeath, null);
        //        //    break;
        //    }
        //    return false;
        //}

        //public override void Serialize(object data)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void Deserialize(object data)
        //{
        //    throw new NotImplementedException();
        //}

        void OnUse()
        {
            var pass = true;
            foreach (var luaCondition in _luaConditions)
            {
                // TODO Get a Lua interpreter!
                //if (!Lua.IsTrue(luaCondition))
                //{
                //    pass = false;
                //    break;
                //}
            }
            if (pass)
            {
                MessageDispatcher2.Instance.DispatchMsg("PlaySequence", 0f, this.UniqueId, _sequencer.UniqueId, null);
                //GameController.Instance.UIManager.CurrentSequence = _sequencer;
                //MessageDispatcher2.Instance.DispatchMsg("StartCutscene", 0f, this.UniqueId, "Game Controller", null);
                //_sequencer.Play();
            }
            else
            {
                EventEnded(this, EventArgs.Empty);
            }
        }

        public void Trigger(Transform requester)
        {
            OnUse();
        }

        //private void AddEntityToDeathPersistence()
        //{
        //    var deathPersistence = GetSceneDeathData();
        //    if (deathPersistence == null)
        //        return;

        //    deathPersistence.DeadEntities.Add(_componentRepositoryId);
        //}

        //private SceneDeathData GetSceneDeathData()
        //{
        //    return GameDataController.Instance.Data.SceneDeathDatas.FirstOrDefault(i => i.SceneUniqueId
        //        == GameDataController.Instance.Data.CurrentSceneUniqueId);
        //}

        //public override void Start()
        //{
        //    base.Start();
        //    if (!Application.isPlaying)
        //        return;
        //    var deathPersistence = GetSceneDeathData();
        //    if (deathPersistence == null)
        //        return;

        //    //if (deathPersistence.DeadEntities.Contains(_componentRepositoryId))
        //    //    GameObject.Destroy(transform.gameObject);
        //    if (deathPersistence.DeadEntities.Contains(_componentRepositoryId))
        //    {
        //        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _componentRepositoryId,
        //            Enums.Telegrams.Kill, null);
        //    }
        //}

        // TODO FINISH SERIALIZATION NOW~!~!!!!!!!
        public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            var sequencerData = new SequencerTriggerData();
            var repo = _sequence.GetComponent<IComponentRepository>();
            sequencerData.SequenerUniqueId = repo.UniqueId;
            base.SerializeComponent(entitySerializedData, sequencerData);
        }

        public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            var sequencerData = base.DeserializeComponent<SequencerTriggerData>(entitySerializedData);
            _sequence = EntityContainer._instance.GetEntity(sequencerData.SequenerUniqueId).transform;
            SetupSequencer();
        }

        private void SetupSequencer()
        {
            if (_sequence != null)
                _sequencer = _sequence.GetComponent<SpriteBaseComponent>();
        }
    }
}
