using RQ.Entity.Common;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Model.Enums;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV3.Conditions
{
    public class IsHomesickConditionConfig : StateTransitionConditionBaseConfig
    {
        public override bool TestCondition(IStateMachine stateMachine)
        {
            var entity = stateMachine.GetComponentRepository();
            var _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            var _aiComponent = entity.Components.GetComponent<AIComponent>();
            
            //var closestEntity = EntityController.Instance.ClosestEntityToPosition(_physicsComponent, _entityTarget);
            
            var distanceSq = Vector2D.Vec2DDistanceSq(_physicsComponent.GetFeetWorldPosition(), _aiComponent.GetHomePosition());
            //return _entity.IsAnimationComplete;
            var rtn = distanceSq > _aiComponent.GetHomeSickDistanceSq();
            //if (rtn)
            //{
            //    //int i = 1;
            //}
                //Debug.Break();
            return rtn;
        }
    }
}
