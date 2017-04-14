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
        private static RoadMachineTaskMode roadMachine1PrevTaskMode = RoadMachineTaskMode.OutHigherThanIn;
        private static RoadMachineTaskMode roadMachine1TaskMode = RoadMachineTaskMode.OutHigherThanIn;

        private static RoadMachineTaskMode roadMachine2PrevTaskMode = RoadMachineTaskMode.OutHigherThanIn;
        private static RoadMachineTaskMode roadMachine2TaskMode = RoadMachineTaskMode.OutHigherThanIn; 

        static ModeConfig()
        {
            try
            {
                config = new ConfigUtil("TASK", "Config/mode.ini");
                roadMachine1PrevTaskMode = (RoadMachineTaskMode)int.Parse(config.Get("roadMachine1PrevTaskMode"));
                roadMachine1TaskMode = (RoadMachineTaskMode)int.Parse(config.Get("roadMachine1TaskMode"));

                roadMachine2PrevTaskMode = (RoadMachineTaskMode)int.Parse(config.Get("roadMachine2PrevTaskMode"));
                roadMachine2TaskMode = (RoadMachineTaskMode)int.Parse(config.Get("roadMachine2TaskMode"));
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 设置模式
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool SetMode(int roadMachineIndex, RoadMachineTaskMode mode)
        {
            if (roadMachineIndex == 1)
            {
                if (mode != RoadMachine1TaskMode)
                {
                    if (mode == RoadMachineTaskMode.AutoMoveOnly)
                    {
                        if (RoadMachine1TaskMode != RoadMachineTaskMode.AutoMoveOnly)
                        {
                            RoadMachine1PrevTaskMode = RoadMachine1TaskMode;
                        }
                    }
                    RoadMachine1TaskMode = mode;
                    return true;
                }
            }
            else
            {
                if (mode != RoadMachine2TaskMode)
                {
                    if (mode == RoadMachineTaskMode.AutoMoveOnly)
                    {
                        if (RoadMachine2TaskMode != RoadMachineTaskMode.AutoMoveOnly)
                        {
                            RoadMachine2PrevTaskMode = RoadMachine2TaskMode;
                        }
                    }

                    RoadMachine2TaskMode = mode;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 恢复之前的模式
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <returns></returns>
        public static bool RecoverMode(int roadMachineIndex)
        {
            return SetMode(roadMachineIndex, roadMachineIndex == 1 ? RoadMachine1PrevTaskMode : RoadMachine2PrevTaskMode);
        }


        public static RoadMachineTaskMode RoadMachine1PrevTaskMode
        {
            get
            {
                return roadMachine1PrevTaskMode;
            }

            set
            {
                roadMachine1PrevTaskMode = value;
                config.Set("roadMachine1PrevTaskMode", (int)value);
                config.Save();
            }
        }

        public static RoadMachineTaskMode RoadMachine1TaskMode
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


        public static RoadMachineTaskMode RoadMachine2PrevTaskMode
        {
            get
            {
                return roadMachine2PrevTaskMode;
            }

            set
            {
                roadMachine2PrevTaskMode = value;
                config.Set("roadMachine2PrevTaskMode", (int)value);
                config.Save();
            }
        }

        public static RoadMachineTaskMode RoadMachine2TaskMode
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
