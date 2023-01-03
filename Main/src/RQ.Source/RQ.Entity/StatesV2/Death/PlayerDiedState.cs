using RQ.Entity.Common;
using RQ.Messaging;
using RQ.Model.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Death/Player Died")]
    public class PlayerDiedState : DiedState
    {

        //[SerializeField]
        //private float _deathTimer = 0.5f;

        public override void Enter()
        {
            base.Enter();
            //Complete();
            var spriteAnimator = GetSpriteAnimator();
            var animationLength = spriteAnimator.GetCurrentClipLength();
            MessageDispatcher2.Instance.DispatchMsg("Timer", animationLength, 
                this.UniqueId, this.UniqueId, null);
        }

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("Timer", this.UniqueId, (data) =>
                {
                    MessageDispatcher2.Instance.DispatchMsg("ProcessPlayerDeath", 0f, this.UniqueId,
                        _componentRepository.UniqueId, null);
                    Complete();
                });            
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("Timer", this.UniqueId, -1);
        }
    }
}
