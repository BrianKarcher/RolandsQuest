using UnityEngine;
//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;
using RQ.Entity.Common;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Battle")]
    public class BattleState : StateBase
    {
        public override void Enter()
        {
            // Let everybody know we are in battle mode
            EntityController.Instance.SendMessageToAllEntities(0f, GameController.Instance.UniqueId, Enums.Telegrams.BattleMode, true);
        }
    }
}
