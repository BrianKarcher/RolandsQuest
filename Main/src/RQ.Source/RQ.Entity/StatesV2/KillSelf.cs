using RQ.Animation.BasicAction.Action;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Kill Self")]
    public class KillSelf : AnimatorState
    {
        [SerializeField]
        private KillSelfAtom _killSelfAtom;

        public override void Enter()
        {
            base.Enter();
            _killSelfAtom.Start(_componentRepository);
            Complete();
        }
    }
}
