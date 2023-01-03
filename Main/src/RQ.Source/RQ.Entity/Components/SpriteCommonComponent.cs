using System.Collections;
using RQ.Common.Components;
using RQ.Messaging;
using RQ.Physics.Components;
using RQ.Serialization;
using UnityEngine;

namespace RQ.Entity.Components
{
    [AddComponentMenu("RQ/Components/Sprite Common")]
    public class SpriteCommonComponent : ComponentPersistence<SpriteCommonComponent>
    {
        [SerializeField]
        private LayerMask _obstacleMask;

        //private int _obstacleMask;

        /// <summary>
        /// Last good position is considered the last tile stood on considered walkable. Sprites will be taken back here when, for exampple,
        /// they fall in water.
        /// </summary>
        [SerializeField]
        private Vector3 _lastGoodPosition;

        private PhysicsComponent _physicsComponent;

        private Coroutine _aiCoroutine;

        private long _sendToLastGoodPositionId, _trackSafeTilesId, _stopTrackingSafeTilesId;
        private bool _trackSafeTiles = true;
        //private int[] _lastGoodTile = new int[2];

        //private Animator _animator;

        public override void Awake()
        {
            base.Awake();
            //_obstacleMask = 0;
            //if (_obstacleMasks != null)
            //{
            //    for (int i = 0; i < _obstacleMasks.Length; i++)
            //    {
            //        _obstacleMask |= _obstacleMasks[i];
            //    }
            //}
        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            _physicsComponent = _componentRepository.Components.GetComponent<PhysicsComponent>();
            if (_physicsComponent != null)
                _lastGoodPosition = _physicsComponent.GetFeetWorldPosition3();
            
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (!Application.isPlaying)
                return;
            _aiCoroutine = StartCoroutine(AI());
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (!Application.isPlaying)
                return;
            if (_aiCoroutine != null)
            {
                StopCoroutine(_aiCoroutine);
                _aiCoroutine = null;
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            if (!Application.isPlaying)
                return;
            if (_aiCoroutine != null)
            {
                StopCoroutine(_aiCoroutine);
                _aiCoroutine = null;
            }
        }

        //public override void Update()
        //{
        //    base.Update();
        //}

        public IEnumerator AI()
        {
            while (true)
            {
                // run every 1/10th of a second
                yield return new WaitForSeconds(0.1f);
                if (!_trackSafeTiles)
                    continue;
                var footPos = _physicsComponent.GetFeetWorldPosition3();
                // Convert units to tiles

                int tileStandingOnX = (int)Mathf.Floor(footPos.x / 0.16f);
                int tileStandingOnY = (int)Mathf.Floor(footPos.y / 0.16f);
                var newTileStandingOn = new Vector3((float)tileStandingOnX * 0.16f, (float)tileStandingOnY * 0.16f, transform.position.z);
                if (newTileStandingOn != _lastGoodPosition)
                {
                    // Is new position good?
                    if (!IsPositionOverObstacle(newTileStandingOn))
                    {
                        _lastGoodPosition = newTileStandingOn;
                    }
                }
            }
        }

        private bool IsPositionOverObstacle(Vector3 position)
        {
            // Do four raycasts to make sure the entity isn't on an obstacle
            // Top left
            if (ObstacleRaycast(position + new Vector3(-0.05f, 0.05f)))
                return true;
            // Top right
            if (ObstacleRaycast(position + new Vector3(0.05f, 0.05f)))
                return true;
            // Bottom left
            if (ObstacleRaycast(position + new Vector3(-0.05f, -0.05f)))
                return true;
            // Bottom right
            if (ObstacleRaycast(position + new Vector3(0.05f, -0.05f)))
                return true;

            return false;
        }

        private bool ObstacleRaycast(Vector3 pos)
        {
            //var layer = Layer | CancelLayer;

            return UnityEngine.Physics.Raycast(new Ray(pos, Vector3.forward), 0.5f, _obstacleMask);

            //var physicsData = _physicsComponent.GetPhysicsData();
            //bool hit = UnityEngine.Physics.Raycast(new Ray(pos, Vector3.forward), out var raycastHit, UnityEngine.Mathf.Infinity, layer);
            //if (!hit)
            //    return false;
            //if (CancelLayer == 0)
            //    return true;
            //// If the layer hit was the Cancel Layer, then return false
            //if ((raycastHit.transform.gameObject.layer & CancelLayer) == CancelLayer)
            //    return false;
            //return true;
        }

        public override void StartListening()
        {
            base.StartListening();

            _sendToLastGoodPositionId = MessageDispatcher2.Instance.StartListening("SendToLastGoodPosition", _componentRepository.UniqueId, (data) =>
            {
                _physicsComponent.SetFeetWorldPosition(_lastGoodPosition);
            });
            _trackSafeTilesId = MessageDispatcher2.Instance.StartListening("TrackSafeTiles", _componentRepository.UniqueId, (data) =>
            {
                _trackSafeTiles = true;
            });
            _stopTrackingSafeTilesId = MessageDispatcher2.Instance.StartListening("StopTrackingSafeTiles", _componentRepository.UniqueId, (data) =>
            {
                _trackSafeTiles = false;
            });
            //    //_damagedMessageId = MessageDispatcher2.Instance.StartListening("Damaged", _componentRepository.UniqueId, (data) =>
            //    //{

            //    //});
            //    _hpChangedMessageId = MessageDispatcher2.Instance.StartListening("EntityHPChanged", _componentRepository.UniqueId, (data) =>
            //    {
            //        var hp = (float)data.ExtraInfo;
            //        //_animator.SetFloat("HP", hp);
            //        if (hp <= 0)
            //            _animator.SetTrigger("Dead");
            //        else
            //            _animator.SetTrigger("Damaged");
            //    });
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("SendToLastGoodPosition", _componentRepository.UniqueId, _sendToLastGoodPositionId);
            MessageDispatcher2.Instance.StopListening("TrackSafeTiles", _componentRepository.UniqueId, _trackSafeTilesId);
            MessageDispatcher2.Instance.StopListening("StopTrackingSafeTiles", _componentRepository.UniqueId, _stopTrackingSafeTilesId);
            //    //MessageDispatcher2.Instance.StopListening("Damaged", _componentRepository.UniqueId, _damagedMessageId);
            //    MessageDispatcher2.Instance.StopListening("EntityHPChanged", _componentRepository.UniqueId, _hpChangedMessageId);
        }

        public void SetTrackSafeTiles(bool trackSafeTiles)
        {
            _trackSafeTiles = trackSafeTiles;
        }

    }
}
