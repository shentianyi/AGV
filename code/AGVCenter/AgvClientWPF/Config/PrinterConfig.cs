using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.ConfigUtil;
using System.Drawing.Printing;
using AgvClientWPF.Model;

namespace AgvClientWPF.Config
{
    public class PrinterConfig
    {
        private static ConfigUtil printerConfig;
        private static List<Printer> printers;
        private static List<string> systemPrinters;

        public static List<string> SystemPrinters
        {
            get
            {
                systemPrinters = new List<string>();
                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    systemPrinters.Add(printer);
                }
                return PrinterConfig.systemPrinters;
            }
        }

        static PrinterConfig()
        {
            try
            {
                printerConfig = new ConfigUtil("Config/printers.ini");
                List<string> printerIds = printerConfig.Notes();
                printers = new List<Printer>();

                foreach (string id in printerIds)
                {
                    printers.Add(new Printer()
                    {
                        Id = id,
                        Output = printerConfig.Get("Output", id),
                        Template = printerConfig.Get("Template", id),
                        Name = printerConfig.Get("Name", id),
                        Type = int.Parse(printerConfig.Get("Type", id)),
                        Copy = int.Parse(printerConfig.Get("Copy", id))

                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Printer> Printers
        {
            get { return PrinterConfig.printers; }
            set { PrinterConfig.printers = value; }
        }

        public static void Save(Printer printer)
        {
            printerConfig.Set("Name", printer.Name, printer.Id);
            printerConfig.Set("Type", printer.Type, printer.Id);
            printerConfig.Set("Copy", printer.Copy, printer.Id);

            //// for change stock
            //if (printer.ChangeStock)
            //{
            //    printerConfig.Set("ChangeStock", printer.ChangeStock, printer.Id);
            //    printerConfig.Set("StockName", printer.StockName, printer.Id);
            //    printerConfig.Set("StockID", printer.StockID, printer.Id);
            //    printerConfig.Set("Orientation", printer.Orientation, printer.Id);
            //}
            printerConfig.Save();
        }

        public static Printer Find(string code)
        {
            return printers.Find(delegate(Printer printer)
            {
                return printer.Id == code;
            });
        }
    }

    public class PrintSet
    {
        private List<Printer> defaultPrinters;

        private List<string> systemPrinters;

        public List<Printer> DefaultPrinters
        {
            get { return defaultPrinters; }
            set { defaultPrinters = value; }
        }


        public List<string> SystemPrinters
        {
            get { return systemPrinters; }
            set { systemPrinters = value; }
        }
    }
}
