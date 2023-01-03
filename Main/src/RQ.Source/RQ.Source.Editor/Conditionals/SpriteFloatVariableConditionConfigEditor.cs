using RQ.Entity.StatesV2.Conditions;
using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(SpriteFloatVariableConditionConfig), true)]
    public class SpriteFloatVariableConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Sprite Float Variable Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<SpriteFloatVariableConditionConfig>("NewSpriteFloatVariableConditionConfig.asset");
        }
    }
}
