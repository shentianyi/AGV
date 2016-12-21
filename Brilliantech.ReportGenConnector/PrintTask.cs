using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Brilliantech.ReportGenConnector
{
    [DataContract]
    public class PrintTask
    {
        private RecordSet pf_dataset;

        private ReportGenConfig pf_config;
        /// <summary>
        /// 打印数据集合
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [DataMember]
        public RecordSet DataSet
        {
            get
            {
                if (pf_dataset == null)
                {
                    pf_dataset = new RecordSet();
                }
                return pf_dataset;
            }

            set { pf_dataset = value; }
        }

        /// <summary>
        /// 打印机本参数
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [DataMember]
        public ReportGenConfig Config
        {
            get
            {
                if (pf_config == null)
                {
                    pf_config = new ReportGenConfig();
                }
                return pf_config;
            }
            set { pf_config = value; }
        }

    }
}
