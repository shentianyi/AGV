using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgvWarehouseWeb.Models
{
    public class MoveStockImportModel
    {
        public string 莱尼内部K号 { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public sealed class MoveStockImportModelCsvMap : CsvClassMap<MoveStockImportModel>
    {
        public MoveStockImportModelCsvMap()
        {
            Map(m => m.莱尼内部K号).Name("莱尼内部K号");
        }
    }
}