using RQ.Entity.StatesV2;
//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;
using RQ.Messaging;
using UnityEngine;


namespace RQ.Controller.StatesV2.GameManager
{
    [AddComponentMenu("RQ/States/State/Game Manager/Play")]
    public class PlayStateGM : MenuState
    {
        public override void Enter()
        {
            base.Enter();
            MessageDispatcher2.Instance.DispatchMsg("EnableUsable", 0f, this.UniqueId, "Usable Controller", null);
        }

        public override void Exit()
        {
            base.Exit();
            MessageDispatcher2.Instance.DispatchMsg("DisableUsable", 0f, this.UniqueId, "Usable Controller", null);
        }



        //public override bool HandleMessage(Telegram telegram)
        //{
        //    if (base.HandleMessage(telegram))
        //        return true;

        //    switch (telegram.Msg)
        //    {
        //        case Enums.Telegrams.ChangeScene:
        //            StateMachine.GetStateInfo().ChangeScene = true;
        //            GameStateController.Instance.ChangingScene = true;
        //            break;
        //    }
        //    return false;
        //}
    }
}
