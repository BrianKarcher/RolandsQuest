using RQ.Animation.BasicAction.Action;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Instantiate Prefab2")]
    public class InstantiatePrefab2 : ActionBase
    {
        public InstantiatePrefabAtom _instantiatePrefabAtom;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            _instantiatePrefabAtom.Start(GetEntity());
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
        }
    }
}
