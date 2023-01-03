using RQ.Common.Controllers;
using RQ.Controller.ManageScene;
using RQ.Model.Serialization;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Change Scenes")]
    public class ChangeScenes : ActionBase
    {
        public SceneConfig sceneConfig;
        //[HideInInspector]
        //public int SpawnPointInstanceId;
        [HideInInspector]
        public string spawnPointUniqueId;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            GameStateController.Instance.ChangeScene(sceneConfig, spawnPointUniqueId);
        }

        //public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    if (sceneConfig == null)
        //    {
        //        Debug.LogError("Scene config is null in " + name + "(" + transform.parent.name + ")");
        //        return;
        //    }
        //    var changeScenesData = new ChangeScenesData()
        //    {
        //        SceneConfigUniqueId = sceneConfig.UniqueId,
        //        SpawnPointUniqueId = spawnPointUniqueId
        //    };
        //    base.SerializeComponent(entitySerializedData, changeScenesData);
        //}

        //public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    var changeScenesData = base.DeserializeComponent<ChangeScenesData>(entitySerializedData);
        //    if (changeScenesData == null)
        //        return;
        //    spawnPointUniqueId = changeScenesData.SpawnPointUniqueId;
        //    sceneConfig = GameDataController.Instance.GetSceneConfig(changeScenesData.SceneConfigUniqueId);
        //}
    }
}
