using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction;
using RQ.AI.AtomAction.Physics;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Physics")]
    [PM.Tooltip("Shoot an object")]
    public class ShootObject2 : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Game Object to shoot.")]
        public FsmGameObject GameObject;

        public ShootObject2Atom _atom;
        private IComponentRepository _entity;
        private Coroutine _coroutine;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            //_atom.UnsafeLayerMask = Helper.LayerArrayToLayerMask(UnsafeLayerMask, false);
            _atom.SetObject(GameObject.Value);
            _atom.Start(_entity);
            _coroutine = StartCoroutine(_atom.ShootObject());
            //Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            StopCoroutine(_coroutine);
            _atom.End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            var result = _atom.OnUpdate();
            if (result == RQ.AI.AtomActionResults.Success)
                Finish();
        }
    }
}
