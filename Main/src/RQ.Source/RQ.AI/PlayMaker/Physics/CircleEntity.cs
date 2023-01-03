using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Physics")]
    [PM.Tooltip("Circle an entity.")]
    public class CircleEntity : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [RequiredField]
        [PM.Tooltip("Game Object to circle.")]
        public FsmGameObject Target;

        public CircleEntityAtom _atom;
        private IComponentRepository _entity;

        public override void OnPreprocess()
        {
            base.OnPreprocess();
            Fsm.HandleFixedUpdate = true;
        }

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.SetTarget(Target.Value);
            _atom.Start(_entity);
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            _atom.OnUpdate();
        }

        //public override void OnUpdate()
        //{
        //    DoGetValue();
        //}

        //void DoGetValue()
        //{           
        //    _atom.OnUpdate();

        //}
    }
}
