using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvWareHouseLibrary.Data.Interface;
using AgvWareHouseLibrary.Model;

namespace AgvWareHouseLibrary.Data.Implement
{
   
    public class PartRepository : IPart
    {
        private AgvWareHouseDataContext context;
        public IQueryable<Data.Part> Search(PartSearchModel partSearchModel)
        {
            IQueryable<Data.Part> Parts = this.context.Part;
            
            if (!string.IsNullOrEmpty( partSearchModel.PartNr))
            {
                Parts = Parts.Where(c => c.PartNr.Equals(partSearchModel.PartNr));
            }
            return Parts;
        }
    }
}
