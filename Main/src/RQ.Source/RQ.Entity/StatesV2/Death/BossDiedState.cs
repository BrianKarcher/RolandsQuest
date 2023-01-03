using RQ.Messaging;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Death/Boss Died")]
    public class BossDiedState : DiedState
    {
        public override void Enter()
        {
            base.Enter();
            MessageDispatcher2.Instance.DispatchMsg("BossDied", 0f,
                this.UniqueId, "Game Controller", null);
            Complete();
        }
    }
}
