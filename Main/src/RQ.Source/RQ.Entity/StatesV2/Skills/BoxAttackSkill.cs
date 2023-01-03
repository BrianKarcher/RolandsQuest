using RQ.Entity.Components;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Physics;
using RQ.Physics;
using UnityEngine;

namespace RQ.Entity.StatesV2.Skills
{
    [AddComponentMenu("RQ/States/State/Skill/Box Attack")]
    public class BoxAttackSkill : AttackSkill
    {
        [SerializeField]
        private Vector2D _offset = Vector2D.Zero();
        [SerializeField]
        private Vector2D _size = Vector2D.Zero();
        [SerializeField]
        private float _angle = 0;
        [SerializeField]
        private float _distance = 0;
        [SerializeField]
        private bool _sameLayer = true;
        [SerializeField]
        private SingleUnityLayer _layer;
        [SerializeField]
        private GameObject _spawn;
        [SerializeField]
        private float _spawnDelay = 0f;

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("Spawn", this.UniqueId, (data) =>
                {
                    var newGo = Instantiate(_spawn, _componentRepository.transform.position, Quaternion.identity) as GameObject;
                    var newRepo = newGo.GetComponent<IComponentRepository>();
                    var facingDirection = _animationComponent.GetFacingDirection();
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, newRepo.UniqueId,
                        Telegrams.SetFacingDirection, facingDirection);
                });
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("Spawn", this.UniqueId, -1);
        }

        public override void ProcessSkill()
        {
            base.ProcessSkill();
            if (_spawn != null)
            {
                MessageDispatcher2.Instance.DispatchMsg("Spawn", _spawnDelay, this.UniqueId, this.UniqueId, null);
            }
        }

        public override RaycastHit[] CollidersHit()
        {
            var facingDirection = AnimationComponent.GetFacingDirectionVector();
            var pos = _spriteBaseComponent.transform.position;
            int layerMask = 0;
            if (_sameLayer)
            {
                var layerIndex = _collisionComponent.transform.gameObject.layer;
                layerMask = 1 << layerIndex;
                //layerMask = ((LayerMask)).value;
            }
            else
                layerMask = _layer.Mask;
            //GameDataController.Instance.GetLayerMask(LevelLayer.LevelOne);
            //var itemsHit = Physics2D.BoxCastAll(pos + _offset, _size, _angle, facingDirection, _distance, layerMask/*, GetEntityUI().GetTransform().gameObject.layer*/);
            RaycastHit[] itemsHit = UnityEngine.Physics.BoxCastAll(pos + _offset.ToVector3(0), _size.ToVector3(0), facingDirection, Quaternion.identity, _distance);
            return itemsHit;
        }
    }
}
