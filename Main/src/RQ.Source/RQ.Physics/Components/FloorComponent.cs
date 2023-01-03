using RQ.Common.Components;
using RQ.Common.Controllers;
using RQ.Enum;
using RQ.Enums;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Floor Component")]
    public class FloorComponent : ComponentPersistence<FloorComponent>
    {
        public int Floor = 1;
        private PhysicsComponent _physicsComponent;

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;

            //if (!GameDataController.Instance.LoadingGame)
            SetFloor(Floor);
        }

        private PhysicsComponent PhysicsComponent
        {
            get
            {
                if (_physicsComponent == null)
                    _physicsComponent = base._componentRepository.Components.GetComponent<PhysicsComponent>();
                return _physicsComponent;
            }
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;
            switch (msg.Msg)
            {
                case Telegrams.SetLevelHeight:
                    var levelLayer = (LevelLayer)msg.ExtraInfo;
                    SetFloor((int)levelLayer);
                    break;
            }
            return false;
        }

        public virtual LevelLayer GetLevel()
        {
            return (LevelLayer)Floor;
        }

        public void SetFloor(int floor)
        {
            Floor = floor;
            //base.SetLevel(levelLayer);
            //_collisionData._levelLayer = levelLayer;

            ////string layerName;
            //SingleUnityLayer layer;

            ////float newZ = 0;
            ////var entityUI = GetEntityUI();
            ////var adjustDepthByY = GetComponent<AdjustDepthByY>();
            //if (GameDataController.Instance.Data == null)
            //    return;
            //var currentScene = GameDataController.Instance.Data.CurrentScene;

            //if (currentScene == null)
            //    throw new Exception("No current scene, cannot set floor.");

            //float newZIndex = 0f;
            //int level = 0;

            //switch (levelLayer)
            //{
            //    case LevelLayer.LevelOne:
            //        //layerName = "Level 1 NC";
            //        layer = _levelOneLayer;
            //        level = 1;
            //        //newZIndex = currentScene.LevelOneZIndex;
            //        //newZ = _levelOneZIndex;
            //        break;
            //    case LevelLayer.LevelTwo:
            //        level = 2;
            //        //newZIndex = currentScene.LevelTwoZIndex;
            //        layer = _levelTwoLayer;
            //        //layerName = "Level 2 NC";
            //        //newZ = _levelTwoZIndex;
            //        break;
            //    case LevelLayer.LevelThree:
            //        level = 3;
            //        //newZIndex = currentScene.LevelThreeZIndex;
            //        layer = _levelThreeLayer;
            //        break;
            //    case LevelLayer.Shared:
            //        level = 2;
            //        // TODO Improve this by separating the physical level to the collision level
            //        //newZIndex = currentScene.LevelTwoZIndex;
            //        layer = _sharedLayer;
            //        break;
            //    default:
            //        layer = _levelOneLayer;
            //        //layerName = string.Empty;
            //        break;
            //}

            // So, level 1 = 0
            // leve 2 = -1
            // etc
            var newZIndex = -(floor - 1);

            if (PhysicsComponent != null)
            {
                PhysicsComponent.SetZOffsetByLevel(newZIndex);
            }
            else
            {
                var pos = _componentRepository.transform.position;
                _componentRepository.transform.position = new Vector3(pos.x, pos.y, newZIndex);
            }

            //SetZIndex(newZ);
            //if (adjustDepthByY != null)
            //    adjustDepthByY.SetOriginalZ(newZ);

            //Rigidbody2D.
            //var mainCollider = _mainCollider;
            //if (_autoChangeLayers && mainCollider != null && mainCollider.gameObject != null)
            //    mainCollider.gameObject.layer = layer.LayerIndex;
            //mainCollider.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}
