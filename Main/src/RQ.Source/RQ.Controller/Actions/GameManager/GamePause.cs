using RQ.Messaging;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Game Manager/Pause")]
    public class GamePause : ActionBase
    {
        [SerializeField]
        private bool _pauseOnEnter = true;
        [SerializeField]
        private bool _pauseOnExit = false;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            PerformAction(_pauseOnEnter);
            //GameController._instance.AudioSource.PlayOneShot(AudioClip);
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            PerformAction(_pauseOnExit);
        }

        private void PerformAction(bool pause)
        {
            if (pause)
                Pause();
            else
                Unpause();
        }

        private void Pause()
        {
            MessageDispatcher2.Instance.DispatchMsg("Pause", 0f, string.Empty, "Game Controller", null);
        }

        private void Unpause()
        {
            MessageDispatcher2.Instance.DispatchMsg("Unpause", 0f, string.Empty, "Game Controller", null);
        }
    }
}
