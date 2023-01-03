using UnityEngine;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(SceneSetup), true)]
    public class SceneSetupEditor : EditorBase
    {
        SceneSetup agent;
        //protected tk2dTileMap tileMap;

        public void OnEnable()
        {
            agent = target as SceneSetup;
            //tileMap = agent.TileMap;
        }

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            base.OnInspectorGUI();

            //if (GUILayout.Button("Import TMX"))
            //{
            //    if (tk2dEditor.TileMap.Importer.Import(tileMap, tk2dEditor.TileMap.Importer.Format.TMX))
            //    {
            //        tileMap.ForceBuild();
            //        //Build(true);
            //        //width = tileMap.width;
            //        //height = tileMap.height;
            //        //partitionSizeX = tileMap.partitionSizeX;
            //        //partitionSizeY = tileMap.partitionSizeY;
            //    }
            //}

            if (GUI.changed)
            {
                Dirty(false);
            }
        }
    }
}
