using System.Collections.Generic;
using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Physics")]
    [PM.Tooltip("Follow the supplied path.")]
    public class FollowPath : FsmStateAction
    {
        [UIHint(UIHint.FsmEvent)]
        public FsmEvent Failed;
        [UIHint(UIHint.FsmEvent)]
        public FsmEvent Complete;
        [UIHint(UIHint.Variable)]
        public FsmArray vectorArray;
        public FollowPathAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();

            List<Vector3> newPath = new List<Vector3>(vectorArray.vector4Values.Length);
            for (int i = 0; i < vectorArray.vector4Values.Length; i++)
            {
                newPath.Add((Vector2) vectorArray.vector4Values[i]);
            }
            _atom.SetPath(newPath);

            //_atom.Path = vectorArray.vector4Values.Select(i => (Vector2)i).ToArray();
            //_atom.Failed = () =>
            //{
            //    Fsm.Event(Failed);
            //};
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
                //vectorArray.vector4Values = _atom.Path.Select(i => new Vector4(i.x, i.y)).ToArray();
                Finish();
                Fsm.Event(Complete);
            }
        }
    }
}
