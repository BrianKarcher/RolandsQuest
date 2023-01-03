using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction;
using RQ.AI.Action;
using RQ.AI;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Quest")]
    [PM.Tooltip("Set Quest Status.")]
    public class SetQuestStatus : FsmStateAction
    {
        public SetQuestStatusAtom _atom;
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

        public override void OnUpdate()
        {
            if (Tick() != AtomActionResults.Running)
            {
                Finish();
            }
        }

        AtomActionResults Tick()
        {
            return _atom.OnUpdate();
        }
    }
}
