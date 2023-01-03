using RQ.Messaging;

namespace RQ.Common.Serialization
{
    public abstract class PersistentObject : MessagingObject
    {
        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Enums.Telegrams.Serialize:
                    Serialize(msg.ExtraInfo);
                    break;
                case Enums.Telegrams.Deserialize:
                    Deserialize(msg.ExtraInfo);
                    break;
            }
            return false;
        }

        public abstract void Serialize(object data);
        //{

        //}

        public abstract void Deserialize(object data);
        //{

        //}
    }
}
