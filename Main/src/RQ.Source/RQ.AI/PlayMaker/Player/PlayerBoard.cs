using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.AI;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Player")]
    [PM.Tooltip("Player Board.")]
    public class PlayerBoard : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Finished.")]
        public FsmEvent FinishEvent;

        [UIHint(UIHint.Variable)]
        public FsmGameObject BoardPlatform;

        public PlayerBoardAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (!BoardPlatform.IsNone)
                _atom.SetBoardPlatform(BoardPlatform.Value);
            _atom.Start(_entity);
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public override void OnUpdate()
        {
            DoGetValue();
        }

        void DoGetValue()
        {
            var result = _atom.OnUpdate();
            if (result != AtomActionResults.Running)
            {
                Finish();
                Fsm.Event(FinishEvent);
            }
        }
    }
}
