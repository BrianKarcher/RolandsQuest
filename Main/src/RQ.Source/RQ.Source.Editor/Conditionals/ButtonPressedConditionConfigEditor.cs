using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(ButtonPressedConditionConfig), true)]
    public class ButtonPressedConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Button Pressed Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<ButtonPressedConditionConfig>("NewButtonPressedConditionConfig.asset");
        }
    }
}
