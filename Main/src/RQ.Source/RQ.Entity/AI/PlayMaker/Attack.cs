using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;
using RQ.Entity.AtomAction.Action;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Returns Success if Button is pressed.")]
    public class Attack : FsmStateAction
    {
        public AttackAtom _attackAtom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _attackAtom.Start(_entity);
        }

        public override void OnExit()
        {
            base.OnExit();
            _attackAtom.End();
        }

        public override void OnUpdate()
        {
            if (Finished)
                return;
            if (_attackAtom.OnUpdate() == RQ.AI.AtomActionResults.Success)
            {
                Debug.Log("Attack Finished");
                Finish();
            }
        }
    }
}
