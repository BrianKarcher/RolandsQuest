using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(AnimationCompleteConditionConfig), true)]
    public class AnimationCompleteConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Animation Complete Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<AnimationCompleteConditionConfig>("NewAnimationCompleteConditionConfig.asset");
        }
    }
}
