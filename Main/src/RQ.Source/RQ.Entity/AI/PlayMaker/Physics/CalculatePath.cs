using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Physics")]
    [PM.Tooltip("Calculate a path to target.")]
    public class CalculatePath : FsmStateAction
    {
        [UIHint(UIHint.FsmEvent)]
        public FsmEvent Failed;
        [UIHint(UIHint.FsmEvent)]
        public FsmEvent Complete;
        [UIHint(UIHint.Variable)]
        public FsmArray vectorArray;
        public CalculatePathAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.SetFailed(() =>
            {
                Fsm.Event(Failed);
            });
            _atom.Start(_entity);
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public override void OnUpdate()
        {
            if (Finished)
                return;
            var result = _atom.OnUpdate();
            if (result == RQ.AI.AtomActionResults.Success)
            {
                Vector4[] vector4s = new Vector4[_atom.Path.Count];
                for (int i = 0; i < _atom.Path.Count; i++)
                {
                    var tempVector = _atom.Path[i];
                    vector4s[i] = new Vector4(tempVector.x, tempVector.y);
                }
                vectorArray.vector4Values = vector4s;
                //vectorArray.vector4Values = _atom.Path.Select(i => new Vector4(i.x, i.y)).ToArray();
                Finish();
                Fsm.Event(Complete);
            }
        }
    }
}
