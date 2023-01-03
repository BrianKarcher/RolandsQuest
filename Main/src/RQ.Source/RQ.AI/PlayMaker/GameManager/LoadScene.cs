using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;
using RQ.AI.Atom.GameManager;

namespace RQ.AI.PlayMaker
{
    [ActionCategory("RQ.GameManager")]
    [PM.Tooltip("Loads a scene.")]
    public class LoadScene : FsmStateAction
    {
        public LoadSceneAtom _atom;
        private IComponentRepository _entity;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when loading a cutscene.")]
        public FsmEvent LoadCutscene;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when load finished.")]
        public FsmEvent Complete;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
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
            if (_atom.GetStartCutscene())
            {
                Fsm.Event(LoadCutscene);
                return;
            }
            if (result == RQ.AI.AtomActionResults.Success)
            {
                Fsm.Event(Complete);
                Finish();                
            }
        }
    }
}
