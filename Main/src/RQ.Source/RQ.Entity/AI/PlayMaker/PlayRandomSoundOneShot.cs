using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.Entity.AtomAction.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Plays a random sound from the list provided")]
    public class PlayRandomSoundOneShot : FsmStateAction
    {
        //[RequiredField]
        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        //public FsmFloat storeResult;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public PlayRandomSoundOneShotAtom _soundAtom;
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
            _soundAtom.Start(_entity);
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
            _soundAtom.End();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        private void Tick()
        {
            _soundAtom.OnUpdate();
            //if (storeResult.IsNone) return;
            //_getSpeedAtom.OnUpdate();
            //storeResult.Value = _getSpeedAtom.Speed;
        }
    }
}
