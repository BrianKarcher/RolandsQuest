using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction;
using RQ.AI.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Player")]
    [PM.Tooltip("Locate nearby liftable object.")]
    public class LocateLiftable : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Nearby liftable object store.")]
        public FsmGameObject StoreObject;

        [UIHint(UIHint.Layer)]
        [PM.Tooltip("Obstacle layer.")]
        public FsmInt[] ObstacleLayer;

        [UIHint(UIHint.Layer)]
        [PM.Tooltip("Layer Mask.")]
        public FsmInt[] Layers;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when no liftable found.")]
        public FsmEvent NoneFoundEvent;

        public LocateLiftableAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            int layerMask = 0;
            for (int i = 0; i < ObstacleLayer.Length; i++)
            {
                layerMask |= 1 << ObstacleLayer[i].Value;
            }
            _atom.SetObstacleLayerMask(layerMask);
            layerMask = 0;
            for (int i = 0; i < Layers.Length; i++)
            {
                layerMask |= 1 << Layers[i].Value;
            }
            _atom.SetLayerMask(layerMask);
            //_atom.UnsafeLayerMask = Helper.LayerArrayToLayerMask(UnsafeLayerMask, false);
            _atom.Start(_entity);
            var gameObject = _atom.GetLiftableObject();
            StoreObject.Value = gameObject;
            if (gameObject == null)
            {
                Fsm.Event(NoneFoundEvent);
            }

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
