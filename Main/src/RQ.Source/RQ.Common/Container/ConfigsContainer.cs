//using RQ.Common.Config;
//using RQ.Model.Interfaces;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace RQ.Common.Container
//{
//    public class ConfigsContainer
//    {
//        private static ConfigsContainer _instance;
//        private Dictionary<string, IRQConfig> _configs;

//        public static ConfigsContainer Instance
//        {
//            get
//            {
//                if (_instance == null)
//                {
//                    _instance = new ConfigsContainer();
//                    _instance.Setup();
//                }
//                return _instance;
//            }
//        }

//        private void Setup()
//        {
//            _configs = new Dictionary<string, IRQConfig>();
//            var configs = Resources.LoadAll<RQBaseConfig>("");
//            //AssetDatabase
//            //var configs = Resources.FindObjectsOfTypeAll<RQBaseConfig>();
//            foreach (var config in configs)
//            {
//                if (_configs.ContainsKey(config.UniqueId))
//                    throw new Exception($"(ConfigsContainer) Key { config.UniqueId } already exists. { config.name}");
//                _configs.Add(config.UniqueId, config);
//            }
//        }

//        public T GetConfig<T>(string uniqueId) where T : class
//        {
//            if (!_configs.ContainsKey(uniqueId))
//            {
//                Debug.LogError($"Could not find Config {uniqueId}");
//                string error = "Error!";
//            }
//            return _configs[uniqueId] as T;
//        }

//        public IRQConfig GetConfig(string uniqueId)
//        {
//            return _configs[uniqueId];
//        }
//    }
//}
