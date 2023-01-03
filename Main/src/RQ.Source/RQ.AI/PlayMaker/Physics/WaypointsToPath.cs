using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction.Physics;
using UnityEngine;

namespace RQ.AI.PlayMaker.Physics
{
    [ActionCategory("RQ.Physics")]
    [PM.Tooltip("Convert waypoints to a path - used by FollowPath.")]
    public class WaypointsToPath : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [RequiredField]
        public FsmArray StoreVectorArray;

        public WaypointsToPathAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);

            if (!StoreVectorArray.IsNone)
            {
                Vector4[] vector4s = new Vector4[_atom.Path.Length];
                for (int i = 0; i < _atom.Path.Length; i++)
                {
                    var tempVector = _atom.Path[i];
                    vector4s[i] = new Vector4(tempVector.x, tempVector.y);
                }
                StoreVectorArray.vector4Values = vector4s;
            }

            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
