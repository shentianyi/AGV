using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AGVCenterLib.Model.ViewModel;
using AgvClientWPF.AgvDeliveryService;
using AgvClientWPF.Config;
using AgvClientWPF.Model;
using Brilliantech.ReportGenConnector;

namespace AgvClientWPF.Helper
{
    public class PrintHelper
    {
        public static void PrintDelivery(string deliveryNr)
        {
            try
            {
                Printer printer = PrinterConfig.Find("P001");

                DeliveryServiceClient dsc = new DeliveryServiceClient();
                List<DeliveryStorageViewModel> models = dsc.GetDeliveryStorageByNr(deliveryNr).ToList();
                RecordSet rs = new RecordSet();
                int trayNum = models.Where(s => !string.IsNullOrEmpty(s.TrayItemTrayNr)).Select(s => s.TrayItemTrayNr).Distinct().Count();
                foreach(var m in models)
                {
                    RecordData rd =new RecordData();
                    rd.Add("DATE", m.CreatedAt.HasValue ? m.CreatedAt.Value.ToString("yyyy-MM-dd") : "");

                    rd.Add("Time", m.CreatedAt.HasValue ? m.CreatedAt.Value.ToString("HH:mm:ss") : "");

                    rd.Add("ASN_Nr", m.Nr);
                    rd.Add("Platte_Qty", trayNum.ToString());
                    rd.Add("KSK_Nr", m.UniqueItemNr);
                    rd.Add("Palett_Nr", m.TrayItemTrayNr);

                    rs.Add(rd);

                }
                printer.Print(rs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
