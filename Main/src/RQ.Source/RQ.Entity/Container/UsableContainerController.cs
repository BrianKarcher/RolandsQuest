using System;
using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Entity.Common;
using RQ.Enums;
using RQ.Messaging;
using RQ.Common;
using RQ.Controller.UI;
using RQ.Model.Containers;
using RQ.Model.ObjectPool;
using UnityEngine;
using RQ.Model.UI;

namespace RQ.Controller.Contianers
{
    [AddComponentMenu("RQ/UI/Usable Controller")]
    public class UsableContainerController : ComponentRepository, IMessagingObject
    {
        [SerializeField] private GameObject _usableToken;
        private GameObject _instantiatedUsableToken = null;
        public GameObject goUIManager;
        public UsableContainer UsableContainer { get; set; }
        private IUIManager UIManager;
        private static UsableContainerController _instance;
        [HideInInspector]
        public static UsableContainerController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<UsableContainerController>();
                return _instance;
            }
        }

        public override string UniqueId
        {
            get { return "Usable Controller"; }
            set { }
        }

        //private IEntity _mainPlayer;

        private Action<object> _createUsableTokenDelegate;

        public override void Awake()
        {
            base.Awake();

            if (!Application.isPlaying)
                return;

            if (goUIManager != null)
                UIManager = goUIManager.GetComponent<IUIManager>();

            UsableContainer = new UsableContainer();
            _createUsableTokenDelegate = CreateToken;
            UsableContainer.UsableChanged += (uniqueId) =>
            {
                MessageDispatcher2.Instance.DispatchMsg("UsableChanged", 0f, null, "Usable Controller", null);
            };
        }

        

        //public override void Start()
        //{
        //    base.Start();
        //    var players = EntityController.Instance.GetByEntityType(EntityType.Player);
        //    _mainPlayer = players.Count == 0 ? null : players[0];
        //    ObjectPool.Instance.ReleaseToPool(ObjectPoolType.IEntityList, players);
        //}

        //public override 

        //public void OnLevelWasLoaded(int level)
        //{            
        //    base.RegisterWithMessageDispatcher();
        //}

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            var mainPlayer = EntityContainer._instance.GetMainCharacter();
            if (mainPlayer != null)
                CalculateAndSetClosestUsable(mainPlayer.UniqueId);
        }

        public void CreateUsableToken()
        {
            //var dataContainer = GameDataController.Instance.Data.UsableContainer;
            if (UsableContainer == null)
                return;
            var usableComponentUniqueId = UsableContainer.CurrentUsableObject;
            if (usableComponentUniqueId == null)
                return;

            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, usableComponentUniqueId,
                Telegrams.GetChild, null, _createUsableTokenDelegate);
        }

        public void CreateToken(object obj)
        {
            DestroyUsableToken();
            Transform usable;
            if (obj == null)
            {
                return;
            }
            usable = (Transform)obj;
            _instantiatedUsableToken = Instantiate(_usableToken, usable.position,
                Quaternion.identity) as GameObject;
            _instantiatedUsableToken.transform.parent = usable;
            Transform textPos = null;
            foreach (Transform child in _instantiatedUsableToken.transform)
            {
                if (child.name == "TextPos")
                {
                    textPos = child;
                    break;
                }
            }
            //var dataContainer = GameDataController.Instance.Data.UsableContainer;
            //UIManager.AddFollowingLabel(UsableContainer.CurrentBubbleText, textPos);
            MessageDispatcher2.Instance.DispatchMsg("AddFollowingLabel", 0f, this.UniqueId, "UI Manager", new FollowingLabelData()
            {
                Text = UsableContainer.CurrentBubbleText,
                Target = textPos
            });
        }

        private void DestroyUsableToken()
        {
            if (_instantiatedUsableToken != null)
            {
                GameObject.Destroy(_instantiatedUsableToken);
                _instantiatedUsableToken = null;
            }
        }

        private void CalculateAndSetClosestUsable(string targetUniqueId)
        {
            // If the game hasn't started.
            if (GameDataController.Instance.Data == null)
                return;
            //var dataContainer = GameDataController.Instance.Data.UsableContainer;
            var usable = GetUsable(targetUniqueId);

            var currentUsable = UsableContainer.GetCurrentUsable();
            if (currentUsable != usable)
            {
                // Send a message to the previous usable object that it is
                // no longer the usable object
                UsableContainer.SetCurrentUsable(usable);
                CreateUsableToken();
                // Send a message to the new usable object that it has
                // been activated as the usable object
            }
        }

        private string GetUsable(string targetUniqueId)
        {
            //var dataContainer = GameDataController.Instance.Data.UsableContainer;
            var usableList = UsableContainer.GetList();
            //var usableObjects = .Select(i => i.Key);
            if (usableList.Count == 0)
            {
                UsableContainer.SetCurrentUsable(string.Empty);
                //CreateUsableToken();
                DestroyUsableToken();
                return string.Empty;
            }
            string closestObject = string.Empty;
            var spriteBase = EntityContainer._instance.GetEntity(targetUniqueId) as SpriteBaseComponent;
            if (spriteBase == null)
            {
                Debug.LogError($"(UsableContainerController) Sprite {targetUniqueId} could not be found.");
                UsableContainer.SetCurrentUsable(string.Empty);
                CreateUsableToken();
                return string.Empty;
            }
            return spriteBase.GetClosestObject(usableList);
            //MessageDispatcher.Instance.DispatchMsg(0f, string.Empty, targetUniqueId,
            //    RQ.Enums.Telegrams.GetClosestObjectFromSuppliedList, usableObjects.AsEnumerable(),
            //    (closest) => closestObject = closest as string);

            //return closestObject;
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Telegrams.AddContainerItem:
                    CreateUsableToken();
                    break;
                case Telegrams.DestroyContainerItem:
                    DestroyUsableToken();
                    break;
            }
            return false;
        }        
    }
}
