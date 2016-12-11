using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AGVCenterLib.Data;

namespace AGVCenterLib.Model.ViewModel
{
    [DataContract]
    public class DeliveryStorageViewModel
    {
        [DataMember]
        public string Nr { get; set; }

        [DataMember]
        public int? State { get; set; }

        [DataMember]
        public DateTime? CreatedAt { get; set; }

        [DataMember]
        public DateTime? UpdatedAt { get; set; }

        [DataMember]
        public int DeliveryItemId { get; set; }

        [DataMember]
        public string DeliveryItemDeliveryNr { get; set; }

        [DataMember]
        public string DeliveryItemUniqItemNr { get; set; }

        [DataMember]
        public DateTime? DeliveryItemCreatedAt { get; set; }

        [DataMember]
        public DateTime? DeliveryItemUpdatedAt { get; set; }

        [DataMember]
        public string UniqueItemNr { get; set; }

        [DataMember]
        public int? UniqueItemBoxTypeId { get; set; }

        [DataMember]
        public string UniqueItemPartNr { get; set; }

        [DataMember]
        public string UniqueItemKNr { get; set; }

        [DataMember]
        public string UniqueItemKNrWithYear { get; set; }

        [DataMember]
        public string UniqueItemCheckCode { get; set; }

        [DataMember]
        public string UniqueItemKskNr { get; set; }
          
        [DataMember]
        public string UniqueItemQR { get; set; }

        [DataMember]
        public int? UniqueItemState { get; set; }

        [DataMember]
        public DateTime? UniqueItemCreatedAt { get; set; }

        [DataMember]
        public DateTime? UniqueItemUpdatedAt { get; set; }

        [DataMember]
        public int? StorageId { get; set; }

        [DataMember]
        public string StoragePositionNr { get; set; }

        [DataMember]
        public string StoragePartNr { get; set; }

        [DataMember]
        public DateTime? StorageFIFO { get; set; }

        [DataMember]
        public string StorageUniqItemNr { get; set; }

        [DataMember]
        public DateTime? StorageCreatedAt { get; set; }

        [DataMember]
        public DateTime? StorageUpdatedAt { get; set; }

        private bool isInStock = false;
        [DataMember]
        public bool IsInStock
        {
            get { return this.isInStock; }
            set { isInStock = value; }
        }

        public static List<DeliveryStorageViewModel> Converts(List<DeliveryStorageView> storageViews)
        {
            List<DeliveryStorageViewModel> itemModels = new List<DeliveryStorageViewModel>();
            foreach (var i in storageViews)
            {
                itemModels.Add(Convert(i));
            }
            return itemModels;
        }

        public static DeliveryStorageViewModel Convert(DeliveryStorageView storageView)
        {
            if (storageView == null) return null;
            DeliveryStorageViewModel item = new DeliveryStorageViewModel();
            var ps = storageView.GetType().GetProperties();
            Type t = item.GetType();
            foreach (var p in ps)
            {
               
                bool hasP = t.GetProperty(p.Name) != null;
                if (hasP) {
                    if (t.GetProperty(p.Name).CanWrite)
                    {
                        t.GetProperty(p.Name).SetValue(item,
                            storageView.GetType().GetProperty(p.Name).GetValue(storageView, null),
                            null);
                    }
                }
            }
            return item;
        }
    }
}
