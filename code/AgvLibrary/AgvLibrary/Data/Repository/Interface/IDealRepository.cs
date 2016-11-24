using AgvLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data.Repository.Interface
{
   public interface IDealRepository
    {
        IQueryable<Deal> Search(DealSearchModel dealSearchModel);
    }
}
