using RQ.Physics;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/WalkWander")]
    public class WalkWander : AnimatorState
    {
        public override void Enter()
        {
            //if (_sprite == null)
            //    return;
            base.Enter();
            if (_steering == null)
                return;
            _steering.TurnOn(behavior_type.wander);
        }

        public override void Exit()
        {
            base.Exit();
            if (_steering == null)
                return;
            _steering.TurnOff(behavior_type.wander);
        }
    }
}
