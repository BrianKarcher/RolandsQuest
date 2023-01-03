using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/UI/Show Modal")]
    public class ShowModal : ActionBase
    {
        [SerializeField]
        private string _text;
        //public AudioClip AudioClip;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            GameController.Instance.UIManager.DisplayModal(_text);
        }
    }
}
