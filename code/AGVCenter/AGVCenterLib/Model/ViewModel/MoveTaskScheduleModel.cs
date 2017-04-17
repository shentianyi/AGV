using AGVCenterLib.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.ViewModel
{
    [DataContract]
    public class MoveTaskScheduleModel : INotifyPropertyChanged
    {
        public MoveTaskScheduleModel()
        {
            this.no = 1;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }


        private int no; 
        [DataMember]
        public int No
        {
            get { return this.no; }
            set
            {
                this.no = value;
                OnPropertyChanged(new PropertyChangedEventArgs("No"));
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


        private DateTime startTime;
        [DataMember]
        public DateTime StartTime
        {
            get { return this.startTime; }
            set
            {
                this.startTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTime"));
            }
        }

        private string startTimeStr;
        [DataMember]
        public string StartTimeStr
        {
            get
            {
                return this.startTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                startTimeStr = value;
            }
        }

        private string endTimeStr;
        [DataMember]
        public string EndTimeStr
        {
            get
            {
                return this.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                endTimeStr = value;
            }
        }


        private DateTime endTime;
        [DataMember]
        public DateTime EndTime
        {
            get { return this.endTime; }
            set
            {
                this.endTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTime"));
            }
        }

        private DateTime createdAt;
        [DataMember]
        public DateTime CreatedAt
        {
            get { return this.createdAt; }
            set
            {
                this.createdAt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreatedAt"));
            }
        }
        private string createUserNr;
        [DataMember]
        public string CreateUserNr
        {
            get { return this.createUserNr; }
            set
            {
                this.createUserNr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreateUserNr"));
            }
        }



        public static List<MoveTaskScheduleModel> Converts(List<MoveTaskSchedule> items)
        {
            List<MoveTaskScheduleModel> itemModels = new List<MoveTaskScheduleModel>();
            var no = 1;
            foreach (var i in items)
            {
                var item = Convert(i);
                item.no = no;
                itemModels.Add(item);
                no++;
            }
            return itemModels;
        }

        public static MoveTaskScheduleModel Convert(MoveTaskSchedule item)
        {
            if (item == null) return null;
            MoveTaskScheduleModel itemModel = new MoveTaskScheduleModel();
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
