using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class EnableComponentAtom : AtomActionBase
    {
        public bool EnableOnEnter;
        public bool EnableOnExit;
        public string ComponentName;

        public override void Start(IComponentRepository entity)
        {
            if (ComponentName == "AppStart")
            {
                int i = 1;
            }
            base.Start(entity);
            //_entity = entity;
            //_isRunning = true;
            var component = entity.Components.GetComponent(ComponentName) as IComponentBase;
            component?.gameObject.SetActive(EnableOnEnter);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public override void End()
        {
            base.End();
            var component = _entity.Components.GetComponent<IComponentBase>(ComponentName);
            if (component != null)
                component.gameObject.SetActive(EnableOnExit);
        }
    }
}
