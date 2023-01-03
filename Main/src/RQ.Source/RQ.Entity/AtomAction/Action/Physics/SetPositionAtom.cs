using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model.Enums;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetPositionAtom : AtomActionBase
    {
        public ActionTarget ActionTarget = ActionTarget.Self;
        public ActionTarget PositionReference = ActionTarget.Self;
        public Vector2 ManualVector;
        public Vector2 Offset;
        private IComponentRepository _target;
        private Vector3 _positionReference;
        //public bool SetToNull = false;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _target = base.GetTarget(ActionTarget);
            if (PositionReference == ActionTarget.ManualVector)
                _positionReference = ManualVector;
            else
                _positionReference = GetTargetPosition(PositionReference);
            if (_target == null)
            {
                Debug.LogError("(SetPositionAtom) - Could not locate target.");
                return;
            }
            if (_positionReference == Vector3.zero)
            {
                Debug.LogError("(SetPositionAtom) - Could not locate position reference.");
                return;
            }

            var pos = _positionReference + (Vector3)Offset;
            var targetPhysicsComponent = _target.GetComponent<PhysicsComponent>();
            targetPhysicsComponent.SetWorldPos(pos);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
