using HOTS.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Concurrent;
using System.Web;
using System.Threading;

namespace HOTS.DAL
{
    public class HotsDAL
    {
        private Mutex _mutex = new Mutex();
        private string _path;
        public HotsDAL(string path)
        {
            _path = HttpContext.Current == null ? Directory.GetCurrentDirectory() + "\\" + path : HttpContext.Current.Server.MapPath(path);
            if (!File.Exists(_path))
            {
                File.Create(_path);
            }
        }

        public ConcurrentDictionary<string, HeroModel> ReadFromFile()
        {
            string fileText = string.Empty;
            if (_mutex.WaitOne())
            {
                fileText = File.ReadAllText(_path);
            }
            _mutex.ReleaseMutex();
            var heros = JsonConvert.DeserializeObject<ConcurrentDictionary<string, HeroModel>>(fileText);

            return heros;
        }

        public void WriteToFile(ConcurrentDictionary<string, HeroModel> heroes)
        {
            var fileContents = JsonConvert.SerializeObject(heroes);
            if (_mutex.WaitOne())
            {
                File.WriteAllText(_path, fileContents);
            }
            _mutex.ReleaseMutex();
        }
    }
}
