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
    public class OutcomingServices : ServiceBase, IOutcomingServices
    {
        private IUniqueItemRepository UniqRep;
        private IPosationRepository PoRep;
        private IStorageRepository SaRep;
        private IMovementRepository MoRep;




        public OutcomingServices(string dbString) : base(dbString) {
            UniqRep = new UniqueItemRepository(this.Context);
            PoRep = new PositionRepository(this.Context);
            SaRep = new StorageRepository(this.Context);
            MoRep = new MovementRepository(this.Context);
        }

        public bool OutComingByUniqNr(string UniqNr)
        {
           
            if(string.IsNullOrEmpty(UniqNr))
            {
                return false;
            }
            if(UniqRep.UnqiNrExist(UniqNr)==false)
            {
                return false;
            }
            UniqueItem UI = UniqRep.SearchByUniqNr(UniqNr);
            Storage Sg = SaRep.SearchByUniqueNr(UniqNr);
            if (string.IsNullOrEmpty(Sg.PositionNr))
            {

                return false;
            }
            bool DeRe = SaRep.Delete(Sg);
            if(DeRe)
            {
                bool OutMoRe = CreateOutcomingMovement(Sg.PositionNr, UI.CreatedAt);
                if (OutMoRe)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
           
          
        }

        public bool CreateOutcomingMovement(string SourcePosition, DateTime CreatedAt)
        {
            Movement Mo = new Movement
            {
                SourcePosition =SourcePosition,
                AimedPosition ="运单目的地",
                OperationType = 2,
                Operator = "System",
                Time = DateTime.Now,
                CreatedAt = CreatedAt
            };

            bool CeRe = MoRep.Create(Mo);
            return CeRe;
        }
    }
}
