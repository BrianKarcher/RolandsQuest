using System;
using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Animation")]
    [PM.Tooltip("Plays the animation by the given name. 'Wait for Animation Completion' will only finish the action when the animation completes.")]
    public class PlayAnimation : FsmStateAction
    {
        [UIHint(UIHint.FsmString)]
        public FsmString _animation;

        [SerializeField]
        public AnimationAtom _animationAtom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (!_animation.IsNone && !String.IsNullOrEmpty(_animation.Value))
                _animationAtom._animation = _animation.Value;
            _animationAtom.Start(_entity);
            Tick();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        public override void OnExit()
        {
            base.OnExit();
            _animationAtom.End();
        }

        void Tick()
        {
            var result = _animationAtom.OnUpdate();
            if (result == RQ.AI.AtomActionResults.Success)
            {
                Finish();
            }
        }
    }
}
