using RQ.Entity.Assets;
using UnityEditor;

namespace RQ.Editor.Stats
{
    [CustomEditor(typeof(PlayerLevelStatsConfig), true)]
    public class PlayerLevelStatsEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Stats/Player Level Stats Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<PlayerLevelStatsConfig>("NewPlayerLevelStatsConfig.asset");
        }
    }
}