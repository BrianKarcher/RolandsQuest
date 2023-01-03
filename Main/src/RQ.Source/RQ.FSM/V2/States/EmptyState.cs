using UnityEngine;
//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;

//using RQ.Entity.UI;


namespace RQ.Entity.StatesV2
{
    /// <summary>
    /// A state that by itself does not do anything.  An example for usage is to connect to a 
    /// child state machine
    /// </summary>
    [AddComponentMenu("RQ/States/State/Empty")]
    public class EmptyState : StateBase//<ISprite>
    {

    }
}
