using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AGVCenterLib.Data;

namespace AGVCenterLib.Model.ViewModel
{
    [DataContract]
    public class PickListStorageViewModel
    {
        [DataMember]
        public string Nr { get; set; }
         

        [DataMember]
        public DateTime? CreatedAt { get; set; }

        [DataMember]
        public DateTime? UpdatedAt { get; set; }

        [DataMember]
        public int PickListItemId { get; set; }

        [DataMember]
        public string PickListItemPickListNr { get; set; }

        [DataMember]
        public string PickListItemUniqItemNr { get; set; }

        [DataMember]
        public DateTime? PickListItemCreatedAt { get; set; }

        [DataMember]
        public DateTime? PickListItemUpdatedAt { get; set; }

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

 


        public static List<PickListStorageViewModel> Converts(List<PickListStorageView> storageViews)
        {
            List<PickListStorageViewModel> itemModels = new List<PickListStorageViewModel>();
            foreach (var i in storageViews)
            {
                itemModels.Add(Convert(i));
            }
            return itemModels;
        }

        public static PickListStorageViewModel Convert(PickListStorageView storageView)
        {
            if (storageView == null) return null;
            PickListStorageViewModel item = new PickListStorageViewModel();
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
