using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AGVCenterLib.Data;

namespace AGVCenterLib.Model.ViewModel
{
    [DataContract]
    public class StorageUniqueItemViewModel
    {
        [DataMember]
    public int Id { get; set; }

    [DataMember]
        public string PositionNr { get; set; }



        [DataMember]
        public string PartNr { get; set; }

        [DataMember]
        public DateTime? FIFO { get; set; }


        [DataMember]
        public string UniqItemNr { get; set; }

        

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
        public bool? PositionIsLocked { get; set; }

        [DataMember]
        public int? PositionRoadMachineIndex { get; set; }

        [DataMember]
        public int? PositionState { get; set; }

        [DataMember]
        public int? PositionRow { get; set; }

        [DataMember]
        public int? PositionColumn { get; set; }

        [DataMember]
        public int? PositionFloor { get; set; }

        [DataMember]
        public string PositionWarehouseNr { get; set; }

        [DataMember]
        public string BoxTypeStr
        {
            get; set;
        }
        public static List<StorageUniqueItemViewModel> Converts(List<StorageUniqueItemView> storageViews)
        {
            List<StorageUniqueItemViewModel> itemModels = new List<StorageUniqueItemViewModel>();
            foreach (var i in storageViews)
            {
                itemModels.Add(Convert(i));
            }
            return itemModels;
        }

        public static StorageUniqueItemViewModel Convert(StorageUniqueItemView storageView)
        {
            if (storageView == null) return null;
            StorageUniqueItemViewModel item = new StorageUniqueItemViewModel();
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
