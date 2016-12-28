using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;

namespace AGVCenterLib.Model
{
    public class RemoteTaskActionItem
    {
        public RemoteActionType ActionType { get; set; }

        public int TaskDbId { get; set; }
        
    }
}
