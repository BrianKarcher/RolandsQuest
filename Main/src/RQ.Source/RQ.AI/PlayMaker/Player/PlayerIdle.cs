using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.AI;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction;
using RQ.AI.AtomAction.Player;

namespace RQ.AI.PlayMaker.Player
{
    [ActionCategory("RQ.Player")]
    [PM.Tooltip("Player Idle.")]
    public class PlayerIdle : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Object affected by a push or jump.")]
        public FsmGameObject StoreObject;

        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("Push Direction.")]
        //[RequiredField]
        //public FsmInt StorePushDirection;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when player is pushing a block.")]
        public FsmEvent PushEvent;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when player is jumping down.")]
        public FsmEvent JumpDownEvent;

        public PlayerIdleAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public override void OnUpdate()
        {
            var result = _atom.OnUpdate();
            if (result == AtomActionResults.Success)
            {
                StoreObject.Value = _atom.GetOtherObject();
                //StorePushDirection.Value = (int)_atom.GetDirection();
                switch (_atom.GetNextEvent())
                {
                    case "Push":
                        Fsm.Event(PushEvent);
                        break;
                    case "JumpDown":
                        Fsm.Event(JumpDownEvent);
                        break;
                }
            }
        }
    }
}
