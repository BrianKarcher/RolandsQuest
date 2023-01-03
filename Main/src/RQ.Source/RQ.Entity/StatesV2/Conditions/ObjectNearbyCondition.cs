using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Object Nearby")]
    public class ObjectNearbyCondition : StateTransitionConditionBase
    {
        protected PhysicsComponent _physicsComponent;
        [SerializeField]
        private float _radius = .16f;
        [SerializeField]
        private Vector2D _offset = new Vector2D(0,0);

        //[SerializeField]
        //private bool _castRay = false;
        //[SerializeField]
        //private int _layerMask;

        //[SerializeField]
        //private LayerMask _layerMask = 0;

        //public int LayerMask { get { return _layerMask; } set { _layerMask = value; } }
        //private RQ.Physics.SteeringBehaviors.FollowPath _behavior;

        public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        {
            base.SetEntity(entity, stateMachineId, stateInfo);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            //_behavior = _entity.GetSteering().GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            //if (!base.TestCondition(stateMachine))
            //    return false;

            ////var isPositive = false;

            ////var facing = _entity.GetFacingDirectionVector();

            ////var vectorToTarget = _entity.Target.position - _entity.GetPos();

            ////var angle = Vector2.Angle(facing, vectorToTarget);

            //// TODO Add a "sniff test" condition to tell if the target is close, regardless of angle



            //Physics2D.OverlapCircleAll(_physicsComponent.GetPos() + _offset, _radius, );

            ////var raycastHit2D = Physics2D.CircleCastAll(_entity.GetPos() + _offset, _radius, Vector2.zero, _radius, _layerMask);



            //// Within field of view?  Worthy of further consideration
            //if (angle < _physicsComponent.FieldOfView)
            //{
            //    // Using distance squared space because a square root is slow
            //    if (Vector2D.Vec2DDistanceSq(_physicsComponent.GetPos(), _physicsComponent.Target.position) < _physicsComponent.LineOfSight * _physicsComponent.LineOfSight)
            //    {
            //        // TODO Cast a ray to check for a clear line of sight
            //        if (_castRay)
            //        {
            //            //UnityEngine.Physics.Raycast
            //            var raycastHit = UnityEngine.Physics2D.Raycast(_physicsComponent.GetPos(), facing, _physicsComponent.LineOfSight, _layerMask);
            //            if (raycastHit)
            //            {
            //                //if (raycastHit.rigidbody == null)
            //                //    return false;

            //                if (raycastHit.collider == null)
            //                    return false;

            //                //var sprite = raycastHit.rigidbody.GetComponent<EntityUIBase>();
            //                //var sprite = raycastHit.collider.attachedRigidbody.GetComponent<EntityUIBase>();
            //                //if (sprite != null)
            //                //{
            //                    //if (sprite.GetInstanceID() == _entity.Target.GetInstanceID())
            //                    if (raycastHit.collider.tag == _physicsComponent.Target.tag)
            //                    {
            //                        return true;
            //                    }
            //                //}
            //            }
            //        }
            //        else
            //        {
            //            return true;
            //        }
            //    }
            //}

            ////var isFinished = _behavior.Path.IsFinished;

            ////if (isFinished)
            ////{
            ////    string hi = "hi8";
            ////}

            return false;

            //return _entity.IsAnimationComplete;
        }
    }
}
