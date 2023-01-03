using RQ.FSM.V2.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(MessageReceivedConditionConfig3), true)]
    public class MessageReceivedConditionConfig3Editor : EditorBase
    {
        MessageReceivedConditionConfig3 agent;

        [MenuItem("Assets/Create/RQ/Conditional/Message Received Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<MessageReceivedConditionConfig3>("MessageReceivedConditionConfig.asset");
            //var actConfig = ScriptableObject.CreateInstance<StoryActConfig>();
            //AssetDatabase.CreateAsset(actConfig, "Assets/Story/Acts/NewActConfig.asset");
            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = actConfig;
        }
    }
}
