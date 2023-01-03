using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Atom.GameManager;
using UnityEngine;

namespace RQ.AI.PlayMaker
{
    [ActionCategory("RQ.GameManager")]
    [PM.Tooltip("Initiates a new game.")]
    public class BeginNewGame : FsmStateAction
    {
        public BeginNewGameAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            Debug.LogError("BeginNewGame Entered");
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
            Finish();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        void Tick()
        {
            var result = _atom.OnUpdate();
            if (result == RQ.AI.AtomActionResults.Success)
            {
                Finish();
            }
        }
    }
}
