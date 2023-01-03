using RQ.Common;
using RQ.Entity.Components;
using System;
using System.Collections.Generic;

public interface IComponentRegistrar
{
    //void RegisterComponentAll<T>(T component) where T : IBaseObject;
    void RegisterComponents<T>(IList<T> component) where T : IBaseObject;
    //void UnregisterComponentAll<T>(T component) where T : IBaseObject;
    void UnregisterComponentById<T>(string uniqueId);
    void RegisterComponent<T>(T component) where T : IBaseObject;
    void UnRegisterComponent<T>(T component) where T : IBaseObject;
    T GetComponent<T>() where T : class, IBaseObject;
    T GetComponent<T>(string name) where T : class, IBaseObject;
    IBaseObject GetComponent(string name);
    IList<IBaseObject> GetComponents();
    //IEnumerable<IBaseObject> GetComponentsAll();
    IList<IBaseObject> GetComponents<T>() where T : class, IBaseObject;
    void SetComponentRepository(IComponentRepository _repo);
}