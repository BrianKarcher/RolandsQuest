using RQ.Model.Item;
using System;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [Obsolete]
    [AddComponentMenu("RQ/Action/Item/Populate Inventory Grid")]
    public class PopulateInventoryGrid : ActionBase
    {
        public ItemClass[] itemClasses;
        public override void Act(Component otherRigidBody)
        {
        }
    }
}
