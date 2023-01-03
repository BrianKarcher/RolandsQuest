using RQ.Messaging;
using RQ.Model.Item;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    //[AddComponentMenu("RQ/States/State/Attack")]
    public class Skill : AnimatorState
    {
        [SerializeField]
        private float _delay = 0f;
        [SerializeField]
        private bool _stopMoving = true;
        [SerializeField]
        private ItemConfig _skill;
        public ItemConfig GetSkill { get { return _skill; } }
        //private LevelLayer _level = LevelLayer.LevelOne;

        //private ISprite _sprite;

        //public override void SetEntity(MonoBehaviour entity)
        //{
        //    base.SetEntity(entity);
        //    //_sprite = EntityUIBase.GetEntity(entity);
        //    //if (_sprite == null)
        //    //    throw new Exception("FSM - Sprite not set.");
        //}

        public override void Enter()
        {
            base.Enter();
            if (_skill != null)
                MessageDispatcher2.Instance.DispatchMsg("SkillUsed", 0f, this.UniqueId, _componentRepository.UniqueId, _skill);
            if (_stopMoving)
            {
                //var theSprite = _entity as ISprite;
                if (_physicsComponent != null)
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                        Enums.Telegrams.StopMovement, null);
                    //_physicsComponent.Stop();
            }
            MessageDispatcher2.Instance.DispatchMsg("ProcessSkill", _delay, this.UniqueId, this.UniqueId, null);
            //SendMessageToSelf(_delay, Enums.Telegrams.ProcessAttack, 
            //    null, Enums.TelegramEarlyTermination.ChangeScenes);
            //SendLocalMessageToSelf(_strikeDelay, Enums.Telegrams.ProcessAttack, null, Enums.TelegramEarlyTermination.ChangeScenes);
            //MessageDispatcher.Instance.DispatchMsg(_strikeDelay, _entity.MessageHandlerID(), _entity.MessageHandlerID(),
            //    Enums.Telegrams.ProcessAttack, null, Enums.TelegramEarlyTermination.ChangeScenes);
        }

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("ProcessSkill", this.UniqueId, (data) =>
                {
                    ProcessSkill();
                });
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("ProcessSkill", this.UniqueId, -1);
        }

        //public override void Exit()
        //{
        //    base.Exit();
        //    //MessageDispatcher.Instance.RemoveFromQueue(i => i.ReceiverId == this.UniqueId
        //    //    && i.Msg == Enums.Telegrams.ProcessAttack);
        //    //sprite.GetSteering().TurnOff(behavior_type.wander);
        //}



        //public override bool HandleMessage(Telegram telegram)
        //{
        //    if (base.HandleMessage(telegram))
        //        return true;

        //    switch (telegram.Msg)
        //    {
        //        case Enums.Telegrams.ProcessAttack:
        //            ProcessAttack();
        //            //_sprite.ProcessAttack();
        //            return true;
        //    }
        //    return false;
        //}

        public virtual void ProcessSkill()
        {
            //GameDataController.Instance.Data.EntityStats.CurrentSP -= _skill.Value;
        }
    }
}
