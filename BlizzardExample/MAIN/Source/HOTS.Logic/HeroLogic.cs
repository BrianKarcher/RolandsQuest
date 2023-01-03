using HOTS.DataObjects;
using HOTS.DAL;
using HOTS.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HOTS.Logic
{
    public class HeroLogic
    {
        private static Mutex _heroesSetupMutex = new Mutex();
        HotsDAL _dal;
        private string _path;

        private ConcurrentDictionary<string, HeroModel> _heroes;
        public ConcurrentDictionary<string, HeroModel> Heroes
        {
            get
            {
                if (_heroes == null)
                {
                    if (_heroesSetupMutex.WaitOne())
                    {
                        _heroes = _dal.ReadFromFile();
                        if (_heroes == null)
                            _heroes = new ConcurrentDictionary<string, HeroModel>();
                    }
                    _heroesSetupMutex.ReleaseMutex();
                }
                return _heroes;
            }
        }

        // This is a singleton
        public static readonly HeroLogic Instance = new HeroLogic();

        private HeroLogic()
        {
            
        }

        public void SetPath(string path)
        {
            _path = path;
            _dal = new HotsDAL(_path);
        }

        public HeroModel GetHero(string name)
        {
            return GetHeroes()[name];
        }

        public ConcurrentDictionary<string, HeroModel> GetHeroes()
        {
            return Heroes;
        }

        public string InsertHero(HeroModel hero)
        {
            ValidateData(hero);
            if (!Heroes.TryAdd(hero.Name, hero))
            {
                throw new Exception("Hero already exists");
            }
            _dal.WriteToFile(Heroes);
            Logger.Instance.LogInfo("Added new hero " + hero.Name + "!");
            return hero.Name;
        }

        public void DeleteHero(string heroName)
        {
            var heroToDelete = Heroes[heroName].Clone() as HeroModel;
            heroToDelete.IsDeleted = true;
            if (!Heroes.TryUpdate(Heroes[heroName].Name, heroToDelete, Heroes[heroName]))
            {
                throw new Exception("Hero has already been modified, update not saved.");
            }

            _dal.WriteToFile(Heroes);
            Logger.Instance.LogInfo("Deleted hero " + Heroes[heroName].Name + "!");
        }

        public string UpdateHero(HeroModel hero)
        {
            ValidateData(hero);
            Heroes.AddOrUpdate(hero.Name, hero, (key, newValue) =>
            {
                return newValue;
            });
            _dal.WriteToFile(Heroes);
            Logger.Instance.LogInfo("Updated new hero " + hero.Name + "!");
            return hero.Name;
        }

        private void ValidateData(HeroModel heroModel)
        {
            bool validateAllProperties = false;

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                heroModel,
                new ValidationContext(heroModel, null, null),
                results,
                validateAllProperties);

            if (results.Count != 0)
            {
                string resultMessage = string.Empty;
                foreach (var result in results)
                {
                    resultMessage += result.ErrorMessage;
                }
                throw new Exception(resultMessage);
            }
        }
    }
}
