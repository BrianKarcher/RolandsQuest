using RQ.Common.Controllers;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Model.GameData.StoryProgress;
using UnityEngine;

namespace RQ.FSM.V3.Conditionals
{
    //[AddComponentMenu("RQ/States/Conditions/Game Progress")]
    public class StorySceneConditionConfig : StateTransitionConditionBaseConfig
    {
        [SerializeField]
        private Operator Operator = Operator.Equal;

        [SerializeField]
        private StorySceneConfig StoryScene = null;

        public override bool TestCondition(IStateMachine stateMachine)
        {
            var storyProgress = GameDataController.Instance.Data.GetStoryProgress();

            switch (Operator)
            {
                case Operator.Equal:
                    if (storyProgress == StoryScene)
                        return true;
                    break;
                case Operator.NotEqual:
                    if (storyProgress != StoryScene)
                        return true;
                    break;
                case Operator.GreaterThen:
                    if (storyProgress > StoryScene)
                        return true;
                    break;
                case Operator.GreaterThenOrEqualTo:
                    if (storyProgress >= StoryScene)
                        return true;
                    break;
                case Operator.LessThen:
                    if (storyProgress < StoryScene)
                        return true;
                    break;
                case Operator.LessThanOrEqualTo:
                    if (storyProgress <= StoryScene)
                        return true;
                    break;
            }
            return false;
        }

        //public void LateUpdate()
        //{
        //    //_isButtonPressed = false;
        //}

        //public override bool HandleMessage(Telegram telegram)
        //{
        //    if (base.HandleMessage(telegram))
        //        return true;

        //    //var data = agent as ISprite;
            
        //    if (telegram.Msg == _telegram)
        //    {
        //        Log.Info("Telegram " + telegram.Msg + " received in MessageReceivedCondition");
        //        SetIsConditionSatisfied(true);
        //    }
        //    return false;
        //}

        //public override void Reset()
        //{
        //    _isTelegramReceived = false;
        //}

        //public int MessageHandlerID()
        //{
        //    return -2;
        //}
    }
}
