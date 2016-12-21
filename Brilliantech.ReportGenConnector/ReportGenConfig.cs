using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TECIT.TFORMer;

namespace Brilliantech.ReportGenConnector
{
    [DataContract]
    public class ReportGenConfig
    {
        private string pf_template;
        private string pf_printer;
        private PrinterType pf_printerType;
        private int pf_copies = 1;
        /// <summary>
        /// 打印数量
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [DataMember]
        public int NumberOfCopies
        {
            get { return pf_copies; }
            set { pf_copies = value; }
        }

        /// <summary>
        /// 标签模板的绝对地址
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [DataMember]
        public string Template
        {
            get { return pf_template; }
            set { pf_template = value; }
        }

        /// <summary>
        /// Windows打印机名
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [DataMember]
        public string Printer
        {
            get { return pf_printer; }
            set { pf_printer = value; }
        }

        [DataMember]
        public PrinterType PrinterType
        {
            get { return pf_printerType; }
            set { pf_printerType = value; }
        }
    }
}
