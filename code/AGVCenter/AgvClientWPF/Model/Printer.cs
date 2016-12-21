using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Brilliantech.ReportGenConnector;
using TECIT.TFORMer;
 
namespace AgvClientWPF.Model
{
    public class Printer
    {
        public string Id { get; set; }
        public string Output { get; set; }
        public string Template { get; set; }
        public string TemplatePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template", this.Template);
            }
        }
        public string Name { get; set; }
        public int Type { get; set; }
        /// <summary>
        /// default is 1
        /// </summary>
        public int Copy { get; set; }

        public void Print(RecordSet data, string printer_name = null, string copy = null)
        {
            string printerName= string.IsNullOrEmpty(printer_name) ? this.Name : printer_name;
            
            IReportGen gen = new TecITGener();
            ReportGenConfig config = new ReportGenConfig()
            {
                Printer =printerName,
                NumberOfCopies = string.IsNullOrEmpty(copy) ? this.Copy : int.Parse(copy),
                PrinterType = (PrinterType)this.Type,
                Template = this.TemplatePath
            };
            gen.Print(data, config);
        }
    }
}
