using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetPhysicsVectorValueAtom : AtomActionBase
    {
        public enum Variable
        {
            AffectorVelocity = 0
        }
        public Variable _variable;
        public Vector2 _value;
        public bool _setToOriginal;
        public string AffectorName;
        private PhysicsComponent _physicsComponent;
        private BasicPhysicsData _physicsData;
        //public Vector2 Gravity;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            if (_physicsComponent == null)
            {
                Debug.Log(entity.name + " has no Physics Component");
                return;
            }
            _physicsData = _physicsComponent.GetPhysicsData();
            //if (_resetToOriginal)
            //    _speed = physicsData.OriginalMaxSpeed;
            //physicsData.MaxSpeed = _speed;
            if (_setToOriginal)
            {
                _value = GetOriginalValue();
            }
            SetVariable(_value);
        }

        private void SetVariable(Vector2 value)
        {
            switch (_variable)
            {
                case Variable.AffectorVelocity:
                    _physicsComponent.SetVelocity(value);
                    break;
            }
        }

        private Vector2 GetOriginalValue()
        {
            var originalPhysicsData = _physicsComponent.GetOriginalPhysicsData();

            switch (_variable)
            {
                case Variable.AffectorVelocity:
                    Debug.LogError("Get Velocity original value not implemented");
                    return Vector2.zero;
                //return _physicsComponent.GetPhysicsAffector(AffectorName).OriginalVelocity;
                default:
                    return Vector2.zero;
            }
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
