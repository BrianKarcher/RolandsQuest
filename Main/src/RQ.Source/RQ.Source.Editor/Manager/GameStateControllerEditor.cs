using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(GameStateController), true)]
    public class GameStateControllerEditor : BaseObjectEditor<GameStateController>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //agent.EntityPrefabs

        }
    }
}
