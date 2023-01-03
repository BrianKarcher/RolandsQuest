using UnityEngine;
using RQ.Model;
using RQ.Messaging;
using System;
using RQ.Physics;
using RQ.Common.Components;
using RQ.Common.Controllers;
using RQ.Model.Interfaces;

namespace RQ
{
    //public enum CameraMode
    //{
    //    FollowTarget = 0,
    //    Manual = 1
    //}
    public class CameraClass : ComponentPersistence<CameraClass>, ICameraClass
    {
        [SerializeField]
        private Vector2 _viewportMargin;
        //private Vector2 cameraSize;
        public Vector2 minCamera; // = new Vector3(0,0,-10);
        public Vector2 maxCamera;
        //private Vector3 pos;
        public Vector2 _screenSize;
        //public CameraMode CameraMode;
        public bool _lockX;
        public bool _lockY;
        public Vector2D _lockPos;
        private Camera _camera;
        private IBasicPhysicsComponent _physicsComponent;
        public float ShakeAmount { get; set; }
        public float ShakeInterval { get; set; }
        private bool _isClamping;
        private Vector2 _offset;

        private long _cameraUpdateId, _isPosInViewportId;
        private Action<Telegram2> _cameraUpdateDelegate, _isPosInViewportDelegate;

        //public Transform Target;
        /// <summary>
        /// UniqueId persists forever
        /// </summary>
        //[SerializeField]
        //[UniqueIdentifier] // Treat this special in the editor.
        //public string _uniqueId; // A String representing our Guid
        //public virtual string UniqueId { get { return _uniqueId; } set { _uniqueId = value; } }

        //private float VerticalHeightScreen;
        //private float HorizontalHeightScreen;
        //private tk2dTileMap tileMap;
        // haha

        //private tk2dCamera camera;

        public override void Awake()
        {
            base.Awake();
            _lockPos = new Vector2D();
            _isClamping = true;
            _cameraUpdateDelegate = (data) =>
            {
                LateUpdate();
            };
            _isPosInViewportDelegate = (data) =>
            {
                var isInViewport = IsPosInViewport((Vector2D) data.ExtraInfo);
                if (isInViewport)
                {
                    MessageDispatcher2.Instance.DispatchMsg("PosInViewport", 0f, this.UniqueId, data.SenderId, null);
                }
            };
        }

        // Use this for initialization
        public override void Start()
        {
            base.Start();

            _physicsComponent = _componentRepository.Components.GetComponent<IBasicPhysicsComponent>();

            MessageDispatcher2.Instance.DispatchMsg("SetCamera", 0f, this.UniqueId, "Game Controller", this);
        }

        public override void Init()
        {
            base.Init();
            GameDataController.Instance.Camera = this;
            _camera = GetComponent<Camera>();
            _lockX = false;
            _lockY = false;
            float verticalHeightScreen;
            float horizontalHeightScreen;
            verticalHeightScreen = _camera.orthographicSize * 2.0f;
            //_camera.pixelWidth 
            //_camera.aspect = (float)Screen.width / Screen.height;
            //_camera.aspect = 16f/9f;
            horizontalHeightScreen = verticalHeightScreen * ((float)Screen.width / Screen.height);
            minCamera = new Vector2(horizontalHeightScreen / 2f, verticalHeightScreen / 2f);

            _screenSize = new Vector2(horizontalHeightScreen, verticalHeightScreen);
            
        }

        public override void StartListening()
        {
            base.StartListening();
            _cameraUpdateId = MessageDispatcher2.Instance.StartListening("CameraUpdate", _componentRepository.UniqueId, _cameraUpdateDelegate);
            //_componentRepository.StartListening("CameraUpdate", this.UniqueId, );
            _isPosInViewportId = MessageDispatcher2.Instance.StartListening("IsPosInViewport", _componentRepository.UniqueId, _isPosInViewportDelegate);
            //_componentRepository.StartListening("IsPosInViewport", this.UniqueId, );
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("CameraUpdate", _componentRepository.UniqueId, _cameraUpdateId);
            //_componentRepository.StopListening("CameraUpdate", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("IsPosInViewport", _componentRepository.UniqueId, _isPosInViewportId);
            //_componentRepository.StopListening("IsPosInViewport", this.UniqueId);
        }

        public bool IsPosInViewport(Vector2D pos)
        {
            var viewport = GetViewport();
            viewport.size = viewport.size + _viewportMargin;
            //var viewportMargin = new Rect()
            return viewport.Contains(pos);

            //if (pos.x > viewport.xMin && pos.x < viewport.xMax && pos.y > viewport.yMin && pos.y < viewport.yMax)
            //{

            //}
        }

        public override void Destroy()
        {
            base.Destroy();
            MessageDispatcher2.Instance.DispatchMsg("SetCamera", 0f, this.UniqueId, "Game Controller", null);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            GameDataController.Instance.Camera = null;
            MessageDispatcher2.Instance.DispatchMsg("SetCamera", 0f, this.UniqueId, "Game Controller", null);
        }

        public Vector2 GetScreenSize()
        {
            return _screenSize;
        }

        /// <summary>
        /// Get the viewport of the camera in world units
        /// </summary>
        /// <returns></returns>
        public Rect GetViewport()
        {
            float halfScreenX = _screenSize.x / 2;
            float halfScreenY = _screenSize.y / 2;
            return new Rect(transform.position.x - halfScreenX, transform.position.y - halfScreenY, _screenSize.x, _screenSize.y);
        }

        //public void SetBounds(Rect bounds)
        //{
        //
        //}

        //public void SetMinBounds(Vector2 minBounds)
        //{
        //
        //}

        public void SetMaxBounds(int tileMapWidth, int tileMapHeight)
        {
            Vector2 screenSize = GetScreenSize();
            // Tile map width and height are in number of tiles.  So we multiply it by 16 to get the pixel size
            // Then divide it by 100 to convert it to units/pixels ratio
            Vector2 tileSizeUnits = new Vector2(tileMapWidth * .16f, tileMapHeight * .16f);
            // The reasoning behind subtracting by 0.08f is because the anchor point of the tile sheet is
            // center. Thus any particular point on the tile sheet is the center of the tile.
            // So the tile at (0,0) actually comprises of (-.08f, -.08f) to (.08f, .08f).
            // Due to the half-tile shift into the negative quadrant, we offset the camera bounds to accomodate for that.
            Vector2 maxBounds = tileSizeUnits - (screenSize / 2f) - new Vector2(0.08f, 0.08f);
            //Vector2 maxBounds = new Vector2(_tileMap.width - (screenSize.x / 2), _tileMap.height - (screenSize.y / 2));
            SetMaxBounds(maxBounds);

            if (tileSizeUnits.x < screenSize.x)
            {
                _lockX = true;
                _lockPos.x = maxBounds.x + ((screenSize.x - tileSizeUnits.x) / 2f);
            }
            else
            {
                _lockX = false;                
            }

            if (tileSizeUnits.y < screenSize.y)
            {
                _lockY = true;
                _lockPos.y = maxBounds.y + ((screenSize.y - tileSizeUnits.y) / 2f);
            }
            else
            {
                _lockY = false;
            }

            // If needed, center the tile map on the screen in either the X or Y positions
            //pos = new Vector3(_lockX ? tileSizeUnits.x / 2f + (screenSize.x - tileSizeUnits.x) / 2f : pos.x,
            //                  _lockY ? tileSizeUnits.y / 2f + (screenSize.y - tileSizeUnits.y) / 2f : pos.y, 
            //                  pos.z);
            //pos = new Vector3(_lockX ? tileSizeUnits.x / 2f - ((screenSize.x - tileSizeUnits.x) / 2f) : pos.x,
            //      _lockY ? tileSizeUnits.y / 2f - ((screenSize.y - tileSizeUnits.y) / 2f) : pos.y,
            //      pos.z);

            //pos = new Vector3(_lockX ? (screenSize.x - tileSizeUnits.x) / 2f: pos.x,
            //      _lockY ? (screenSize.y - tileSizeUnits.y) / 2f: pos.y,
            //      pos.z);

            //pos = new Vector3(_lockX ? maxBounds.x + ((screenSize.x - tileSizeUnits.x) / 2f) : pos.x,
            //     _lockY ? maxBounds.y + ((screenSize.y - tileSizeUnits.y) / 2f) : pos.y,
            //     pos.z);

            // TODO Refactor this
            transform.position = new Vector3(_lockX ? _lockPos.x : transform.position.x,
                 _lockY ? _lockPos.y : transform.position.y,
                 transform.position.z);
        }

        private void SetMaxBounds(Vector2 maxBounds)
        {
            maxCamera = maxBounds;
        }

        public Vector2 GetMaxBounds()
        {
            return maxCamera;
        }

        public Vector2D ClampPos(Vector2D pos)
        {
            //Vector3 newpos = Vector3.zero;
            if (maxCamera != Vector2.zero)
            {
                var newX = _lockX ? _lockPos.x : Mathf.Clamp(pos.x, minCamera.x, maxCamera.x);
                var newY = _lockY ? _lockPos.y : Mathf.Clamp(pos.y, minCamera.y, maxCamera.y);
                return new Vector2D(newX, newY);
                //return new Vector3(newpos.x, newpos.y, pos.z);
            }

            return pos;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //if (CameraMode == RQ.CameraMode.FollowTarget)
            //{
            //    if (Target == null)
            //    {
            //        if (EntityContainer._instance.GetMainCharacter() != null)
            //            Target = EntityContainer._instance.GetMainCharacter().transform;
            //    }

            //    if (Target != null)
            //    {
            //        transform.position = ClampPos(Target.transform.position).ToVector3(transform.position.z);
            //    }
            //}
            if (_isClamping)
                transform.position = ClampPos(transform.position).ToVector3(transform.position.z);
            //else
            //    transform.position = 

            //transform.position = pos;
        }

        public Camera GetCamera()
        {
            return _camera;
        }

        public void SetClamping(bool isClamping)
        {
            _isClamping = isClamping;
        }

        public void SetOffset(Vector2 offset)
        {
            _offset = offset;
        }

        //IEnumerator Shake()
        //{
        //    while (true)
        //    {
        //        var pos = _physicsComponent.GetWorldPos();
        //        //float x = UnityEngine.Random.Range(-_shakeAmt, _shakeAmt);
        //        //float y = UnityEngine.Random.Range(-_shakeAmt, _shakeAmt);
        //        float x = UnityEngine.Random.value * ShakeAmount * 2 - ShakeAmount;
        //        float y = UnityEngine.Random.value * ShakeAmount * 2 - ShakeAmount;
        //        //transform.position += new Vector3(x, y, 0);
        //        //physicsComponent.SetWorldPos(pos + new Vector2D(x, y));
        //        //_physicsComponent.GetPhysicsData().ExternalVelocity = new Vector2D(x, y) * 50f;
        //        _physicsComponent.GetPhysicsData().Offset = new Vector2D(x, y);
        //        var targetLocation = (Vector2D)_aiComponent.Target.transform.position + _physicsComponent.GetPhysicsData().Offset;

        //        MessageDispatcher2.Instance.DispatchMsg("SetPos", 0f, _entity.UniqueId, _physicsComponent.UniqueId,
        //            targetLocation);
        //        MessageDispatcher2.Instance.DispatchMsg("CameraUpdate", 0f, _entity.UniqueId, _entity.UniqueId, null);
        //        yield return new WaitForSeconds(ShakeInterval);
        //    }
        //}
    }
}