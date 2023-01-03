using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetPhysicsFloatValueAtom : AtomActionBase
    {
        public enum Variable
        {
            MaxForce = 0,
            MaxSpeed = 1,
            ZOffset = 2,
            Timescale = 3,
            CurrentSpeed = 4,
            AirVelocity = 5,
            Gravity = 6
        }
        public Variable _variable;
        public float _value;
        public bool _setToOriginal;
        public string PhysicsAffector;
        private PhysicsComponent _physicsComponent;
        private AltitudePhysicsComponent _altitudePhysicsComponent;
        private BasicPhysicsData _physicsData;
        private BasicPhysicsData _originalPhysicsData;
        private IPhysicsAffector _physicsAffector;
        private AltitudeData _altitudeData;
        
        //public Vector2 Gravity;

        public override void Start(IComponentRepository entity)
        {
            if (_variable == Variable.ZOffset)
            {
                int i = 1;
            }
            base.Start(entity);
            if (_physicsComponent == null)
                _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            if (_physicsAffector == null)
                _physicsAffector = _physicsComponent.GetPhysicsAffector(PhysicsAffector);
            if (_altitudePhysicsComponent == null)
                _altitudePhysicsComponent = entity.Components.GetComponent<AltitudePhysicsComponent>();
            if (_altitudeData == null)
                _altitudeData = _altitudePhysicsComponent.GetAltitudeData();
            if (_physicsComponent == null)
            {
                Debug.Log(entity.name + " has no Physics Component");
                return;
            }
            _physicsData = _physicsComponent.GetPhysicsData();
            _originalPhysicsData = _physicsComponent.GetOriginalPhysicsData();

            if (_setToOriginal)
            {
                _value = GetOriginalValue();
            }
            SetVariable(_value);
        }

        private void SetVariable(float value)
        {
            switch (_variable)
            {
                case Variable.MaxForce:
                    _physicsAffector.MaxForce = value;
                    break;
                case Variable.MaxSpeed:
                    if (_physicsAffector == null)
                        _physicsData.MaxSpeed = value;
                    else
                        _physicsAffector.MaxSpeed = value;
                    break;
                case Variable.ZOffset:
                    _physicsData.ZOffset = value;
                    break;
                case Variable.Timescale:
                    Time.timeScale = value;
                    break;
                case Variable.CurrentSpeed:
                    _physicsComponent.SetVelocity(_physicsComponent.GetVelocity().normalized * value);
                    break;
                case Variable.AirVelocity:
                    _altitudeData.AirVelocity = new Vector2D(_altitudeData.AirVelocity.x, value);
                    break;
                case Variable.Gravity:
                    _altitudeData.Gravity = new Vector2D(_altitudeData.Gravity.x, value);
                    break;
            }
        }

        private float GetOriginalValue()
        {
            //var originalPhysicsData = _physicsComponent.GetOriginalPhysicsData();

            switch (_variable)
            {
                case Variable.MaxForce:
                    return _physicsAffector.OriginalMaxForce;
                case Variable.MaxSpeed:
                    if (_physicsAffector == null)
                        return _physicsData.OriginalMaxSpeed;
                    else
                        return _physicsAffector.OriginalMaxSpeed;
                case Variable.ZOffset:
                    return _originalPhysicsData.ZOffset;
                default:
                    return 0f;
            }
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
