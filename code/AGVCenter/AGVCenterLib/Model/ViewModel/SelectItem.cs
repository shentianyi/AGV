using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.ViewModel
{
    [DataContract]
    public  class SelectItem
    {
        public int Value { get; set; }
        public string Text { set; get; }
    }
}

