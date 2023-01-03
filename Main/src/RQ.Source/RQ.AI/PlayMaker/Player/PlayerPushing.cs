using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ;
using RQ.AI;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction;
using RQ.AI.AtomAction.Player;

namespace RQ.AI.PlayMaker.Player
{
    [ActionCategory("RQ.Player")]
    [PM.Tooltip("Player Push.")]
    public class PlayerPushing : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Object affected by a push or jump.")]
        [RequiredField]
        public FsmGameObject PushObject;

        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("Object affected by a push or jump.")]
        //[RequiredField]
        //public FsmInt PushDirection;

        public PlayerPushingAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            //_atom.SetDirection((Direction)PushDirection.Value);
            _atom.SetPushableObject(PushObject.Value);
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
            if (result != AtomActionResults.Running)
            {
                Finish();
                //AffectedObject.Value = _atom.OtherObject;
                //switch (_atom.NextEvent)
                //{
                //    case "Push":
                //        Fsm.Event(PUshEvent);
                //        break;
                //    case "JumpDown":
                //        Fsm.Event(JumpDownEvent);
                //        break;
                //}
            }
        }

        //void DoGetValue()
        //{           
        //    _atom.OnUpdate();

        //}
    }
}
