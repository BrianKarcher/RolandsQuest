using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    public enum VariableType
    {
        SteeringOffset = 0,
        //GetOffsetPursuitLocation = 1
    }

    [Serializable]
    public class GetVector3VariableAtom : AtomActionBase
    {
        public VariableType VariableType;
        [HideInInspector]
        public Vector3 Variable;
        private SteeringBehaviorManager _steering;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _steering = physicsComponent.GetSteering() as SteeringBehaviorManager;
            
            //_steering.TurnOn(Physics.behavior_type.flee);
        }

        private void Set()
        {
            switch (VariableType)
            {
                case VariableType.SteeringOffset:
                    _steering.Offset = Variable;
                    break;
                //case VariableType.FootPosition:
                //case VariableType.GetOffsetPursuitLocation:
                //    _steering.TargetAgent1 + _steering.Offset;
                //    leader.GetWorldPos2D() + offset;
                //    break;
            }
        }

        public override AtomActionResults OnUpdate()
        {
            Set();
            return AtomActionResults.Running;
        }
    }
}
