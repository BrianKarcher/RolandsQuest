using UnityEngine;

namespace RQ.Render.Graphics
{
    [AddComponentMenu("RQ/Graphics/Sprite Animator")]
    public class RQSpriteAnimatorSingleTexture : MonoBehaviour
    {
        [SerializeField]
        private string _textureName = "_MainTex";
        [SerializeField]
        private Texture _texture;
        [SerializeField]
        private float _frameRate;
        [SerializeField]
        private AnimationWrapMode _animationWrapMode = AnimationWrapMode.Loop;
        private float _frameDelay = 0f;
        // TODO Make not serializable when done testing
        [SerializeField]
        private int _currentFrame = 0;
        [SerializeField]
        private int _width;
        [SerializeField]
        private int _height;
        private float _nextFrameFlip = 0f;
        [SerializeField]
        private Material _material;
        //private Renderer _renderer;
        private bool _goingForwards = true;
        private int _frameCount;
        //private tk2dSprite _sprite;

        public void Start()
        {
            //_sprite = GetComponent<tk2dSprite>();
            //_renderer = GetComponent<Renderer>();
            _frameDelay = 1f / _frameRate;
            _frameCount = _material.GetTexture(_textureName).width / _width;
            SetNextFrameFlip();
            // This one is trippy!
            //Vector2[] uvs = new Vector2[]
            //{
            //    new Vector2(0, 0),
            //    new Vector2(0, 16f / height),
            //    new Vector2(16f / width, 16f / height),
            //    new Vector2(16f / width, 0)
            //};
            //Vector2[] uvs = new Vector2[]
            //{
            //    new Vector2(0, 0),
            //    new Vector2(16f / width, 16f / height),
            //    new Vector2(16f / width, 0),                
            //    new Vector2(0, 16f / height)
            //};
            //_mesh.uv[0] = new Vector2();
            //tk2dSprite sp;
            //sp.CurrentSprite.materialInst.
            var width = (float)_material.mainTexture.width;
            var height = (float)_material.mainTexture.height;
            // This one is trippy!
            //Vector2[] uvs = new Vector2[]
            //{
            //    new Vector2(0, 0),
            //    new Vector2(0, 16f / height),
            //    new Vector2(16f / width, 16f / height),
            //    new Vector2(16f / width, 0)
            //};
            //Vector2[] uvs = new Vector2[]
            //{
            //    new Vector2(0, 0),
            //    new Vector2(16f / width, 16f / height),
            //    new Vector2(16f / width, 0),                
            //    new Vector2(0, 16f / height)
            //};

            //Vector2[] uvs = new Vector2[]
            //{
            //    new Vector2(_uvOffset.x / width, _uvOffset.y / height),
            //    new Vector2((16f + _uvOffset.x) / width, (16f + _uvOffset.y) / height),
            //    new Vector2((16f + _uvOffset.x) / width, _uvOffset.y / height),                
            //    new Vector2(_uvOffset.x / width, (16f + _uvOffset.y) / height)
            //};
            //_material.SetVector
            //_mesh.uv = uvs;
        }

        private void SetNextFrameFlip()
        {
            _nextFrameFlip = Time.time + _frameDelay;
        }

        public void Update()
        {
            //tk2dSprite sprite;
            //sprite.GetCurrentSpriteDef().uvs;
            //tk2dClippedSprite sprite;
            if (Time.time > _nextFrameFlip)
            {
                //_currentFrame++;
                // Hit the end?
                if (_currentFrame >= _frameCount - 1 && _goingForwards)
                {
                    if (_animationWrapMode == AnimationWrapMode.Loop)
                        _currentFrame = 0;
                    else if (_animationWrapMode == AnimationWrapMode.PingPong)
                    {
                        _goingForwards = false;
                        _currentFrame--;
                    }
                }
                // Hit the beginning?
                else if (_currentFrame <= 0 && !_goingForwards)
                {
                    if (_animationWrapMode == AnimationWrapMode.Loop)
                        _currentFrame = _frameCount - 1;
                    else if (_animationWrapMode == AnimationWrapMode.PingPong)
                    {
                        _goingForwards = true;
                        _currentFrame++;
                    }
                }
                // Continue as normal
                else
                {
                    _currentFrame = _goingForwards ? _currentFrame + 1 : _currentFrame - 1;
                }
                //_material.SetTexture(_textureName, _textures[_currentFrame]);
                //_material.SetTextureOffset("", offset);

                SetNextFrameFlip();
                //_sprite.CurrentSprite.materialInst.SetTextureOffset(_textureName, _uvOffset);
            }
        }
    }
}
