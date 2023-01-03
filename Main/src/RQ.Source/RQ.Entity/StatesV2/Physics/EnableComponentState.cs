using RQ.Common;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2.Physics
{
    [AddComponentMenu("RQ/States/State/Physics/Enable Component")]
    public class EnableComponentState : StateBase
    {
        private enum Component
        {
            Usable = 0
        }

        [SerializeField]
        private Component _component;
        [SerializeField]
        private bool _enabled = true;

        public override void Enter()
        {
            base.Enter();
            IBaseObject component = null;
            switch(_component)
            {
                case Component.Usable:
                    component = _componentRepository.Components.GetComponent<UsableComponent>();
                    break;
            }
            MessageDispatcher2.Instance.DispatchMsg("EnableComponent", 0f, this.UniqueId,
                component.UniqueId, _enabled);
            Complete();
        }
    }
}
