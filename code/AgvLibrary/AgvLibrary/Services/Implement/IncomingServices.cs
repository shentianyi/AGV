using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Services.Interface;
using AgvLibrary.Data.Repository.Implement;

namespace AgvLibrary.Services.Implement
{
   public class IncomingServices : ServiceBase,IIncomingServices
    {
        private IUniqueItemRepository UniqRep;
        private IPosationRepository PoRep;
        private IStorageRepository SaRep;
        

        public IncomingServices(string dbString) : base(dbString) {
           UniqRep = new UniqueItemRepository(this.Context);
            PoRep = new PositionRepository(this.Context);
            SaRep = new StorageRepository(this.Context);
        }

        public bool IncomingByNr(string UniqNr, string PositionNr)
        {
           
            if(UniqRep.UnqiNrExist(UniqNr)==false)
            {
                return false;
            }
            UniqueItem UI = UniqRep.SearchByUniqNr(PositionNr);

            string PartNr = UI.PartNr;
            DateTime FIFO = UI.CreatedAt;
           

            if (PoRep.PositionNrExist(PositionNr)==false)
            {
                return false;
            }
            if(SaRep.PositionNrExist(PositionNr))
            {
                return false;
            }
            Storage sg = new Storage
            {
                PositionNr = PositionNr,
                PartNr = PartNr,
                FIFO = FIFO,
                UniqItemNr = UniqNr,
                CreatedAt = DateTime.Now

            };
            try
            {

                SaRep.Create(sg);
                return true;
            }
            catch
            {
                return false;
            }

            
            
            
        }

        public bool IncomingByPosation(string UniqNr, string WHNr, int Floor, int Column, int Row)
        {
            throw new NotImplementedException();
        }

       
    }
}
