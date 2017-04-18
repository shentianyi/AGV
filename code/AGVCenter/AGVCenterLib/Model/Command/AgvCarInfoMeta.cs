using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Model.Command
{
    public class AgvCarInfoMeta
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string Route { get; set; }
        public string Point { get; set; }
        public string Voltage { get; set; }
    }
}