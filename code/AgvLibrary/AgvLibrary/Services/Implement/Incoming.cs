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
   public class Incoming : ServiceBase,IIncomingServices
    {
        public bool IncomingByNr(string UniqNr, string PositionNr)
        {
            throw new NotImplementedException();
        }

        public bool IncomingByPosation(string UniqNr, string WHNr, int Floor, int Column, int Row)
        {
            throw new NotImplementedException();
        }

       
    }
}
