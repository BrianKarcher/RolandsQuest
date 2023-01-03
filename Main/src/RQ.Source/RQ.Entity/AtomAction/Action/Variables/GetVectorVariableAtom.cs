using RQ.AI;
using RQ.Entity;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Entity.Data;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Model.Enums;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class GetVectorVariableAtom : AtomActionBase
    {
        public Vector2 Value;
        [SerializeField]
        public VectorVariableEnum VariableToSet;
        public string VariableName;
        public string PhysicsAffector;
        private PhysicsComponent _physicsComponent;
        //private AIComponent _aiComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_physicsComponent == null)
                _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            //if (_aiComponent == null)
            //    _aiComponent = entity.Components.GetComponent<AIComponent>();
            SetValue();
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        private void SetValue()
        {
            switch (VariableToSet)
            {
                case VectorVariableEnum.Velocity:
                    _physicsComponent.SetVelocity(Value);
                    break;
                case VectorVariableEnum.Force:
                    _physicsComponent.GetPhysicsAffector(PhysicsAffector).Force = Value;
                    break;
                case VectorVariableEnum.FootPosition:
                    _physicsComponent.SetFeetWorldPosition(Value);
                    break;
            }
        }
    }
}
