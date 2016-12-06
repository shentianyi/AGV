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

        public BasicMessage IncomingByNr(string UniqNr, string PositionNr)
        {
           
            if(UniqRep.UnqiNrExist(UniqNr)==false)
            {
                Message.MsgText = "唯一吗不在UniqItem表中";
                Message.Result = false;
                return Message;
            }
            UniqueItem UI = UniqRep.SearchByUniqNr(PositionNr);

            string PartNr = UI.PartNr;
            DateTime FIFO = UI.CreatedAt;
           

            if (PoRep.PositionNrExist(PositionNr)==false)
            {
                Message.MsgText = "PositionNr不在Position表中";
                Message.Result = false;
                return Message;
            }
            if(SaRep.PositionNrExist(PositionNr))
            {
                Message.MsgText = "PositionNr已占用库位";
                Message.Result = false;
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
                    

                    if (InMoCre)
                    {
                        Message.MsgText = "入库成功并写入日志";
                        Message.Result = true;
                        return Message;

                    }
                    else
                    {
                        Message.MsgText = "入库成功，但未写入日志";
                        Message.Result = false;
                        return Message;
                    }
                }
                else
                {
                    Message.MsgText = "入库未成功";
                    Message.Result = false;
                    return Message;
                }
                
            }
            catch(Exception ex)
            {
                Message.MsgText = ex.Message;
                Message.Result = false;
                return Message;
            }

            
            
            
        }

        public BasicMessage IncomingByPosation(string UniqNr, string WHNr, int Floor, int Row,int Column)
        {
          Position  Po= PoRep.SearchByPosition(WHNr, Floor, Row, Column);
            if (string.IsNullOrEmpty(Po.PositionNr))
            {
                Message.MsgText = "PositionNr在Position表中不存在";
                Message.Result = false;
                return Message;
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
