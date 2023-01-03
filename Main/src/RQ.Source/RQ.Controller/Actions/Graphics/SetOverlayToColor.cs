using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Graphics/Set Overlay to Color")]
    public class SetOverlayToColor : ActionBase
    {
        [SerializeField]
        private Color _color;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            SetColor();
        }

        protected virtual void SetColor()
        {
            if (GameController.Instance != null)
                GameController.Instance.GetGraphicsEngine().SetOverlayToColor(_color);
        }
    }
}
