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
    public class OutcomingServices : ServiceBase, IOutcomingServices
    {
        private IUniqueItemRepository UniqRep;
        private IPosationRepository PoRep;
        private IStorageRepository SaRep;
        private IMovementRepository MoRep;
        private BasicMessage Message = new BasicMessage();




        public OutcomingServices(string dbString) : base(dbString) {
            UniqRep = new UniqueItemRepository(this.Context);
            PoRep = new PositionRepository(this.Context);
            SaRep = new StorageRepository(this.Context);
            MoRep = new MovementRepository(this.Context);
        }

        public BasicMessage OutComingByUniqNr(string UniqNr)
        {
           
            if(string.IsNullOrEmpty(UniqNr))
            {
                Message.MsgText = "唯一码不能为空";
                Message.Result = false;
                return Message;
            }
            if(UniqRep.UnqiNrExist(UniqNr)==false)
            {
                Message.MsgText = "唯一码在UniqItem表中不存在";
                Message.Result = false;
                return Message;
            }
            UniqueItem UI = UniqRep.SearchByUniqNr(UniqNr);
            Storage Sg = SaRep.SearchByUniqueNr(UniqNr);
            if (string.IsNullOrEmpty(Sg.PositionNr))
            {
                Message.MsgText = "PositionNr在Storage表中不存在";
                Message.Result = false;
                return Message;
            }
            bool DeRe = SaRep.Delete(Sg);
            if(DeRe)
            {
                bool OutMoRe = CreateOutcomingMovement(Sg.PositionNr, UI.CreatedAt);
                if (OutMoRe)
                {
                    Message.MsgText = "出库成功，并且写入Movement表";
                    Message.Result = true;
                    return Message;
                }
                else
                {
                    Message.MsgText = "出库成功，但无法写入Movement表";
                    Message.Result = false;
                    return Message;
                }

            }
            else
            {
                Message.MsgText = "出库失败";
                Message.Result = false;
                return Message;
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
