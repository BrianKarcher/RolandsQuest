using UnityEngine;
using RQ.Enum;
using RQ.Messaging;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Set Level")]
    public class SetLevel : ActionBase
    {
        //public SceneConfig sceneConfig;
        //[HideInInspector]
        //public int SpawnPointInstanceId;
        //[HideInInspector]
        //public string spawnPointUniqueId;
        [SerializeField]
        private LevelLayer _levelLayer = LevelLayer.LevelOne;

        //public override void Awake()
        //{
        //    base.Awake();
        //    // Everything can change levels
        //    //_checkTag = false;
        //}

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            //object1.layer = LayerMask.NameToLayer("My Layer");
            //var gameObject = otherRigidBody.GetComponent<IComponentRepository>();
            var obj = GetEntity();
            if (obj != null)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, obj.UniqueId,
                    Enums.Telegrams.SetLevelHeight, _levelLayer);
                //var entity = EntityUIBase.GetEntity(gameObject);
                //gameObject.SetLevel(_levelLayer);
            }
            //other.attachedRigidbody.gameObject.layer = LayerMask.NameToLayer("Level 2");
            //GameController._instance.ChangeScene(sceneConfig, spawnPointUniqueId);
        }
    }
}
