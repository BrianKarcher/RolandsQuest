using UnityEngine;
//using Sprites = RQ.Entity.Sprites;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/UI/Play")]
    public class PlayState : PanelState //<ISprite>
    {
        public override void Enter()
        {
            base.Enter();
            if (_entity == null)
                return;
        }
    }
}
