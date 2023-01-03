using RQ.Messaging;
using RQ.Physics;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Physics/Lock On Target")]
    public class LockOnTarget : ActionBase
    {
        //public override void Act(Component otherRigidBody)
        //{
        //    base.Act(otherRigidBody);
        //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, "Game Controller",
        //        Enums.Telegrams.PlayOneShot, AudioClip);
        //    //GameController._instance.AudioSource.PlayOneShot(AudioClip);
        //}

        public void LateUpdate()
        {
            if (!Application.isPlaying)
                return;
            var targetLocation = (Vector2D) _aiComponent.Target.transform.position;
            MessageDispatcher2.Instance.DispatchMsg("SetPos", 0f, this.UniqueId, _physicsComponent.UniqueId,
                targetLocation);
            MessageDispatcher2.Instance.DispatchMsg("CameraUpdate", 0f, this.UniqueId, GetEntity().UniqueId, null);
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
            //    Telegrams.SetPos, targetLocation);
        }
    }
}
