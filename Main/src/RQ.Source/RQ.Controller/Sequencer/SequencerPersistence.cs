using RQ.Common.Serialization;
using RQ.Messaging;
using System;
using UnityEngine;
using WellFired;

namespace RQ.Controller.Sequencer
{
    [AddComponentMenu("RQ/Sequencer/Sequencer Persistence")]
    public class SequencerPersistence : PersistentObject
    {
        [SerializeField]
        private USSequencer _sequencer;

        public override void Awake()
        {
            base.Awake();
            if (_sequencer == null)
                _sequencer = GetComponent<USSequencer>();
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {

                //case Enums.Telegrams.SetEnabled:
                //    var enabled = (bool)msg.ExtraInfo;

                //    break;
                //case Enums.Telegrams.:
                //    AddEntityToDeathPersistence();
                //    //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, "Game Controller",
                //    //    Enums.Telegrams.PersistDeath, null);
                //    break;
            }
            return false;
        }

        public override void Serialize(object data)
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(object data)
        {
            throw new NotImplementedException();
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
    }
}
