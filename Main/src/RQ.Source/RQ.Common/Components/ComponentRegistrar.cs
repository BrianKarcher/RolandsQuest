using RQ.Entity.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Common.Components
{
    public class ComponentRegistrar : IComponentRegistrar
    {
        // TODO Make more components have names so we can make this a Dictionary?
        private List<IBaseObject> _entityComponents;

        private Dictionary<Type, List<IBaseObject>> _dictEntityComponents;
        //private Dictionary<string, List<IBaseObject>> _entityComponents;
        //private Dictionary<string, IBaseObject> _entityComponentsAll;
        public Action<string> PerformUnregister;
        public Action<string, IBaseObject> PerformRegister;
        private IComponentRepository _repo;

        public ComponentRegistrar()
        {
            _entityComponents = new List<IBaseObject>(100);
            _dictEntityComponents = new Dictionary<Type, List<IBaseObject>>();
            //_entityComponents = new Dictionary<string, List<IBaseObject>>();
            //_entityComponentsAll = new Dictionary<string, IBaseObject>();
        }

        public void RegisterComponents<T>(IList<T> components) where T : IBaseObject
        {
            foreach (var component in components)
                RegisterComponent(component);
        }

        public void RegisterComponent<T>(T component) where T : IBaseObject
        {
            //var type = typeof(T);
            //var typeName = typeof(T).Name;
            var componentType = component.GetType();
            //var componentTypeName = component.GetType().Name;

            //if (componentType == "IComponentBase")
            //{
            //    int i = 1;
            //}

            //var componentName = component.name + " " + componentType.ToString();
            //if (_repo.name.Contains("Rolan") && componentName.Contains("PlayerComponent"))
            //    Debug.LogWarning("Registring component " + componentName);
            //PerformRegister(component.UniqueId, component);
            //List<IBaseObject> componentList;
            //if (!_entityComponents.TryGetValue(type.ToString(), out var componentList))
            //{
            //    componentList = new List<IBaseObject>();
            //    _entityComponents.Add(type.ToString(), componentList);
            //}
            //if (!_entityComponents.ContainsKey(type))
            //    _entityComponents.Add(component.UniqueId, component);
            //componentList.Add(component);

            if (!_dictEntityComponents.TryGetValue(componentType, out var dictComponents))
            {
                dictComponents = new List<IBaseObject>(3);
                _dictEntityComponents.Add(componentType, dictComponents);
            }
            // Don't allow duplicates
            foreach (var dupCheckComponent in dictComponents)
            {
                if (dupCheckComponent.UniqueId == component.UniqueId)
                    return;
            }
            dictComponents.Add(component);
            _entityComponents.Add(component);
        }

        //public void RegisterComponentAll<T>(T component) where T : IBaseObject
        //{
        //    //PerformRegister(component.UniqueId, component);
        //    if (!_entityComponentsAll.ContainsKey(component.UniqueId))
        //        _entityComponentsAll.Add(component.UniqueId, component);
        //}

        //public void UnregisterComponentAll<T>(T component) where T : IBaseObject
        //{
        //    //PerformRegister(component.UniqueId, component);
        //    if (_entityComponentsAll.ContainsKey(component.UniqueId))
        //        _entityComponentsAll.Remove(component.UniqueId);
        //}

        public void UnRegisterComponent<T>(T component) where T : IBaseObject
        {            
            if (component == null)
                throw new Exception("Component is null in UnRegisterComponent");
            if (!_dictEntityComponents.TryGetValue(typeof(T), out var components))
                return;
            //var componentName = component.name + " " + component.GetType().ToString();
            //if (_repo.name.Contains("Rolan") && componentName.Contains("PlayerComponent"))
            //    Debug.LogWarning("Unregistring componnet " + componentName);
            //PerformUnregister(component.UniqueId);
            //List<IBaseObject> componentList;
            //if (!_entityComponents.TryGetValue(typeof(T).ToString(), out componentList))
            //    return;
            //var componentList = _entityComponents[typeof(T)];
            //componentList.Remove(component);
            //if (_entityComponents.ContainsKey(component.UniqueId))
            //    _entityComponents.Remove(component.UniqueId);
            _entityComponents.Remove(component);
            _dictEntityComponents[typeof(T)].Remove(component);
        }

        public void UnregisterComponentById<T>(string uniqueId)
        {
            //List<IBaseObject> componentList;
            //if (!_entityComponents.TryGetValue(typeof(T).ToString(), out componentList))
            //    return;
            //var componentList = _entityComponents[typeof(T)];
            IBaseObject componentToRemove = null;
            //var components = _dictEntityComponents[typeof(T)];
            if (!_dictEntityComponents.TryGetValue(typeof(T), out var components))
                return;
            foreach (var component in components)
            {
                if (component.UniqueId == uniqueId)
                {
                    componentToRemove = component;
                    break;
                }
            }
            //for (int i = 0; i < _entityComponents.Count; i++)
            //{
            //    if (_entityComponents[i].UniqueId == uniqueId)
            //    {
            //        componentToRemove = _entityComponents[i];
            //        break;
            //    }
            //}
            if (componentToRemove != null)
            {
                _entityComponents.Remove(componentToRemove);
                components.Remove(componentToRemove);
            }
            //if (_entityComponents.ContainsKey(uniqueId))
            //    _entityComponents.Remove(uniqueId);
        }

        //public void DeleteComponent(string uniqueId)
        //{
        //    if (_entityComponents.ContainsKey(uniqueId))
        //        _entityComponents.Remove(uniqueId);
        //}

        //public void AddComponent(string uniqueId, IBaseObject component)
        //{
        //    if (component == null)
        //        throw new Exception("Component is null in RegisterComponent");
        //    if (!_entityComponents.ContainsKey(component.UniqueId))
        //        _entityComponents.Add(component.UniqueId, component);
        //}

        public T GetComponent<T>() where T : class, IBaseObject
        {
            //return _entityComponents[typeof(T)][0] as T;
            return GetComponent<T>(string.Empty);
        }

        public T GetComponent<T>(string name) where T : class, IBaseObject
        {
            if (name == null)
                name = string.Empty;
            //if (!_entityComponents.TryGetValue(typeof(T).ToString(), out var componentList))
            //    return null;
            if (!_dictEntityComponents.TryGetValue(typeof(T), out var components))
            {
                //Debug.LogError($"(GetComponent<T> Component type {typeof(T).Name}, name {name} not found in {_repo.name}, enumerating whole list.");
                // Sometimes the type requested doesn't match the type registered (for example, may register as concrete type, but requesting an interface)
                // In that case, we need to loop through the whole ist.
                //if (_repo.name.Contains("Plant") && typeof(T).Name.Contains("Damage"))
                //{
                //    int i = 1;
                //}

                components = _entityComponents;
            }
                //return null;

            //var components = _dictEntityComponents[typeof(T)];
            foreach (var component in components)
            {
                if (component is T && component.ComponentName == name)
                {
                    return component as T;
                }
            }

            //for (int i = 0; i < _entityComponents.Count; i++)
            //{
            //    var component = _entityComponents[i];
            //    if (component is T && component.ComponentName == name)
            //    {
            //        return _entityComponents[i] as T;
            //    }
            //}
            //for (int i = 0; i < _entityComponents.Values.Count; i++)
            //{
            //    var component = _entityComponents.Values[i];

            //}
            //var component = _entityComponents.Values.Where(i => i is T && i.ComponentName == name)
            //    .FirstOrDefault();
            //if (component == null)
            //    return null;
            //return component as T;
            return null;
        }

        public IBaseObject GetComponent(string name)
        {
            if (name == null)
                name = string.Empty;

            //var components = _dictEntityComponents[typeof(T)];
            foreach (var component in _entityComponents)
            {
                if (component.ComponentName == name)
                {
                    return component;
                }
            }

            //for (int i = 0; i < _entityComponents.Count; i++)
            //{
            //    var component = _entityComponents[i];
            //    if (component is T && component.ComponentName == name)
            //    {
            //        return _entityComponents[i] as T;
            //    }
            //}
            //for (int i = 0; i < _entityComponents.Values.Count; i++)
            //{
            //    var component = _entityComponents.Values[i];

            //}
            //var component = _entityComponents.Values.Where(i => i is T && i.ComponentName == name)
            //    .FirstOrDefault();
            //if (component == null)
            //    return null;
            //return component as T;
            return null;
        }

        public IList<IBaseObject> GetComponents()
        {
            return _entityComponents;
        }

        //public IEnumerable<IBaseObject> GetComponentsAll()
        //{
        //    return _entityComponentsAll.Values;
        //    //foreach (var component in _entityComponents.Values)
        //    //{

        //    //}
        //}

        /// <summary>
        /// Casting a list as a new type creates a new list. We are returning IBaseObject instead so no new list is created.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<IBaseObject> GetComponents<T>() where T : class, IBaseObject
        {
            //var type = typeof(T);
            //var typeName = typeof(T).Name;
            if (!_dictEntityComponents.TryGetValue(typeof(T), out var components))
            {
                //Debug.LogError($"(GetComponents<T> Component type {typeof(T).Name} not found, allocating new list.");
                components = new List<IBaseObject>();
                foreach (var component in _entityComponents)
                {
                    if (component is T)
                    {
                        components.Add(component);
                    }
                }
                return components;
            }
                //return null;
            //var components = _dictEntityComponents[typeof(T)];
            return components;
            //foreach (var component in components)
            //{
            //    if (component.ComponentName == name)
            //    {
            //        return component as T;
            //    }
            //}

            //List<T> componentsByType = new List<T>();
            //for (int i = 0; i < _entityComponents.Count; i++)
            //{
            //    var component = _entityComponents[i] as T;
            //    if (component != null)
            //        componentsByType.Add(component);
            //}
            //if (!_entityComponents.TryGetValue(typeof(T).ToString(), out var componentList))
            //    return null;
            //return _entityComponents.Values.Where(i => i is T).Select(i => i as T);
            //return componentsByType;
        }

        public void SetComponentRepository(IComponentRepository repo)
        {
            _repo = repo;
        }
    }
}
