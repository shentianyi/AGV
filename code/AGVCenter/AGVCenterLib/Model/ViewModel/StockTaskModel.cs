using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AGVCenterLib.Data;

namespace AGVCenterLib.Model.ViewModel
{

    [DataContract]
    public class StockTaskModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? RoadMachineIndex { get; set; }
        [DataMember]
        public int? BoxType { get; set; }

        [DataMember]
        public string BoxTypeStr { get; set; }
        [DataMember]
        public string PositionNr { get; set; }
        [DataMember]
        public int? PositionFloor { get; set; }
        [DataMember]
        public int? PositionColumn { get; set; }
        [DataMember]
        public int? PositionRow { get; set; }

        [DataMember]
        public int? AgvPassFlag { get; set; }
        [DataMember]
        public int? RestPositionFlag { get; set; }

        [DataMember]
        public string BarCode { get; set; }
        [DataMember]
        public int? State { get; set; }
        [DataMember]
        public int? Type { get; set; }
        [DataMember]
        public int? TrayReverseNo { get; set; }


        [DataMember]
        public int? TrayNum { get; set; }
        [DataMember]
        public int? DeliveryItemNum { get; set; }
        [DataMember]
        public string DeliveryBatchId { get; set; }
        [DataMember]
        public string TrayBatchId { get; set; }
        [DataMember]
        public DateTime? CreatedAt { get; set; }
        [DataMember]
        public DateTime? UpdatedAt { get; set; }
         

        [DataMember]
        public string StateStr { get; set; }

        public static List<StockTaskModel> Converts(List<StockTask> stockTasks)
        {
            List<StockTaskModel> itemModels = new List<StockTaskModel>();
            foreach (var i in stockTasks)
            {
                itemModels.Add(Convert(i));
            }
            return itemModels;
        }

        public static StockTaskModel Convert(StockTask stockTask)
        {
            if (stockTask == null) return null;
            StockTaskModel item = new StockTaskModel();
            var ps = stockTask.GetType().GetProperties();
            Type t = item.GetType();
            foreach (var p in ps)
            {

                bool hasP = t.GetProperty(p.Name) != null;
                if (hasP)
                {
                    if (t.GetProperty(p.Name).CanWrite)
                    {
                        t.GetProperty(p.Name).SetValue(item,
                            stockTask.GetType().GetProperty(p.Name).GetValue(stockTask, null),
                            null);
                    }
                }
            }
            return item;
        }

    }
}