using RQ.Input;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Entity/Enable Input")]
    public class EnableInput : ActionBase
    {
        [SerializeField]
        private bool _enable;
        //public AudioClip AudioClip;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            var inputComponent = GetEntity().Components.GetComponent<InputComponent>();
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, inputComponent.UniqueId, 
                Enums.Telegrams.SetEnabled, _enable);
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, "Game Controller",
            //    Enums.Telegrams.PlayOneShot, AudioClip);
            //GameController._instance.AudioSource.PlayOneShot(AudioClip);
        }
    }
}
