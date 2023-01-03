using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Hit Ground")]
    public class HitGroundState : AnimatorState
    {
        public override void Enter()
        {
            base.Enter();
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId, 
            //    Enums.Telegrams.StopMovement, null);
            //var physicsData = _physicsComponent.GetPhysicsData() as PhysicsData;
            //physicsData.Altitude = new Vector2D(0f, 0f);
            Complete();
        }
    }
}
