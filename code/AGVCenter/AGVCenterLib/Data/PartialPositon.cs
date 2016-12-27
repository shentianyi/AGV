using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data
{
    public partial class Position
    {

        public string StorageUniqItemNr
        {
            get
            {
                var s= this.Storage.FirstOrDefault();
                return s == null ? string.Empty : s.UniqItemNr;
            }
        }
    }
}
