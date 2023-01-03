using RQ.Common.Components;
using RQ.Common.Container;
using RQ.Entity.Common;
using RQ.Messaging;
using RQ.Model.Serialization;
using System;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Player Death")]
    public class PlayerDeathComponent : ComponentPersistence<PlayerDeathComponent>
    {
        [SerializeField]
        private bool _notifyGameController = true;
        [SerializeField]
        private SpriteBaseComponent _actionController;

        private long _processPlayerDeathId;
        private Action<Telegram2> _processPlayerDeathDelegate;

        public override void Awake()
        {
            base.Awake();
            _processPlayerDeathDelegate = (data) =>
            {
                if (_actionController != null)
                {
                    MessageDispatcher2.Instance.DispatchMsg("CheckAndRunActionSequences", 0f,
                        this.UniqueId, _actionController.UniqueId, "PlayerDied");
                }
                if (_notifyGameController)
                {
                    MessageDispatcher2.Instance.DispatchMsg("PlayerDied", 0f,
                        this.UniqueId, "Game Controller", null);
                }
            };
        }

        public override void StartListening()
        {
            base.StartListening();
            _processPlayerDeathId = MessageDispatcher2.Instance.StartListening("ProcessPlayerDeath", _componentRepository.UniqueId, _processPlayerDeathDelegate);
            //_componentRepository.StartListening("ProcessPlayerDeath", this.UniqueId, );
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("ProcessPlayerDeath", _componentRepository.UniqueId, _processPlayerDeathId);
            //_componentRepository.StopListening("ProcessPlayerDeath", this.UniqueId);
        }

        public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            var playerDiedStateData = new PlayerDiedStateData();
            if (_actionController != null)
                playerDiedStateData.ActionControllerUniqueId = _actionController.UniqueId;
            playerDiedStateData.NotifyGameController = _notifyGameController;
            base.SerializeComponent(entitySerializedData, playerDiedStateData);
        }

        public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            var playerDiedStateData = base.DeserializeComponent<PlayerDiedStateData>(entitySerializedData);
            _notifyGameController = playerDiedStateData.NotifyGameController;
            if (!string.IsNullOrEmpty(playerDiedStateData.ActionControllerUniqueId))
                _actionController = EntityContainer._instance.GetEntity(playerDiedStateData.ActionControllerUniqueId) as SpriteBaseComponent;
        }
    }
}
