using RQ.Common.Controllers;
using RQ.Model.GameData.StoryProgress;
using UnityEngine;

namespace RQ.Controller.Actions.Conditionals
{
    [AddComponentMenu("RQ/Action/Conditional/Game Progress")]
    public class StorySceneConditional : ConditionalBase
    {
        public StorySceneConfig StoryScene;

        public override bool Check()
        {
            var storyProgress = GameDataController.Instance.Data.GetStoryProgress();

            switch (Operator)
            {
                case OperatorEnum.Equal:
                    if (storyProgress == StoryScene)
                        return true;
                    break;
                case OperatorEnum.NotEqual:
                    if (storyProgress != StoryScene)
                        return true;
                    break;
                case OperatorEnum.GreaterThen:
                    if (storyProgress > StoryScene)
                        return true;
                    break;
                case OperatorEnum.GreaterThenOrEqualTo:
                    if (storyProgress >= StoryScene)
                        return true;
                    break;
                case OperatorEnum.LessThen:
                    if (storyProgress < StoryScene)
                        return true;
                    break;
                case OperatorEnum.LessThanOrEqualTo:
                    if (storyProgress <= StoryScene)
                        return true;
                    break;
            }
            return false;
        }
    }
}
