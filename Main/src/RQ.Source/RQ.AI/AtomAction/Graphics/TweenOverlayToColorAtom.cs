using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model;
using System;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class TweenOverlayToColorAtom : AtomActionBase
    {
        public TweenToColorInfo _overlayColor = null;
        private float _endTime;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            MessageDispatcher2.Instance.DispatchMsg("TweenToColor", 0f, string.Empty, "Graphics Engine", _overlayColor);
            _endTime = UnityEngine.Time.time + _overlayColor.Delay + _overlayColor.Duration;
        }

        public override void End()
        {
        }

        public override AtomActionResults OnUpdate()
        {
            if (_endTime > UnityEngine.Time.time)
                return AtomActionResults.Success;
            return AtomActionResults.Running;
        }
    }
}
