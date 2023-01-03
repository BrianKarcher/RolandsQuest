using RQ.Enums;
using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.Entity.StatesV2.Physics
{
    [AddComponentMenu("RQ/States/State/Simple movement to random targets state")]
    public class SimpleMovementToRandomTargetsState : SimpleMovementToTargetState
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
            SetupState();
            ChooseTarget();
            base.Enter();
        }

        protected void ChooseTarget()
        {
            if (_aiComponent == null)
            {
                throw new Exception("AI Component in " + _componentRepository.name + " is null");
            }
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
                Telegrams.ChooseRandomTarget, null);
        }
    }
}
