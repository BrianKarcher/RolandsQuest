using UnityEngine;
//using Sprites = RQ.Entity.Sprites;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/UI/Conversation")]
    public class ConversationState : PanelState //<ISprite>
    {
        public override void Enter()
        {
            base.Enter();
            if (_entity == null)
                return;
            //var sprite = agent as ISprite;
            //Debug.Log("Entering UI Conversation state");
            _entity.SelectContinueButtion();

            // TODO This is game controller logic, should have no place in the UI controller
            // Move it out of here, it is screwing up the game.

            // Pause the main game
            //MessageDispatcher.Instance.DispatchMsg(0f, "Game Controller", "Game Controller", 
            //    Enums.Telegrams.Pause, null);

            // TODO FIX THIS!!!!!!!!!!!!!!
            //data.GetSpriteAnimator().AnimateAttack(data._direction);
            //data.SetVelocity(Vector3.zero);
            //if (!String.IsNullOrEmpty(_animationType))
            //{
            //    _entity.Animate(_animationType);
            //}
            //else
            //{
            //    //_entity.Animate(AnimationType);
            //}
            //_entity.IsAnimationComplete = false;
            //var spriteAnimator = _entity.GetSpriteAnimator() as SpriteAnimator;
            //var animationLength = spriteAnimator.GetCurrentClipLength();
            //MessageDispatcher.Instance.DispatchMsg(animationLength, _entity.ID(), _entity.ID(),
            //    Enums.Telegrams.AnimationComplete, ID);
            //throw new Exception("Hi");
        }

        public override void Exit()
        {
            base.Exit();
            // Play the main game
            //MessageDispatcher.Instance.DispatchMsg(0f, "Game Controller", 
            //    "Game Controller", Enums.Telegrams.Play, null);
            //Debug.Log("Exiting UI Conversation state");
            //Log.Info("Exiting Conversation state");

            //if (GameController.Instance.UIManager.StartSequenceOnConversationEnd != null)
            //{
            //    GameController.Instance.UIManager.StartSequenceOnConversationEnd.Play();
            //    GameController.Instance.UIManager.StartSequenceOnConversationEnd = null;
            //}
        }
    }
}
