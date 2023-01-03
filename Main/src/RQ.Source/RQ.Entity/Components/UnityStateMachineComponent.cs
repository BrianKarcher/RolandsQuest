using RQ.Common.Components;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Unity State Machine")]
    public class UnityStateMachineComponent : ComponentPersistence<UnityStateMachineComponent>
    {
        [SerializeField]
        private Animator _animator;
        private long _damagedMessageId;
        private long _hpChangedMessageId;

        public override void StartListening()
        {
            base.StartListening();
            //_damagedMessageId = MessageDispatcher2.Instance.StartListening("Damaged", _componentRepository.UniqueId, (data) =>
            //{

            //});
            _hpChangedMessageId = MessageDispatcher2.Instance.StartListening("EntityHPChanged", _componentRepository.UniqueId, (data) =>
            {
                var hp = (float)data.ExtraInfo;
                //_animator.SetFloat("HP", hp);
                if (hp <= 0)
                    _animator.SetTrigger("Dead");
                else
                    _animator.SetTrigger("Damaged");
            });
        }

        public override void StopListening()
        {
            base.StopListening();
            //MessageDispatcher2.Instance.StopListening("Damaged", _componentRepository.UniqueId, _damagedMessageId);
            MessageDispatcher2.Instance.StopListening("EntityHPChanged", _componentRepository.UniqueId, _hpChangedMessageId);
        }
    }
}
