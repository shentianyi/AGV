using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data.Repository.Interface
{
   public interface IPosationRepository
    {
        Position FindByPositionNr(string PositionNr);

        Position FindByPosition(string WHNr, int Floor, int Column, int Row);
        Position FindByState(int State);
        bool PositionNrExist(string PositionNr);
        bool PositionExist(string WHNr, int Floor, int Column, int Row);
        bool StateExist(int State);

    }
}
