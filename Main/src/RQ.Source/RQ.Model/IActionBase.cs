using RQ.Common;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Serialization;
using System;
using UnityEngine;
namespace RQ.Model
{
    public interface IAction : ISerializableObject
    {
        void Act(Component otherRigidBody);
        void ActExit(Component otherRigidBody);
        //void SetEntity(IComponentRepository entity);
        void InitAction();
        void FixedUpdate();
        void Reset();
        void SetState(IState state);
        void SetComponentRepository(IComponentRepository entity);
        bool isActiveAndEnabled { get; }
        //void Serialize(EntitySerializedData entityData);
        //void Deserialize(EntitySerializedData entityData);

        //void Act(Component otherRigidBody);
        //{ }

        //void ActExit(Component otherRigidBody);
        //{ }
    }
}
