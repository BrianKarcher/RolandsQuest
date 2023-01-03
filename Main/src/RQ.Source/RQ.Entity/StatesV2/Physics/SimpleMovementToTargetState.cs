using RQ.Physics;
using UnityEngine;

namespace RQ.Entity.StatesV2.Physics
{
    [AddComponentMenu("RQ/States/State/Physics/Simple Movement To Target")]
    public class SimpleMovementToTargetState : SimpleMovementState
    {
        //[SerializeField]
        //private Transform _target;

        //private ISprite _sprite;

        //public override void SetEntity(Transform entity)
        //{
        //    base.SetEntity(entity);
        //    //_sprite = EntityUIBase.GetEntity(entity);
        //    ////sprite = entity.GetComponent<ISprite>();
        //    ////if (sprite == )
        //    ////var entityUIBase = entity.GetComponent<EntityUIBase>();
        //    ////_sprite = entityUIBase.GetRQObject() as ISprite;
        //    //if (_sprite == null)
        //    //    throw new Exception("FSM - Sprite not set.");
        //}

        public override void Enter()
        {
            base.Enter();
            var physicsData = _physicsComponent.GetPhysicsData();
            _steering.Target = _aiComponent.Target.position;
            //_steering.Target = _target.transform.position;
            _steering.TurnOn(behavior_type.seek);
        }

        public override void Exit()
        {
            base.Exit();
            _steering.TurnOff(behavior_type.seek);
        }
    }
}
