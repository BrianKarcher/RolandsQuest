using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Treasure Opening")]
    public class TreasureOpeningState : AnimatorState
    {
        //public override void Exit()
        //{
        //    base.Exit();
        //    //sprite.GetSteering().TurnOff(behavior_type.wander);
        //    var usableObject = _spriteBase.Components.GetComponent<UsableComponent>();
        //    if (usableObject != null)
        //    {
        //        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, usableObject.UniqueId,
        //            Enums.Telegrams.SetEnabled, false);
        //    }
        //}
    }
}
