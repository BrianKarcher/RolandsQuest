using RQ.Entity.Common;
using RQ.Enums;
using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.Entity.StatesV2.Custom
{
    [AddComponentMenu("RQ/States/State/Custom/Flying Racoon Target Reached")]
    public class FlyingRacoonTargetReachedState : Active
    {

        [SerializeField]
        private string _treeState;

        public override void Enter()
        {
            base.Enter();
            
            _aiComponent.Parent = _aiComponent.Target;
                      
            var parentSpriteComponent = _aiComponent.Parent.GetComponent<SpriteBaseComponent>();
            if (parentSpriteComponent == null)
                throw new Exception("Target has no SpriteBaseComponent");
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, parentSpriteComponent.UniqueId,
                Telegrams.ChangeStateByName, _treeState);

            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                Telegrams.StopMovement, null);
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
            //    Telegrams.ChooseRandomTarget, null);      
            Complete();
        }
    }
}
