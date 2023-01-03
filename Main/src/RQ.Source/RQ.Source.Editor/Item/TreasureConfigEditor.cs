using UnityEngine;
using UnityEditor;
using RQ.Controller.ManageScene;
using RQ.Entity.Assets;

namespace RQ.Editor
{
    [CustomEditor(typeof(TreasureConfig), true)]
    public class TreasureConfigEditor : BaseObjectEditor<TreasureConfig>
    {
        //TreasureComponent _spriteBaseComponent;
        //protected tk2dTileMap tileMap;
        //public bool isGetCenterPointPressed = false;

        //public override void OnEnable()
        //{
        //    base.OnEnable();
        //    _spriteBaseComponent = target as SpriteBaseComponent;
        //    //tileMap = GameObject.FindObjectOfType<tk2dTileMap>();
        //}

        [MenuItem("Assets/Create/RQ/Treasure Config")]
        public static void CreateNewAsset()
        {
            var treasureConfig = EditorBase.CreateAsset<TreasureConfig>("NewTreasureConfig.asset");
            // Get the next Id
            var gameConfig = Resources.FindObjectsOfTypeAll<GameConfig>()[0];
            //var newId = gameConfig.TreasureChestCount;
            //treasureConfig.SetTreasureId(newId);
            // Set the new count
            //gameConfig.TreasureChestCount++;
            //Dirty();
            EditorUtility.SetDirty(treasureConfig);
            EditorUtility.SetDirty(gameConfig);
        }

        //public override void OnInspectorGUI()
        //{
        //    base.OnInspectorGUI();

        //    if (agent.GetTreasureId() == -1)
        //    {

        //    }
        //}
    }
}
