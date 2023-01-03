using System;
using System.Collections.Generic;
using RQ.Common.Components;
using RQ.Common.Container;
using RQ.Entity.Common;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Entity.Components
{
    [AddComponentMenu("RQ/Components/Entity Alive Tracker")]
    public class EntityAliveTrackerComponent : MessagingObject
    {
        public GameObject[] NotifyWhenOneDied;
        public GameObject[] NotifyWhenAllDied;
        public SpriteBaseComponent[] EntitiesToTrack;

        private long _globalEntityDiedId;
        private Action<Telegram2> _globalEntityDiedDelegate;
        private HashSet<string> _entitiesStillAlive;

        public override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying)
                return;
            if (_globalEntityDiedDelegate == null)
            {
                _globalEntityDiedDelegate = (data) =>
                {
                    var diedEntityUniqueId = (string)data.ExtraInfo;
                    if (!_entitiesStillAlive.Contains(diedEntityUniqueId))
                        return;
                    var playerTransform = EntityContainer._instance.GetMainCharacter().transform;
                    //foreach (var entity in NotifyWhenOneDied)
                    for (int i = 0; i < NotifyWhenOneDied.Length; i++)
                    {
                        NotifyWhenOneDied[i].BroadcastMessage("OnUse", playerTransform, SendMessageOptions.DontRequireReceiver);
                    }

                    _entitiesStillAlive.Remove(diedEntityUniqueId);

                    if (_entitiesStillAlive.Count == 0)
                    {
                        //foreach (var entity in NotifyWhenAllDied)
                        for (int i = 0; i < NotifyWhenAllDied.Length; i++)
                        {
                            Debug.Log($"Broadcasting OnUse to {NotifyWhenAllDied[i].name}");
                            NotifyWhenAllDied[i].BroadcastMessage("OnUse", playerTransform, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                };
            }

            _entitiesStillAlive = new HashSet<string>();

            for (int i = 0; i < EntitiesToTrack.Length; i++)
            {
                _entitiesStillAlive.Add(EntitiesToTrack[i].UniqueId);
            }
        }

        //public override void Start()
        //{
        //    base.Start();
        //    if (!Application.isPlaying)
        //        return;
            
        //}

        public override void StartListening()
        {
            base.StartListening();
            _globalEntityDiedId =
                MessageDispatcher2.Instance.StartListening("GlobalEntityDied", this.UniqueId,
                    _globalEntityDiedDelegate);
        }

        public override void StopListening()
        {
            MessageDispatcher2.Instance.StopListening("GlobalEntityDied", this.UniqueId, _globalEntityDiedId);
        }
    }
}
