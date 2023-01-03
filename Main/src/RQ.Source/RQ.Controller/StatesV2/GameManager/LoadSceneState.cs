using RQ.Common.Controllers;
using RQ.FSM.V2;
using RQ.FSM.V2.States;
using RQ.Messaging;
using System;
using UnityEngine;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Load Scene")]
    [Obsolete]
    public class LoadSceneState : ChangeState
    {
        public override void Enter()
        {
            base.Enter();
            // TODO This If statement should go into a "Start" state, this isn't the right place for it.

            if (GameDataController.Instance.NextSceneConfig != null)
            {
                Debug.Log("LoadSceneState: Loading Scene " + GameDataController.Instance.NextSceneConfig.Scene.name);
                GameStateController.Instance.LoadScene(GameDataController.Instance.NextSceneConfig.Scene.name);
            }
            //if (GameStateController.Instance.PlayCutscene)
            //    MessageDispatcher2.Instance.DispatchMsg("StartCutscene", 0f, this.UniqueId, "Game Controller", null);
            //else
            //    Complete();
        }

        public override void Exit()
        {
            Debug.Log("Exiting LoadSceneState");
            StateMachine.GetStateInfo().ChangeScene = false;
            base.Exit();
        }

        // The reason this is in Update is because the Application variables 
        // don't get updated until the next refresh.
        public override void Update()
        {
            base.Update();
            if (!Application.isPlaying)
                return;
            if (!GameStateController.Instance.ChangingScene)
            {
                if (GameStateController.Instance.PlayCutscene)
                {
                    MessageDispatcher2.Instance.DispatchMsg("StartCutscene", 0f, this.UniqueId, "Game Controller", null);
                }
                else
                    Complete();
            }
            //Complete();
            //StateMachine.GetStateInfo().IsComplete = true;
        }

        //public override bool HandleMessage(Telegram telegram)
        //{
        //    if (base.HandleMessage(telegram))
        //        return true;

        //    switch (telegram.Msg)
        //    {
        //        case Enums.Telegrams.ChangeScene:
        //            StateMachine.GetStateInfo().ChangeScene = true;
        //            break;
        //    }
        //    return false;
        //}
    }
}
