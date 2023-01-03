using RQ.FSM.V2;
using System;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Load Game Entities")]
    [Obsolete]
    public class LoadGameEntitiesState : StateBase
    {
        public override void Enter()
        {
            Complete();
        }
    }
}
