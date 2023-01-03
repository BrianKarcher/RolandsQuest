using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;
using RQ.Entity.AtomAction.Condition;
using RQ.AI.Conditions;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Gets whether an entity has been damaged.")]
    public class IsDamaged : FsmStateAction
    {
        //[RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when message is received.")]
        public FsmEvent Damaged;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        [SerializeField]
        public IsDamagedConditionAI _isDamagedAtom;
        private IComponentRepository _entity;

        //public override void Reset()
        //{
        //    //gameObject = null;
        //    //storeResult = null;
        //    //everyFrame = false;
        //}

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            if (rqSM == null)
                Debug.LogError("PlayMaker SM Component Not Found on " + this.Name);
            _entity = rqSM.GetComponentRepository();
            _isDamagedAtom.Start(_entity);
            Tick();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
            _isDamagedAtom.End();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        void Tick()
        {
            if (Finished)
                return;
            var result = _isDamagedAtom.OnUpdate();
            if (result == RQ.AI.AtomActionResults.Success)
            {
                //Finish();
                Fsm.Event(Damaged);
            }
        }
    }
}
