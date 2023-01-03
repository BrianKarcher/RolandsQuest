using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.FSM.V2.States;
using RQ.Messaging;
using UnityEngine;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Pauseable")]
    public class PausableState : ChangeState
    {
        [SerializeField]
        private bool _pause = true;

        public override void Enter()
        {
            //Debug.Log("Entering Pause state");
            base.Enter();

            if (_pause)
            {
                EnableInput(false);
                Time.timeScale = 0.0000001f;
            }
        }

        public override void Exit()
        {
            //Debug.Log("Exiting Pause state");
            base.Exit();
            if (_pause)
            {
                EnableInput(true);
                Time.timeScale = 1.0f;
            }
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
