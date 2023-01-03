using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction.Variables;

namespace RQ.AI.PlayMaker.Variables
{
    [ActionCategory("RQ.Variable")]
    [PM.Tooltip("Gets the value and stores it in a RQ Variable.")]
    public class GetGameObjectVariable : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("The variable to store into")]
        public FsmGameObject GameObject;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public GetGameObjectVariableAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
            DoGetValue();
            //if (!everyFrame)
            //{
                //DoGetValue();
                Finish();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public override void OnUpdate()
        {
            DoGetValue();
        }

        private void DoGetValue()
        {
            var gameObject = GameObject.IsNone ? null : GameObject.Value;
            
            _atom.SetVariable(gameObject);
            //_atom.Value = GameObject.Value;
            _atom.OnUpdate();
            //storeResult.Value = _atom.Value;
        }
    }
}
