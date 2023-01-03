using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using RQ.Common.Serialization;

namespace RQ.Serialization
{
    public class Persistence
    {
        const string FILE_FORMAT = "Rolan_{0}.sav";

        //JSON
        public static void SaveGame(string fileName, GameSerializedData gameData)
        {
            //XMLSerialize<GameSerializedData>(Application.persistentDataPath + "/" + fileName, gameData);
            var folder = GetSaveFolder();

            var filePath = Path.Combine(folder, fileName);

            var data = JsonSerializer.Serialize<GameSerializedData>(gameData);
            //var filePath = Application.persistentDataPath + "/" + fileName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, data);
        }

        public static string[] GetSaveFiles()
        {
            var folder = GetSaveFolder();
            var files = Directory.GetFiles(folder, "*.sav");
            return files;
        }

        public static string CalculateFileName()
        {
            var files = GetSaveFiles();
            int index = 1;
            if (files == null)
                index = 1;
            else
            {
                index = files.Length;
            }
            return string.Format(FILE_FORMAT, index);
        }

        //public static object SerializeObject<T>(T obj) where T : class
        //{
        //    var jsonData = JsonSerializer.Serialize<T>(obj);
        //    return jsonData;
        //}

        //public static T DeserializeObject<T>(object data) where T : class
        //{
        //    var obj = JsonSerializer.Deserialize<T>(data.ToString());
        //    //JObject.Parse(data.ToString()).;
        //    return obj;
        //}

        public static object SerializeObject<T>(T obj) where T : class
        {
            //var jsonData = JsonSerializer.Serialize<T>(obj);
            return obj;
        }

        // JSON
        public static T DeserializeObject<T>(object data)
        {
            if (data == null)
                return default(T);
            var obj = JsonSerializer.Deserialize<T>(data.ToString());
            return obj;
            //JObject.Parse(data.ToString()).;

            //return data as T;
        }

        // Binary
        //public static T DeserializeObject<T>(object data) where T : class
        //{
        //    //if (data == null)
        //    //    return null;
        //    //var obj = JsonSerializer.Deserialize<T>(data.ToString());
        //    //return obj;
        //    //JObject.Parse(data.ToString()).;

        //    return data as T;
        //}

        // Binary
        //public static void SaveGame(string fileName, GameSerializedData gameData)
        //{
        //    Save<GameSerializedData>(gameData, fileName);
        //}

        //public static GameSerializedData LoadGame(string fileName)
        //{
        //    Log.Info("Persistence LoadGame called");
        //    //GameData gameData = Load<GameData>("TestSave.sav");
        //    GameSerializedData gameData = XMLDeserialize<GameSerializedData>(Application.persistentDataPath + "/" + fileName);

        //    return gameData;

        //    //EntityController._instance.ResetEntityList();
        //    //Log.Info("Application.LoadLevel being called");
        //    //GameController._instance.RegisterEntities = false;
        //    //Application.LoadLevel(gameData.SceneId);
        //    //GameController._instance.LoadingGame = true;
        //    ////GameController._instance.InitScene();
        //    //// Loads the Lua script into the Dialogue System
        //    //PersistentDataManager.ApplySaveData(gameData.DialogueSystemData);
        //    //GameController._instance.gameData = gameData;
        //    //SpriteController._instance.Deserialize(gameData.SpriteData);
        //}

        public static GameSerializedData LoadGame(string fileName)
        {
            //Log.Info("Persistence LoadGame called");

            var folder = GetSaveFolder();

            var filePath = Path.Combine(folder, fileName);

            // Binary
            //GameSerializedData gameData = Load<GameSerializedData>(filePath);
            // JSON
            var data = File.ReadAllText(filePath);
            var gameData = JsonSerializer.Deserialize<GameSerializedData>(data);

            return gameData;
        }

        public static bool CheckFileExists(string fileName)
        {
            var folder = GetSaveFolder();

            var filePath = Path.Combine(folder, fileName);
            return File.Exists(filePath);
        }

        private static string GetSaveFolder()
        {
            var saveFolder = GetUnityDataPath() + "\\Save";
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            return saveFolder;
        }

        private static string GetUnityDataPath()
        {
            return Application.persistentDataPath;
        }
        
        /// <summary>
        /// Saves data in binary format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        public static void Save<T>(T data, string fileName) where T : class
        {
            Save<T, BinaryFormatter>(data, fileName);
        }

        public static void Save<T, F>(T data, string fileName)
            where T : class
            where F : IFormatter, new()
        {
            F bf = new F();
            var folder = GetSaveFolder();
            var filePath = Path.Combine(folder, fileName);
            FileStream stream = File.Create(filePath);
            //Log.Info("Saving to " + filePath);
            bf.Serialize(stream, data);
            stream.Close();
        }

        /// <summary>
        /// Loads data in binary format
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Load<T>(string fileName) where T : class
        {
            return Load<T, BinaryFormatter>(fileName);
        }

        public static T Load<T, F>(string fileName)
            where T : class
            where F : IFormatter, new()
        {
            var folder = GetSaveFolder();
            var filePath = Path.Combine(folder, fileName);
            if (File.Exists(filePath))
            {
                F formatter = new F();
                FileStream stream = File.Open(filePath, FileMode.Open);
                T data = formatter.Deserialize(stream) as T;
                stream.Close();
                return data;
            }
            return null;
        }
    }
}
