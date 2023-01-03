using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Player")]
    [PM.Tooltip("Player Charge Slash state.")]
    public class PlayerChargeSlashState : FsmStateAction
    {
        public PlayerChargeSlashStateAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            //_atom.UnsafeLayerMask = Helper.LayerArrayToLayerMask(UnsafeLayerMask, false);
            _atom.Start(_entity);

            //if (!storeVector.IsNone)
            //    storeVector.Value = _atom.Value;

            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
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
