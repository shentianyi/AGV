using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;
using AgvLibrary.Model;

namespace AgvLibrary.Services.Interface
{
   public interface IPartServices
    {
        List<Part> All();

        IQueryable<Part> Search(PartSearchModel partSearchModel);
    }
}
