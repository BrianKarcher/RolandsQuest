using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Instantiates a prefab and sets its velocity.")]
    public class ShootObject : FsmStateAction
    {
        [UIHint(UIHint.FsmFloat)]
        public FsmFloat MinSpeed;
        [UIHint(UIHint.FsmFloat)]
        public FsmFloat MaxSpeed;

        public ShootObjectAtom _atom;
        private IComponentRepository _entity;
        private Coroutine _coroutine;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (!MinSpeed.IsNone)
                _atom._minSpeed = MinSpeed.Value;
            if (!MaxSpeed.IsNone)
                _atom._maxSpeed = MaxSpeed.Value;
            _atom.Start(_entity);
            _coroutine = StartCoroutine(_atom.ShootObject());
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
