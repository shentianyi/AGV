using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AGVCenterLib.Data;

namespace AGVCenterLib.Model.ViewModel
{

    [DataContract]
    public class StorageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        private int id;
        [DataMember]
        public int Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }

        private string positionNr;
        [DataMember]
        public string PositionNr
        {
            get { return this.positionNr; }
            set
            {
                this.positionNr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PositionNr"));
            }
        }

        private string partNr;
        [DataMember]
        public string PartNr
        {
            get { return this.partNr; }
            set
            {
                this.partNr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PartNr"));
            }
        }

        private DateTime? fifo;
        [DataMember]
        public DateTime? FIFO
        {
            get { return this.fifo; }
            set
            {
                this.fifo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FIFO"));
            }
        }

        private string uniqItemNr;
        [DataMember]
        public string UniqItemNr
        {
            get { return this.uniqItemNr; }
            set
            {
                this.uniqItemNr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UniqItemNr"));
            }
        }


        [DataMember]
        public DateTime? CreatedAt { get; set; }

        [DataMember]
        public DateTime? UpdatedAt { get; set; }

       
        public static List<StorageModel> Converts(List<Storage> items)
        {
            List<StorageModel> itemModels = new List<StorageModel>();
            foreach (var i in items)
            {
                itemModels.Add(Convert(i));
            }
            return itemModels;
        }

        public static StorageModel Convert(Storage item)
        {
            if (item == null) return null;
            StorageModel itemModel = new StorageModel();
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