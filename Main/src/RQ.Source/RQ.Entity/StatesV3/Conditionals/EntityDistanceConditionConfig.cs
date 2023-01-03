using RQ.Entity.Common;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Model.Enums;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV3.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Entity Distance")]
    public class EntityDistanceConditionConfig : StateTransitionConditionBaseConfig
    {
        [SerializeField]
        private EntityTarget _entityTarget = EntityTarget.Enemy;
        //[SerializeField]
        //[Tag]
        //private string _tag = null;
        [SerializeField]
        private float _distance = 0f;
        //private float _distanceSq;
        //protected PhysicsComponent _physicsComponent;

        //public override void Awake()
        //{
        //    //base.Awake();
        //    _distanceSq = _distance * _distance;
        //}


        //public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        //{
        //    base.SetEntity(entity, stateMachineId, stateInfo);
        //    _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            
        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            var entity = stateMachine.GetComponentRepository();
            var _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            var closestEntity = EntityController.Instance.ClosestEntityToPosition(_physicsComponent, _entityTarget);
            
            var distanceSq = Vector2D.Vec2DDistanceSq(_physicsComponent.GetWorldPos(), closestEntity.transform.position);
            //return _entity.IsAnimationComplete;
            var rtn = distanceSq < _distance * _distance;
            //if (rtn)
            //{
            //    //int i = 1;
            //}
                //Debug.Break();
            return rtn;
        }
    }
}
