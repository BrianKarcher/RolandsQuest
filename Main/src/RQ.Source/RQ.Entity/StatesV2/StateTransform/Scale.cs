using RQ.FSM.V2;
using UnityEngine;

namespace RQ.Entity.StatesV2.StateTransform
{
    [AddComponentMenu("RQ/States/State/Transform/Scale")]
    public class Scale : StateBase
    {
        [SerializeField]
        private float _scaleRate = 1f;
        [SerializeField]
        private float _startScale = 1f;
        [SerializeField]
        private float _maxScale = 30f;
        private float _currentScale;

        public override void Enter()
        {
            base.Enter();
            //AnimationComponent.GetSpriteAnimator().SetColor(_color);
            //_sprite.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
            _componentRepository.transform.localScale = new Vector3(_startScale, _startScale, _startScale);
        }

        public override void Update()
        {
            base.Update();
            if (!Application.isPlaying)
                return;
            if (_currentScale < _maxScale)
            {
                _currentScale += _scaleRate * Time.deltaTime;
                _componentRepository.transform.localScale = new Vector3(_currentScale, _currentScale, _componentRepository.transform.localScale.z);
            }
        }

        public override void Exit()
        {
            base.Exit();
            //AnimationComponent.GetSpriteAnimator().SetColor(Color.white);
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }
    }
}
