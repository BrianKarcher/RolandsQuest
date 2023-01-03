using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;
using RQ.AI.Action.Damage;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Damage")]
    [PM.Tooltip("What happens when user runs into this entity.")]
    public class SetCollisionDamageType : FsmStateAction
    {
        [SerializeField]
        public SetCollisionActionTypeAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
