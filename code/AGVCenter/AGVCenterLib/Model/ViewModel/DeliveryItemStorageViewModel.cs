using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AGVCenterLib.Data;

namespace AGVCenterLib.Model.ViewModel
{
    [DataContract]
    public class DeliveryItemStorageViewModel
    {
        [DataMember]
    public int Id { get; set; }

    [DataMember]
        public string DeliveryNr { get; set; }



        [DataMember]
        public string UniqItemNr { get; set; }


        [DataMember]
        public int? State { get; set; }

        [DataMember]
        public DateTime? CreatedAt { get; set; }

        [DataMember]
        public DateTime? UpdatedAt { get; set; }

         
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


        [DataMember]
        public int? TrayItemId { get; set; }

        [DataMember]
        public string TrayItemUniqItemNr { get; set; }

        [DataMember]
        public string TrayItemTrayNr { get; set; }

        [DataMember]
        public int? TrayItemState { get; set; }

       
        [DataMember]
        public DateTime? TrayItemCreatedAt { get; set; }

        [DataMember]
        public DateTime? TrayItemUpdatedAt { get; set; }


        public static List<DeliveryItemStorageViewModel> Converts(List<DeliveryItemStorageView> storageViews)
        {
            List<DeliveryItemStorageViewModel> itemModels = new List<DeliveryItemStorageViewModel>();
            foreach (var i in storageViews)
            {
                itemModels.Add(Convert(i));
            }
            return itemModels;
        }

        public static DeliveryItemStorageViewModel Convert(DeliveryItemStorageView storageView)
        {
            if (storageView == null) return null;
            DeliveryItemStorageViewModel item = new DeliveryItemStorageViewModel();
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
