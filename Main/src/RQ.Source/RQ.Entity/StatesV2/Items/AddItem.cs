using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Item/Add Item")]
    public class AddItem : StateBase
    {
        private UsableComponent _usableComponent;
        public override void SetupState()
        {
            base.SetupState();
            _usableComponent = _spriteBase.Components.GetComponent<UsableComponent>();
        }

        public override void Enter()
        {
            base.Enter();
            MessageDispatcher2.Instance.DispatchMsg("AcquireItem", 0f, this.UniqueId, 
                _componentRepository.UniqueId, null);
            MessageDispatcher2.Instance.DispatchMsg("DisplayAcquireModal", 0f, this.UniqueId,
                _componentRepository.UniqueId, null);
            
            //GameController.Instance.UIManager.DisplayModal(_text);

            MessageDispatcher2.Instance.DispatchMsg("SetGameProgress", 0f, this.UniqueId, _usableComponent.UniqueId, null);
            //_physicsComponent.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
            base.Complete();
        }
    }
}
