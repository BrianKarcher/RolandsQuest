using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Game Manager/Load First Scene")]
    public class LoadFirstScene : ActionBase
    {
        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            Debug.LogError("LoadFirstScene called");
            

            base.Complete();
        }
    }
}
