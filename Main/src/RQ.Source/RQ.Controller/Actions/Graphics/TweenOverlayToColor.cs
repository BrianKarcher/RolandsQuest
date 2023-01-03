using RQ.Model;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Graphics/Tween Overlay to Color")]
    public class TweenOverlayToColor : ActionBase
    {
        [SerializeField]
        private TweenToColorInfo _overlayColor = null;
        //public GameObject GameObject;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            if (_overlayColor != null)
            {
                if (_overlayColor.Active)
                {
                    PerformTween();
                }
            }
            //GameObject.DestroyObject(GameObject);
        }

        protected virtual void PerformTween()
        {
            if (GameController.Instance != null)
                GameController.Instance.GetGraphicsEngine().TweenOverlayToColor(_overlayColor);
        }
    }
}
