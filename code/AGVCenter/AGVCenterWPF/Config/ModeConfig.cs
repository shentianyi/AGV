using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;
using Brilliantech.Framwork.Utils.ConfigUtil;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterWPF.Config
{
    public class ModeConfig
    {
        private static ConfigUtil config;
        private static RoadMachineTaskModel roadMachine1TaskMode = RoadMachineTaskModel.OutHigherThanIn;
        private static RoadMachineTaskModel roadMachine2TaskMode = RoadMachineTaskModel.OutHigherThanIn; 

        static ModeConfig()
        {
            try
            {
                config = new ConfigUtil("TASK", "Config/mode.ini"); 
                roadMachine1TaskMode = (RoadMachineTaskModel)int.Parse(config.Get("roadMachine1TaskMode"));
                roadMachine2TaskMode = (RoadMachineTaskModel)int.Parse(config.Get("roadMachine2TaskMode"));
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }
 
        public static RoadMachineTaskModel RoadMachine1TaskMode
        {
            get
            {
                return roadMachine1TaskMode;
            }

            set
            {
                roadMachine1TaskMode = value;
                config.Set("roadMachine1TaskMode", (int)value);
                config.Save();
            }
        }


        public static RoadMachineTaskModel RoadMachine2TaskMode
        {
            get
            {
                return roadMachine2TaskMode;
            }

            set
            {
                roadMachine2TaskMode = value;
                config.Set("roadMachine2TaskMode", (int)value);
                config.Save();
            }
        }
    }
}
