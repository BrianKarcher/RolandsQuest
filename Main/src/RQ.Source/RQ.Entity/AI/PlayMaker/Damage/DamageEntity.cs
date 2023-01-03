using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;
using RQ.Entity.AtomAction.Condition;
using RQ.AI.Conditions;
using RQ.AI.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Damage")]
    [PM.Tooltip("Processes damage to the supplied entity.")]
    public class DamageEntity : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Entity to damage.")]
        public FsmGameObject EntityToDamage;

        [SerializeField]
        public DamageEntityAtom _atom;

        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.SetEntityToDamage(EntityToDamage.Value);
            _atom.Start(_entity);
            //Tick();
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        //public override void OnUpdate()
        //{
        //    Tick();
        //}

        //void Tick()
        //{
        //    if (Finished)
        //        return;
        //    var result = _damageBounceAtom.OnUpdate();
        //    if (result == RQ.AI.AtomActionResults.Success)
        //    {
        //        Finish();
        //        //Fsm.Event(Damaged);
        //    }
        //}
    }
}
