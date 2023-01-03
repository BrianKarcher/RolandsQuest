using RQ.Common;
using System;
namespace RQ.Messaging
{
    public interface IMessagingObject : IBaseObject
    {
        bool HandleMessage(Telegram msg);
    }
}
