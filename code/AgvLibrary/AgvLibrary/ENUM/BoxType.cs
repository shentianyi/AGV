using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AgvLibrary.ENUM
{

        public enum BoxType
        {
            [Description("大箱")]
            BigBox = 1,
            [Description("小箱")]
            SmallBox = 2
        }
    
}
