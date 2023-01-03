using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(TimerConditionConfig), true)]
    public class TimerConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Timer Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<TimerConditionConfig>("NewTimerConditionConfig.asset");
        }
    }
}
