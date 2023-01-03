using UnityEngine;
using UnityEditor;
using RQ.FSM.V2;

namespace RQ.Editor
{
    [CustomEditor(typeof(AIComponent), true)]
    public class AIComponentEditor : BaseObjectEditor<AIComponent>
    {
        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            base.OnInspectorGUI();
            if (GUILayout.Button("Set current as Home Position"))
            {
                agent.SetHomePosition(agent.transform.position);
                Dirty();
            }          

            if (GUI.changed)
            {
                Dirty();
            }
        }
    }
}
