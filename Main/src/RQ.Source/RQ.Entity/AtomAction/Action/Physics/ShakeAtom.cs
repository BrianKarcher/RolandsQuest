using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model;
using RQ.Model.Interfaces;
using RQ.Physics;
using UnityEngine;

namespace RQ.AI.Action
{
    public class ShakeAtom : AtomActionBase
    {
        //public Vector2 force;
        //public string _physicsComponentName;
        //[SerializeField]
        //public ShootTarget _shootTarget = ShootTarget.Random;
        private IBasicPhysicsComponent _physicsComponent;
        public float _shakeAmt;
        public float _shakeInterval;
        private ICameraClass _cameraClass;
        private AIComponent _aiComponent;
        private float _timer = 0f;
        private Vector2D _startingPos;

        //public override 

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<IBasicPhysicsComponent>();
            _aiComponent = entity.Components.GetComponent<AIComponent>();
            //_physicsComponent.GetPhysicsData().Velocity += (Vector2D)force;
            _cameraClass = entity.Components.GetComponent<ICameraClass>();
            _isRunning = false;
            _cameraClass.ShakeAmount = _shakeAmt;
            _cameraClass.ShakeInterval = _shakeInterval;
            _cameraClass.SetClamping(false);
            _startingPos = _physicsComponent.GetWorldPos();
            //_cameraClass.StartCoroutine("Shake");
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            _timer += Time.deltaTime;
            if (_timer < _shakeInterval)
                return;
            _timer = 0f;
            //var pos = _physicsComponent.GetWorldPos();
            //float x = UnityEngine.Random.Range(-_shakeAmt, _shakeAmt);
            //float y = UnityEngine.Random.Range(-_shakeAmt, _shakeAmt);
            float x = UnityEngine.Random.value * _shakeAmt * 2f - _shakeAmt;
            float y = UnityEngine.Random.value * _shakeAmt * 2f - _shakeAmt;
            //transform.position += new Vector3(x, y, 0);
            //physicsComponent.SetWorldPos(pos + new Vector2D(x, y));
            //_physicsComponent.GetPhysicsData().ExternalVelocity = new Vector2D(x, y) * 50f;
            _physicsComponent.GetPhysicsData().Offset = new Vector2D(x, y);
            //var targetLocation = (Vector2D)_aiComponent.Target.transform.position + _physicsComponent.GetPhysicsData().Offset;
            var targetLocation = _startingPos + _physicsComponent.GetPhysicsData().Offset;

            MessageDispatcher2.Instance.DispatchMsg("SetPos", 0f, _entity.UniqueId, _physicsComponent.UniqueId,
                targetLocation);
            MessageDispatcher2.Instance.DispatchMsg("CameraUpdate", 0f, _entity.UniqueId, _entity.UniqueId, null);
        }

        public override void End()
        {
            base.End();
            //_cameraClass.StopCoroutine("Shake");
            _physicsComponent.GetPhysicsData().Offset = Vector2D.Zero();
            _cameraClass.SetClamping(true);
        }
    }
}
