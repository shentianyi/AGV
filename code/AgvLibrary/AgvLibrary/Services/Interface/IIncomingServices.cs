using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;
using AgvLibrary.Model.Message;


namespace AgvLibrary.Services.Interface
{
   public interface IIncomingServices
    {
        BasicMessage IncomingByNr(string UniqNr, string PositionNr);
        BasicMessage IncomingByPosation(string UniqNr, string WHNr, int Floor, int Column, int Row);

       bool CreateIncomingMovement(string PositionNr,DateTime CreatedAt);
    }
}
