using HOTS.DataObjects;
using HOTS.Logging;
using HOTS.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HOTS.WCFHost
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HotsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select HotsService.svc or HotsService.svc.cs at the Solution Explorer and start debugging.
    public class HotsService : IHotsService
    {
        //private HeroLogic _logic = new HeroLogic();

        public HotsService()
        {
            //logic = new HeroLogic(ConfigurationManager.AppSettings["Path"]);
            HeroLogic.Instance.SetPath(ConfigurationManager.AppSettings["Path"]);
        }

        /// <summary>
        /// Gets a hero by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<HeroModel> GetHero(string name)
        {
            var task = Task<HeroModel>.Factory.StartNew(() =>
            {
                return HeroLogic.Instance.GetHero(name);
            });
            return await task.ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all heroes
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ConcurrentDictionary<string, HeroModel>> GetHeroes()
        {
            var task = Task<ConcurrentDictionary<string, HeroModel>>.Factory.StartNew(() =>
            {
                return HeroLogic.Instance.GetHeroes();
            });
            return await task.ConfigureAwait(false);
        }

        /// <summary>
        /// Inserts a new hero
        /// </summary>
        /// <param name="hero"></param>
        /// <returns></returns>
        public async Task<string> InsertHero(HeroModel hero)
        {
            var task = Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    Logger.Instance.LogInfo("Inserting hero " + hero.Name);
                    var result = HeroLogic.Instance.InsertHero(hero);
                    Logger.Instance.LogInfo("Hero " + hero.Name + " inserted");
                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogError(ex);
                    throw new FaultException(ex.Message);
                }
            });
            return await task.ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the hero
        /// </summary>
        /// <param name="hero"></param>
        /// <returns></returns>
        public async Task<string> UpdateHero(HeroModel hero)
        {
            var task = Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    Logger.Instance.LogInfo("Updating hero " + hero.Name);
                    var result = HeroLogic.Instance.UpdateHero(hero);
                    Logger.Instance.LogInfo("Hero " + hero.Name + " updated");
                    return result;
                    //return string.Format("You entered: {0}", value);
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogError(ex);
                    throw new FaultException(ex.Message);
                }
            });
            return await task.ConfigureAwait(false);
        }

        /// <summary>
        /// Performs a soft delete
        /// </summary>
        /// <param name="heroName"></param>
        /// <returns></returns>
        public async Task DeleteHero(string heroName)
        {
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    Logger.Instance.LogInfo("Deleting hero " + heroName);
                    HeroLogic.Instance.DeleteHero(heroName);
                    Logger.Instance.LogInfo("Hero " + heroName + " deleted");
                    return;
                    //return string.Format("You entered: {0}", value);
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogError(ex);
                    throw new FaultException(ex.Message);
                }
            });
            await task.ConfigureAwait(false);
            return;
        }
    }
}
