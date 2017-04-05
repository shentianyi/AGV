using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgvWarehouseWeb.Models
{
    public class PositionImportModel
    {
        public string PositionNr { get; set; }
        public int InStorePriority { get; set; }
    }

    public sealed class PositionImportModelCsvMap : CsvClassMap<PositionImportModel>
    {
        public PositionImportModelCsvMap()
        {
            Map(m => m.PositionNr).Name("PositionNr");
            Map(m => m.InStorePriority).Name("InStorePriority");
        }
    }
}