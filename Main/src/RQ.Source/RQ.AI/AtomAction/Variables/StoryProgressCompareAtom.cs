using RQ.AI;
using RQ.Common.Controllers;
using RQ.Controller.Actions.Conditionals;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Model.GameData.StoryProgress;
using System;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class StoryProgressCompareAtom : AtomActionBase
    {
        public OperatorEnum Operator;
        public StorySceneConfig StoryScene;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Success : AtomActionResults.Failure;
        }

        private void Tick()
        {
            if (StoryScene == null)
                return;
            var storyProgress = GameDataController.Instance.Data.GetStoryProgress();

            switch (Operator)
            {
                case OperatorEnum.Equal:
                    if (storyProgress == StoryScene)
                        _isRunning = false;
                    break;
                case OperatorEnum.NotEqual:
                    if (storyProgress != StoryScene)
                        _isRunning = false;
                    break;
                case OperatorEnum.GreaterThen:
                    if (storyProgress > StoryScene)
                        _isRunning = false;
                    break;
                case OperatorEnum.GreaterThenOrEqualTo:
                    if (storyProgress >= StoryScene)
                        _isRunning = false;
                    break;
                case OperatorEnum.LessThen:
                    if (storyProgress < StoryScene)
                        _isRunning = false;
                    break;
                case OperatorEnum.LessThanOrEqualTo:
                    if (storyProgress <= StoryScene)
                        _isRunning = false;
                    break;
            }
        }
    }
}
