using RQ.Animation;
using RQ.AnimationV2;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Graphics/Sprite Color")]
    public class SpriteColor : ActionBase
    {
        [SerializeField]
        private AnimationComponent _animationComponent;

        [SerializeField]
        private SpriteRendererBase2 _sprite;

        [SerializeField]
        private string _name;

        [SerializeField]
        private Color _color;

        public override void InitAction()
        {
            base.InitAction();
            if (_animationComponent == null)
            {
                for (int i = 0; i < _animationComponents.Count; i++)
                {
                    var aniamtionComponent = _animationComponents[i] as AnimationComponent;
                    if (aniamtionComponent.IsMain())
                    {
                        _animationComponent = aniamtionComponent;
                        break;
                    }
                }

                //_animationComponent = _animationComponents.FirstOrDefault(i => i.IsMain());
            }
            if (_sprite == null)
                _sprite = _animationComponent.GetSpriteAnimator() as SpriteRendererBase2;
        }

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            _sprite.SetColor(_name, _color);
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            _sprite.RemoveColor(_name);
        }
    }
}
