using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Instantiates a prefab facing the direction of the entity.")]
    public class InstantiatePrefab : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Object to instantiate.")]
        public FsmGameObject Instantiate;

        //[RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Where to store new prefab.")]
        public FsmGameObject storeResult;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public InstantiatePrefabAtom _instantiatePrefabAtom;
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
            if (!Instantiate.IsNone && Instantiate.Value != null)
            {
                _instantiatePrefabAtom.SetGameObject(Instantiate.Value);
            }
            _instantiatePrefabAtom.Start(_entity);
            if (!storeResult.IsNone)
                storeResult.Value = _instantiatePrefabAtom.GetPrefab();
            if (!_instantiatePrefabAtom.IsRunning())
                Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _instantiatePrefabAtom.End();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        private void Tick()
        {
            var result = _instantiatePrefabAtom.OnUpdate();
            if (result != RQ.AI.AtomActionResults.Running)
                Finish();
        }
    }
}
