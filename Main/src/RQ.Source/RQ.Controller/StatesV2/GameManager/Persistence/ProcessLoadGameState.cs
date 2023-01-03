//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;
using RQ.Messaging;
using UnityEngine;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Process Load Game")]
    public class ProcessLoadGameState : StateBase
    {
        public override void Enter()
        {
            base.Enter();
            //Debug.LogError("ProcessLoadGameState Called");
            //GameStateController.Instance.LoadGame(GameStateController.Instance.LoadGameFileName);
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, "UI Manager",
            //    Enums.Telegrams.LoadComplete, null);
            //Complete();
        }
    }
}
