using RQ.Common.UniqueId;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.Action
{
    public class EnableBehaviorTreeAtom : AtomActionBase
    {
        [SerializeField]
        [UniqueIdentifier]
        public string _uniqueId;
        private BehaviorTreeComponent _btComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _btComponent = entity.Components.GetComponent<BehaviorTreeComponent>();     
            if (_btComponent != null)
                _btComponent.EnableBT();
        }

        public override void End()
        {
            base.End();
            if (_btComponent != null)
                _btComponent.DisableBT();
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }
    }
}
