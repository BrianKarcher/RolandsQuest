using UnityEngine;

namespace RQ.Render.Graphics
{
    [AddComponentMenu("RQ/Graphics/Animated UVs")]
    public class AnimatedUVs : MonoBehaviour
    {
        [SerializeField]
        private Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
        [SerializeField]
        private string _textureName = "_MainTex";
        [SerializeField]
        private Vector2 _uvOffset = Vector2.zero;
        private Vector2 _currentUvOffset = Vector2.zero;
        private Renderer _renderer;
        private float _speedAdjustor;
        private float _speedAdjustorDirection;
        private Mesh _mesh;
        //private tk2dSprite _sprite;

        public void Start()
        {
            _speedAdjustor = 1f;
            _speedAdjustorDirection = 1;
            //_sprite = GetComponent<tk2dSprite>();
            _renderer = GetComponent<Renderer>();
            _mesh = GetComponent<MeshFilter>().mesh;
            var width = (float)_renderer.material.mainTexture.width;
            var height = (float)_renderer.material.mainTexture.height;
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
            Vector2[] uvs = new Vector2[]
            {
                new Vector2(_uvOffset.x / width, _uvOffset.y / height),
                new Vector2((16f + _uvOffset.x) / width, (16f + _uvOffset.y) / height),
                new Vector2((16f + _uvOffset.x) / width, _uvOffset.y / height),                
                new Vector2(_uvOffset.x / width, (16f + _uvOffset.y) / height)
            };
            _mesh.uv = uvs;
            //_mesh.uv[0] = new Vector2();
            //tk2dSprite sp;
            //sp.CurrentSprite.materialInst.
        }

        public void Update()
        {
            if (this.enabled)
            {
                _currentUvOffset += uvAnimationRate * Time.deltaTime * _speedAdjustor;
                _renderer.material.SetTextureOffset(_textureName, _currentUvOffset);

                //_speedAdjustor += Time.deltaTime * _speedAdjustorDirection;

                if (_speedAdjustorDirection == 1 && _speedAdjustor > 2)
                {
                    _speedAdjustorDirection = -_speedAdjustorDirection;
                }
                else if (_speedAdjustorDirection == -1 && _speedAdjustor < 0.5f)
                {
                    _speedAdjustorDirection = -_speedAdjustorDirection;
                }

                //_sprite.CurrentSprite.materialInst.SetTextureOffset(_textureName, _uvOffset);
            }
        }
    }
}
