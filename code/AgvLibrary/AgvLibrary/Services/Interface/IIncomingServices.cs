using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;


namespace AgvLibrary.Services.Interface
{
   public interface IIncomingServices
    {
        bool IncomingByNr(string UniqNr, string PositionNr);
        bool IncomingByPosation(string UniqNr, string WHNr, int Floor, int Column, int Row);
    }
}
