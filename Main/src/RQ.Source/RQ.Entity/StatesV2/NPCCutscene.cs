using RQ.Messaging;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/NPC Cutscene")]
    public class NPCCutscene : Active
    {
        public override void Enter()
        {
            base.Enter();
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                Enums.Telegrams.StopMovement, null);
            //_physicsComponent.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
        }

        public override void Exit()
        {
            base.Exit();
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }
    }
}
