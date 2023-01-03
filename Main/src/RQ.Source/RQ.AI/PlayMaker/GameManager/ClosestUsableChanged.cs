//using PM = HutongGames.PlayMaker;
//using HutongGames.PlayMaker;
//using RQ.Animation.BasicAction.Action;
//using RQ.Entity.Components;
//using RQ.Physics.Components;
//using UnityEngine;
//using RQ.AI.Atom.GameManager;
//using RQ.AI.AtomAction.GameManager;

//namespace RQ.AI.PlayMaker
//{
//    [ActionCategory("RQ.GameManager")]
//    [PM.Tooltip("Did the closest usable change?")]
//    public class ClosestUsableChanged : FsmStateAction
//    {
//        public ClosestUsableChangedAtom _atom;
//        private IComponentRepository _entity;

//        public override void OnEnter()
//        {
//            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
//            _entity = rqSM.GetComponentRepository();
//            _atom.Start(_entity);
//            Tick();
//            Finish();
//        }

//        public override void OnUpdate()
//        {
//            Tick();
//        }

//        public override void OnExit()
//        {
//            base.OnExit();
//            _atom.End();
//        }

//        void Tick()
//        {
//            var result = _atom.OnUpdate();
//            if (result == RQ.AI.AtomActionResults.Success)
//            {
//                Finish();
//            }
//        }
//    }
//}
