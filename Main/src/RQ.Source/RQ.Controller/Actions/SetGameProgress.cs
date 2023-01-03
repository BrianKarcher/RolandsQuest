using RQ.Common.Controllers;
using RQ.Model.GameData.StoryProgress;
using UnityEngine;

namespace RQ.Controller.Actions.Conditionals
{
    [AddComponentMenu("RQ/Action/Set Game Progress")]
    public class SetGameProgress : ActionBase
    {
        [SerializeField]
        private StorySceneConfig _storySceneConfig;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            if (_storySceneConfig == null)
                return;
            GameDataController.Instance.Data.SetStoryProgress(_storySceneConfig);
        }
    }
}
