using System;
namespace RQ.Common
{
    public interface IBaseObject
    {
        int Id { get; }
        string UniqueId { get; }
        string ComponentName { get; set; }
        string GetName();
        string name { get; }
        string GetTag();
        bool IsActive { get; }
    }
}
