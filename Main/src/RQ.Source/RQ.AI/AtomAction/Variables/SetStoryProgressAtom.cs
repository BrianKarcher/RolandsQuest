using RQ.AI;
using RQ.Common.Controllers;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Model.GameData.StoryProgress;
using System;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetStoryProgressAtom : AtomActionBase
    {
        public StorySceneConfig StorySceneConfig;
        public bool Force = false;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            Set();
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        private void Set()
        {
            if (StorySceneConfig == null)
                return;
            GameDataController.Instance.Data.SetStoryProgress(StorySceneConfig, Force);
        }
    }
}
