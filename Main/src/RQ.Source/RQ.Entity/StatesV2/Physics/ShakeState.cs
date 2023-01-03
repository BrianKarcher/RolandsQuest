using RQ.Model.Interfaces;
using RQ.Physics;
using System.Collections;
using UnityEngine;

namespace RQ.Entity.StatesV2.Physics
{
    [AddComponentMenu("RQ/States/State/Physics/Shake")]
    public class ShakeState : AnimatorState
    {
        [SerializeField]
        private float _shakeAmt;
        [SerializeField]
        private float _shakeInterval;
        private Vector3 _originalPos;

        public override void Enter()
        {
            base.Enter();
            _originalPos = transform.position;
            StartCoroutine("Shake");
        }

        IEnumerator Shake()
        {
            while (true)
            {
                var physicsComponent = _spriteBase.Components.GetComponent<IBasicPhysicsComponent>();
                var pos = physicsComponent.GetWorldPos();
                //float x = UnityEngine.Random.Range(-_shakeAmt, _shakeAmt);
                //float y = UnityEngine.Random.Range(-_shakeAmt, _shakeAmt);
                float x = UnityEngine.Random.value * _shakeAmt * 2 - _shakeAmt;
                float y = UnityEngine.Random.value * _shakeAmt * 2 - _shakeAmt;
                //transform.position += new Vector3(x, y, 0);
                physicsComponent.SetWorldPos(pos + new Vector2D(x, y));
                yield return new WaitForSeconds(_shakeInterval);
            }            
        }        

        public override void Exit()
        {
            base.Exit();
            StopCoroutine("Shake");
            //sprite.GetSteering().TurnOff(behavior_type.wander);
            transform.position = _originalPos;
        }
    }
}
