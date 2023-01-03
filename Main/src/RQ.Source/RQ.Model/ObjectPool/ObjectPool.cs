using RQ.Entity.Components;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RQ.Model.ObjectPool
{
    [AddComponentMenu("RQ/Components/Object Pool")]
    public class ObjectPool : MonoBehaviour
    {
        [Serializable]
        public struct Pool
        {
            public GameObject Prefab;
            public string Name;
            public int Qty;
            //public Dictionary<string, Queue<GameObject>> _gameObjectsInPool;
        }

        private static ObjectPool _instance = null;
        private Dictionary<ObjectPoolType, Queue<object>> _objectsInPool;
        public List<Pool> GlobalPools;
        private Dictionary<string, Queue<GameObject>> _gameObjectsInPool;
        private Dictionary<string, Pool> _dictPools;
        public List<GameObject> DebugData = new List<GameObject>();

        //private ObjectPool()
        //{

        //}

        public void Awake()
        {
            // The Comparer avoids boxing the Enum keys
            _objectsInPool = new Dictionary<ObjectPoolType, Queue<object>>(new ObjectPoolTypeComparer());
            _gameObjectsInPool = new Dictionary<string, Queue<GameObject>>();
            _dictPools = new Dictionary<string, Pool>();
            var gos = AddPools(GlobalPools);
            var persistentScene = SceneManager.GetSceneByName("Persistent Scene");
            
            foreach (var go in gos)
            {
                SceneManager.MoveGameObjectToScene(go, persistentScene);
            }
        }

        public List<GameObject> AddPools(List<Pool> pools)
        {
            //_objectsInPool.Clear();
            //DebugData.Clear();
            //_gameObjectPool.Clear();
            List<GameObject> gameObjectsCreated = new List<GameObject>();

            foreach (var pool in pools)
            {
                if (_gameObjectsInPool.ContainsKey(pool.Name))
                    continue;
                var objectPool = new Queue<GameObject>();
                _gameObjectsInPool.Add(pool.Name, objectPool);
                _dictPools.Add(pool.Name, pool);
                //itemInPool._gameObjectsInPool = new Dictionary<string, Queue<GameObject>>();

                if (pool.Qty <= 0)
                    continue;

                for (int i = 0; i < pool.Qty; i++)
                {
                    //Debug.Log($"(ObjectPool) Creating pool for '{pool.Name}'");
                    var newObject = Instantiate(pool.Prefab);
                    newObject.SetActive(false);
                    objectPool.Enqueue(newObject);
                    DebugData.Add(newObject);
                    gameObjectsCreated.Add(newObject);
                }
            }
            return gameObjectsCreated;
        }

        public void DeletePools(List<Pool> pools)
        {
            foreach (var pool in pools)
            {
                var queue = _gameObjectsInPool[pool.Name];

                while (queue.Count != 0)
                {
                    var item = queue.Dequeue();
                    GameObject.DestroyObject(item);
                }
                _gameObjectsInPool.Remove(pool.Name);
                _dictPools.Remove(pool.Name);
            }
        }

        public static ObjectPool Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<ObjectPool>();
                //_instance = new ObjectPool();
                return _instance;
            }
        }

        public T PullFromPool<T>(ObjectPoolType tag) where T : class, new()
        {
            if (!_objectsInPool.TryGetValue(tag, out var objPool))
            {
                return new T();
                //objPool = new Queue<object>();
                //_objectsInPool.Add(tag, objPool);
            }
            if (objPool.Count == 0)
                return new T();

            return objPool.Dequeue() as T;
        }

        public void ReleaseToPool(ObjectPoolType tag, object objectToPool)
        {
            //Queue<object> objPool;
            if (!_objectsInPool.TryGetValue(tag, out var objPool))
            {
                objPool = new Queue<object>();
                _objectsInPool.Add(tag, objPool);
            }

            objPool.Enqueue(objectToPool);
        }

        public bool IsInPool(string name)
        {
            return _gameObjectsInPool.ContainsKey(name);
        }

        public GameObject PullGameObjectFromPool(string name, Vector3 position, Quaternion rotation)
        {
            //Debug.Log("(ObjectPool) Pulling Game Object " + name + " from pool.");
            _gameObjectsInPool.TryGetValue(name, out var objPool);

            if (objPool == null || objPool.Count == 0)
            {
                return InstantiateFromPool(name, position, rotation);
                //return Instantiate(prefab, position, rotation);
            }

            var obj = objPool.Dequeue();
            DebugData.Remove(obj);
            if (obj == null)
            {
                //Debug.LogWarning("(ObjectPool) Game object " + name + " was destroyed, instantiating new.");
                return InstantiateFromPool(name, position, rotation);
                //return Instantiate(prefab, position, rotation);
            }
            var newObject = obj.GetComponent<IComponentRepository>();
            // Call this before enabling the game object so it can properly listen to events when the States get run in OnEnable.
            newObject.ReAwaken();
            // This will cause the State Machine to instantly fire
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        public GameObject InstantiateFromPool(string name, Vector3 position, Quaternion rotation)
        {
            var pool = _dictPools[name];
            //Debug.LogWarning($"(ObjectPool) Game Object {name} not found in Pool, instantiating");
            var newObject = Instantiate(pool.Prefab, position, rotation);
            return newObject;

            //foreach (var pool in Pools)
            //{
            //    if (pool.Name == name)
            //    {
            //        Debug.LogWarning($"(ObjectPool) Game Object {name} not found in Pool, instantiating");
            //        var newObject = Instantiate(pool.Prefab, position, rotation);
            //        return newObject;
            //    }
            //}
            //return null;
        }

        public void ReleaseGameObjectToPool(string name, GameObject objectToPool)
        {
            //Debug.Log($"(ObjectPool) Releasing object {objectToPool.name} to pool.");
            //Queue<object> objPool;
            _gameObjectsInPool.TryGetValue(name, out var objPool);
            // Store the Game Object in the Persistent Scene so it does not get destoryed when the existing scene exits
            //SceneManager.MoveGameObjectToScene(objectToPool, SceneManager.GetSceneByName("Persistent Scene"));
            var repo = objectToPool.GetComponent<IComponentRepository>();
            repo.Reset();
            objectToPool.SetActive(false);
            objPool?.Enqueue(objectToPool);
            DebugData.Add(objectToPool);
        }

        //public static T InstantiateFromPool<T>(string objectPoolName, T prefabGameObject, Vector3 position, Quaternion rotation) where T : UnityEngine.Object
        //{
        //    if (string.IsNullOrEmpty(objectPoolName))
        //    {
        //        return GameObject.Instantiate(prefabGameObject, position, rotation);
        //    }
        //    else
        //    {
        //        var newGO = ObjectPool.Instance.PullGameObjectFromPool(objectPoolName, position, Quaternion.identity);
        //        var newObject = newGO.GetComponent<IComponentRepository>();
        //        newObject.Reset();
        //        //return newGO;
        //        //if (typeof(T) is GameObject)
        //        //    return (T) newGO;
        //        //else
        //        //    return (T) newGO.transform;
        //        return newGO;
        //    }
        //}

        public static GameObject InstantiateFromPool(string objectPoolName, GameObject prefabGameObject, Vector3 position, Quaternion rotation)
        {
            if (string.IsNullOrEmpty(objectPoolName))
            {
                return GameObject.Instantiate(prefabGameObject, position, rotation) as GameObject;
            }
            else
            {
                var newGO = ObjectPool.Instance.PullGameObjectFromPool(objectPoolName, position, Quaternion.identity);
                //var newObject = newGO.GetComponent<IComponentRepository>();
                //newObject.StartListening();
                //newObject.Reset();
                return newGO;
            }
        }

        public static Transform InstantiateFromPool(string objectPoolName, Transform prefabTransform, Vector3 position, Quaternion rotation)
        {
            if (string.IsNullOrEmpty(objectPoolName))
            {
                return GameObject.Instantiate(prefabTransform, position, rotation) as Transform;
            }
            else
            {
                var newGO = ObjectPool.Instance.PullGameObjectFromPool(objectPoolName, position, Quaternion.identity);
                //DebugData.Remove(newGO);
                //var newObject = newGO.GetComponent<IComponentRepository>();
                //newObject.Reset();
                return newGO.transform;
            }
        }
    }
}
