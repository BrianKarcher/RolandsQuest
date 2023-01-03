using RQ.Entity.StatesV2;
//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Controller.StatesV2.GameManager
{
    [AddComponentMenu("RQ/States/State/Game Manager/Normal")]
    public class NormalStateGM : MenuState
    {
        public override void Enter()
        {
            base.Enter();
            MessageDispatcher2.Instance.DispatchMsg("Unpause", 0f, string.Empty, _spriteBase.UniqueId, null);
            //MessageDispatcher2.Instance.DispatchMsg("EnableUsable", 0f, this.UniqueId, "Usable Controller", null);
        }
    }
}
