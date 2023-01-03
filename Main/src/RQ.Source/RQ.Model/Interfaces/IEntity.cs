using RQ.Enums;
using System;
using UnityEngine;
namespace RQ.Common
{
    public interface IEntity
    {
        string name { get; }
        int Id { get; }
        string UniqueId { get; }
        string GetName();
        string GetTag();
        bool IsActive { get; }
        bool isActiveAndEnabled { get; }
        Transform transform { get; }
        GameObject gameObject { get; }
        bool AddToEntityContainer { get; }
        bool GetRecreateOnLoadGame();
        IComponentRegistrar Components { get; }
    }
}
