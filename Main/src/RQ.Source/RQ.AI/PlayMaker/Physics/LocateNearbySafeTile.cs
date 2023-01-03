using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Physics")]
    [PM.Tooltip("Locate a safe tile to place the player.")]
    public class LocateNearbySafeTile : FsmStateAction
    {
        [UIHint(UIHint.Layer)]
        [Tooltip("Layers to avoid.")]
        public FsmInt[] UnsafeLayerMask;

        [UIHint(UIHint.FsmVector2)]
        public FsmVector2 storeVector;

        public LocateNearbySafeTileAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.UnsafeLayerMask = Helper.LayerArrayToLayerMask(UnsafeLayerMask, false);
            _atom.Start(_entity);

            if (!storeVector.IsNone)
                storeVector.Value = _atom.Value;

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
