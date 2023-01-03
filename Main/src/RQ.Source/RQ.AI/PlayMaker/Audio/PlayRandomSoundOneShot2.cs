using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.AI.AtomAction.Audio;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace RQ.AI.PlayMaker.Audio
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Plays a random sound from the list provided")]
    public class PlayRandomSound2 : FsmStateAction
    {
        //[RequiredField]
        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        //public FsmFloat storeResult;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public PlayRandomSound2Atom _atom;
        private IComponentRepository _entity;

        //public override void Reset()
        //{
        //    //gameObject = null;
        //    storeResult = null;
        //    everyFrame = false;
        //}

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
            Finish();
            //DoGetSpeed();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        private void Tick()
        {
            _atom.OnUpdate();
            //if (storeResult.IsNone) return;
            //_getSpeedAtom.OnUpdate();
            //storeResult.Value = _getSpeedAtom.Speed;
        }
    }
}
