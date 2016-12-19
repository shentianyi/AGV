using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AGVCenterLib.Data;

namespace AGVCenterLib.Model.ViewModel
{

    [DataContract]
    public class DeliveryModel
    {
        [DataMember]
        public int Nr { get; set; }

        [DataMember]
        public int? State { get; set; }

        [DataMember]
        public DateTime? CreatedAt { get; set; }

        [DataMember]
        public DateTime? UpdatedAt { get; set; }


        public static List<DeliveryModel> Converts(List<Delivery> items)
        {
            List<DeliveryModel> itemModels = new List<DeliveryModel>();
            foreach (var i in items)
            {
                itemModels.Add(Convert(i));
            }
            return itemModels;
        }

        public static DeliveryModel Convert(Delivery item)
        {
            if (item == null) return null;
            DeliveryModel itemModel = new DeliveryModel();
            var ps = item.GetType().GetProperties();
            Type t = itemModel.GetType();
            foreach (var p in ps)
            {

                bool hasP = t.GetProperty(p.Name) != null;
                if (hasP)
                {
                    if (t.GetProperty(p.Name).CanWrite)
                    {
                        t.GetProperty(p.Name).SetValue(itemModel,
                            item.GetType().GetProperty(p.Name).GetValue(item, null),
                            null);
                    }
                }
            }
            return itemModel;
        }

    }
}