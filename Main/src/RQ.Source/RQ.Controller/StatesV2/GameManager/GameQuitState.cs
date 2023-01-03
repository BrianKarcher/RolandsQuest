using UnityEngine;
//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Game Quit")]
    public class GameQuitState : StateBase
    {
        public override void Enter()
        {
            Debug.Log("Quit pressed");
            Application.Quit();
        }
    }
}
