using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Services.Interface;
using AgvLibrary.Data.Repository.Implement;
using AgvLibrary.Model.Message;

namespace AgvLibrary.Services.Implement
{
   public class IncomingServices : ServiceBase,IIncomingServices
    {
        private IUniqueItemRepository UniqRep;
        private IPosationRepository PoRep;
        private IStorageRepository SaRep;
        private IMovementRepository MoRep;
        private BasicMessage Message = new BasicMessage();
        

        public IncomingServices(string dbString) : base(dbString) {
           UniqRep = new UniqueItemRepository(this.Context);
            PoRep = new PositionRepository(this.Context);
            SaRep = new StorageRepository(this.Context);
            MoRep = new MovementRepository(this.Context);
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

               bool SaCre= SaRep.Create(sg);
                if (SaCre)
                {
                    bool InMoCre = CreateIncomingMovement(PositionNr, UI.CreatedAt);

                    if (SaCre && InMoCre)
                    {
                        return true;

                    }
                    else if (SaCre && !InMoCre)
                    {
                        Message.con = "入库成功，但未写入日志";
                        return false;
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
            catch
            {
                return false;
            }

            
            
            
        }

        public bool IncomingByPosation(string UniqNr, string WHNr, int Floor, int Row,int Column)
        {
          Position  Po= PoRep.SearchByPosition(WHNr, Floor, Row, Column);
            if (string.IsNullOrEmpty(Po.PositionNr))
            {
                return false;
            }
            else
            {
                return IncomingByNr(UniqNr, Po.PositionNr);
            }
        }

        public bool CreateIncomingMovement(string PositionNr,DateTime CreatedAt)
        {
            Movement Mo = new Movement
            {
                SourcePosition = "入库缓冲区",
                AimedPosition=PositionNr,
                OperationType=1,
                Operator="System",
                Time=DateTime.Now,
                CreatedAt=CreatedAt
            };

            bool CeRe = MoRep.Create(Mo);
            return CeRe;
            
            
        }
    }
}
