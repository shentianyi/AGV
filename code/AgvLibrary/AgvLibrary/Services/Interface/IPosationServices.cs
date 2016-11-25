using AgvLibrary.Data;
using AgvLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Services.Interface
{
   public interface IPosationServices
    {
        IQueryable<Posation> Search(PosationSearchModel posationSearchModel);
    }
}
