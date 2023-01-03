using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.FSM.V2.States;
using RQ.Messaging;
using UnityEngine;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Disable Input")]
    public class DisableInputState : ChangeState
    {
        public override void Enter()
        {
            base.Enter();
            EnableInput(false);
        }

        public override void Exit()
        {
            base.Exit();
            EnableInput(true);
        }

        private void EnableInput(bool enable)
        {
            // If the game is not running, not much to do.
            if (GameDataController.Instance.Data == null)
                return;

            var mainCharacter = EntityContainer._instance.GetMainCharacter();

            // Let the running scene control the players input state
            var runningSceneUniqueId = GameDataController.Instance.Data.RunningSequenceUniqueId;
            if (runningSceneUniqueId != null)
                return;
            //{

            MessageDispatcher2.Instance.DispatchMsg("SetInputState", 0f, this.UniqueId, mainCharacter.UniqueId, enable);
            //}
        }
    }
}
