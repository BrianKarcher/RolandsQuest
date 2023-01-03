using RQ.Physics;
using RQ.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Model
{
    [Serializable]
    public class Variables
    {
        // Unity does not recognize dictionaries unfortanately
        [SerializeField]
        private List<StringVariable> StringVariables;
        [SerializeField]
        private List<BoolVariable> BoolVariables;
        [SerializeField]
        private List<FloatVariable> FloatVariables;
        [SerializeField]
        private List<Vector2Variable> Vector2Variables;
        [SerializeField]
        private List<Vector3Variable> Vector3Variables;

        private Dictionary<string, string> StringVariablesDict;
        private Dictionary<string, bool> BoolVariablesDict;
        private Dictionary<string, float> FloatVariablesDict;
        private Dictionary<string, Vector2D> Vector2VariablesDict;
        private Dictionary<string, Vector3Serializer> Vector3VariablesDict;

        public void Init()
        {
            StringVariablesDict = new Dictionary<string, string>();
            for (int i = 0; i < StringVariables.Count; i++)
            {
                StringVariablesDict.Add(StringVariables[i].Name, StringVariables[i].Value);
            }

            BoolVariablesDict = new Dictionary<string, bool>();
            for (int i = 0; i < BoolVariables.Count; i++)
            {
                BoolVariablesDict.Add(BoolVariables[i].Name, BoolVariables[i].Value);
            }

            FloatVariablesDict = new Dictionary<string, float>();
            for (int i = 0; i < FloatVariables.Count; i++)
            {
                FloatVariablesDict.Add(FloatVariables[i].Name, FloatVariables[i].Value);
            }

            Vector2VariablesDict = new Dictionary<string, Vector2D>();
            for (int i = 0; i < Vector2Variables.Count; i++)
            {
                Vector2VariablesDict.Add(Vector2Variables[i].Name, Vector2Variables[i].Value);
            }

            Vector3VariablesDict = new Dictionary<string, Vector3Serializer>();
            for (int i = 0; i < Vector3Variables.Count; i++)
            {
                Vector3VariablesDict.Add(Vector3Variables[i].Name, Vector3Variables[i].Value);
            }
            //StringVariablesDict = StringVariables.ToDictionary(i => i.Name, i => i.Value);
            //BoolVariablesDict = BoolVariables.ToDictionary(i => i.Name, i => i.Value);
            //FloatVariablesDict = FloatVariables.ToDictionary(i => i.Name, i => i.Value);
            //Vector2VariablesDict = Vector2Variables.ToDictionary(i => i.Name, i => i.Value);
            //Vector3VariablesDict = Vector3Variables.ToDictionary(i => i.Name, i => i.Value);
        }

        public string GetString(string name)
        {
            return Get(StringVariablesDict, name);
        }

        public void SetString(string name, string value)
        {
            Set(StringVariablesDict, name, value);
        }

        public bool GetBool(string name)
        {
            return Get(BoolVariablesDict, name);
        }

        public void SetBool(string name, bool value)
        {
            Set(BoolVariablesDict, name, value);
        }

        public float GetFloat(string name)
        {
            return Get(FloatVariablesDict, name);
        }

        public void SetFloat(string name, float value)
        {
            Set(FloatVariablesDict, name, value);
        }

        public Vector2D GetVector2(string name)
        {
            return Get(Vector2VariablesDict, name);
        }

        public void SetVector2(string name, Vector2D value)
        {
            Set(Vector2VariablesDict, name, value);
        }

        //public string GetString(string name)
        //{
        //    if (!StringVariablesDict.TryGetValue(name, out string variable))
        //        throw new Exception($"Variable {name} not found");
        //    return variable;
        //}

        //public void SetString(string name, string value)
        //{
        //    if (!StringVariablesDict.ContainsKey(name))
        //        StringVariablesDict.Add(name, value);
        //    else
        //        StringVariablesDict[name] = value;
        //}

        public T Get<T>(Dictionary<string, T> dict, string name)
        {
            if (!dict.TryGetValue(name, out T variable))
                throw new Exception($"Variable {name} not found");
            return variable;
        }

        public void Set<T>(Dictionary<string, T> dict, string name, T value)
        {
            if (!dict.ContainsKey(name))
                dict.Add(name, value);
            else
                dict[name] = value;
        }
    }
}
