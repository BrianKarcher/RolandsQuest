using RQ.Animation.BasicAction.Action;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Send Self to Pool")]
    public class SendSelfToPool : ActionBase
    {
        public SendSelfToPoolAtom _atom;
        //public Transform Transform;
        //public List<Transform> GameObjects;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            _atom.Start(GetEntity());

            //GameObject.DestroyObject(GameObject);
            //if (Transform == null)
            //{
            //    var entity = GetEntity();
            //    if (entity != null)
            //        Transform = entity.transform;
            //}
            //if (Transform != null)
            //    Kill(Transform);
            //if (GameObjects != null)
            //{
            //    foreach (var go in GameObjects)
            //    {
            //        Kill(go);
            //    }
            //}
        }
    }
}
