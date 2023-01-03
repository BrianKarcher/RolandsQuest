using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Sprite Color")]
    public class SpriteColor : AnimatorState
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private Color _color;

        public override void Enter()
        {
            base.Enter();
            AnimationComponent.GetSpriteAnimator().SetColor(_name, _color);
            //_sprite.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
        }

        public override void Exit()
        {
            base.Exit();
            AnimationComponent.GetSpriteAnimator().RemoveColor(_name);
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }
    }
}
