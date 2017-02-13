using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data
{
   public partial class StorageUniqueItemView
    {
        public string BoxTypeStr
        {
            get
            {
                return BoxType.GetStr(this.UniqueItemBoxTypeId);
            }
        }

        public string FIFOStr
        {
           get {
                return this.FIFO.HasValue ? this.FIFO.Value.ToString("yyyy-MM-dd HH:mm:sss") : string.Empty;  
            }
        }
    }
}
