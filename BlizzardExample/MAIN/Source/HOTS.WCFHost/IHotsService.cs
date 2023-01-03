using HOTS.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HOTS.WCFHost
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHotsService" in both code and config file together.
    [ServiceContract]
    public interface IHotsService
    {
        [OperationContract]
        Task<HeroModel> GetHero(string name);

        [OperationContract]
        Task<string> InsertHero(HeroModel hero);
    }
}
