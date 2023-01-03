using RQ.Common.Components;
using RQ.Messaging;
using RQ.Serialization;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/PlayMaker State Machine")]
    public class PlayMakerStateMachineComponent : ComponentPersistence<PlayMakerStateMachineComponent>
    {
        //[SerializeField]
        //private Animator _animator;
        //private long _damagedMessageId;
        //private long _hpChangedMessageId;
        public PlayMakerFSM pm;

        //public override void Start()
        //{
        //    base.Start();
        //    //pm.
        //    //pm.
        //}

        public void SetTemplate(FsmTemplate template)
        {
            pm.SetFsmTemplate(template);
        }

        public void StartFsm()
        {
            if (pm == null || pm.Fsm == null)
                return;
            pm.Fsm.Start();
        }

        public void StopFsm()
        {
            if (pm == null || pm.Fsm == null)
                return;
            pm.Fsm.Stop();
        }

        //public void EnablePlaymaker()
        //{
        //    gameObject.SetActive(true);
        //}

        //public void DisablePlaymaker()
        //{
        //    gameObject.SetActive(false);
        //}

        public override void Update()
        {
            base.Update();
            if (pm == null)
                return;
            if (pm.Fsm == null)
                return;
            // This is so we can view the empty slot in the inspector. Otherwise it is hard to tell if the FSM is finished
            if (pm.Fsm.Finished)
                pm.SetFsmTemplate(null);
        }

        //public override void Reset()
        //{
        //    base.Reset();
        //    var template = pm.FsmTemplate;
        //    pm.Reset();

        //}

        public bool IsFinished()
        {
            if (pm.Fsm == null)
                return true;
            //pm.FsmTemplate.fsm.
            return pm.Fsm.Finished;
        }

        //public override void StartListening()
        //{
        //    base.StartListening();
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
        //}

        //public override void StopListening()
        //{
        //    base.StopListening();
        //    //MessageDispatcher2.Instance.StopListening("Damaged", _componentRepository.UniqueId, _damagedMessageId);
        //    MessageDispatcher2.Instance.StopListening("EntityHPChanged", _componentRepository.UniqueId, _hpChangedMessageId);
        //}

        public override void Serialize(EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            //pm.
        }

        public override void Deserialize(EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);

        }
    }
}
