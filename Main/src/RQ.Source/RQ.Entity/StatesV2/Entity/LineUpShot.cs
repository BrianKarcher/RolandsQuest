using RQ.Messaging;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Entity/Line Up Shot")]
    public class LineUpShot : AnimatorState
    {
        [SerializeField]
        private float _strikeDelay = 0f;
        [SerializeField]
        private bool _stopMovingDuringAttack = true;

        public override void Enter()
        {
            base.Enter();
            if (_stopMovingDuringAttack)
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                    Enums.Telegrams.StopMovement, null);
                //_physicsComponent.Stop();
            SendMessageToSelf(_strikeDelay, Enums.Telegrams.ProcessAttack, null, 
                Enums.TelegramEarlyTermination.ChangeScenes);
            //SendLocalMessageToSelf(_strikeDelay, Enums.Telegrams.ProcessAttack, 
            //    null, Enums.TelegramEarlyTermination.ChangeScenes);
            //MessageDispatcher.Instance.DispatchMsg(_strikeDelay, _sprite.MessageHandlerID(), _sprite.MessageHandlerID(),
            //    Enums.Telegrams.ProcessAttack, null, Enums.TelegramEarlyTermination.ChangeScenes);
        }

        public override void Exit()
        {
            base.Exit();
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }

        public override bool HandleMessage(Telegram telegram)
        {
            if (base.HandleMessage(telegram))
                return true;

            switch (telegram.Msg)
            {
                case Enums.Telegrams.ProcessAttack:
                    //_physicsComponent.ProcessAttack();
                    return true;
            }
            return false;
        }
    }
}
