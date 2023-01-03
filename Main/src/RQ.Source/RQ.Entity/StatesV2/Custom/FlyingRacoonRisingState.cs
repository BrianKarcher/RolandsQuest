using RQ.Entity.Common;
using RQ.Enums;
using RQ.Messaging;
using RQ.Physics;
using System;
using UnityEngine;

namespace RQ.Entity.StatesV2.Physics
{
    [AddComponentMenu("RQ/States/State/Custom/Flying Racoon Rise")]
    public class FlyingRacoonRisingState : SimpleMovementState
    {        
        [SerializeField]
        private string _treeState;

        public override void Enter()
        {
            base.Enter();
            var parentSpriteComponent = _aiComponent.Parent.GetComponent<SpriteBaseComponent>();
            if (parentSpriteComponent == null)
                throw new Exception("Target has no SpriteBaseComponent");
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, parentSpriteComponent.UniqueId,
                Telegrams.ChangeStateByName, _treeState);
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                Telegrams.SetPos, (Vector2D)parentSpriteComponent.transform.position);
            _collisionComponent.gameObject.SetActive(true);
            //MessageDispatcher2.Instance.DispatchMsg("EnableGO", 0f, this.UniqueId, _collisionComponent.UniqueId, "1");
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
            //    Telegrams.ChooseRandomTarget, null);            
        }
    }
}
