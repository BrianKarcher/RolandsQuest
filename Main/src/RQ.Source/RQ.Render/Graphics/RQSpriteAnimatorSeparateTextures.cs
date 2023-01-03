using System.Collections.Generic;
using UnityEngine;

namespace RQ.Render.Graphics
{
    [AddComponentMenu("RQ/Graphics/Sprite Animator")]
    public class RQSpriteAnimator : MonoBehaviour
    {
        [SerializeField]
        private string _textureName = "_MainTex";
        [SerializeField]
        private List<Texture> _textures;
        [SerializeField]
        private float _frameRate;
        [SerializeField]
        private AnimationWrapMode _animationWrapMode = AnimationWrapMode.Loop;
        private float _frameDelay = 0f;
        // TODO Make not serializable when done testing
        [SerializeField]
        private int _currentFrame = 0;
        private float _nextFrameFlip = 0f;
        private Material _material;
        private Renderer _renderer;
        private bool _goingForwards = true;
        //private tk2dSprite _sprite;

        public void Start()
        {
            //_sprite = GetComponent<tk2dSprite>();
            _renderer = GetComponent<Renderer>();
            _material = _renderer != null ? _renderer.material : _material;
            _frameDelay = 1f / _frameRate;
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
                if (_currentFrame >= _textures.Count - 1 && _goingForwards)
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
                        _currentFrame = _textures.Count - 1;
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
                _material.SetTexture(_textureName, _textures[_currentFrame]);

                SetNextFrameFlip();
                //_sprite.CurrentSprite.materialInst.SetTextureOffset(_textureName, _uvOffset);
            }
        }
    }
}
