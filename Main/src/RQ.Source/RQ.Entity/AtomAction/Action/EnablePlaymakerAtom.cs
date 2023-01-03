using RQ.Common.UniqueId;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.Action
{
    public class EnablePlaymakerAtom : AtomActionBase
    {
        public string Name;
        private PlayMakerStateMachineComponent _playMakerComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _playMakerComponent = entity.Components.GetComponent<PlayMakerStateMachineComponent>(Name);
            if (_playMakerComponent == null)
                return;
            _playMakerComponent.gameObject.SetActive(true);
            _playMakerComponent.StartFsm();
        }

        public override void End()
        {
            base.End();
            if (_playMakerComponent == null)
                return;
            _playMakerComponent.StopFsm();
            _playMakerComponent.gameObject.SetActive(false);            
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }
    }
}
