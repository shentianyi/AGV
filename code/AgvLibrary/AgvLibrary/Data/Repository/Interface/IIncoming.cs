using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data.Repository.Interface
{
  public  interface IIncoming
    {
        UniqueItem SearchByUniqNr(string UniqNr);

        bool UnqiNrExist(string UniqNr);
        Warehouse SearchByWHNr(string WHNr);
        bool WHNrExist(string WHNr);



        Storage SearchByUniqueNr(string UniqueItemNr);

        bool UniqueItemNrExist(string UniqueItemNr);

        Position FindByPositionNr(string PositionNr);

        Position FindByPosition(string WHNr, int Floor, int Column, int Row);
       
        bool PositionNrExist(string PositionNr);
        bool PositionExist(string WHNr, int Floor, int Column, int Row);

        bool Create(Storage sg);
    }
}
