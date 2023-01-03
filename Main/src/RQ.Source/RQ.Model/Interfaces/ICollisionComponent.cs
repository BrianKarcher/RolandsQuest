using RQ.Common;
using RQ.Physics.Data;
using System;
namespace RQ.Model.Interfaces
{
    public interface ICollisionComponent : IBaseObject
    {
        bool HandleMessage(RQ.Messaging.Telegram msg);
        bool ReceivesDamage();
        CollisionData GetCollisionData();
    }
}
