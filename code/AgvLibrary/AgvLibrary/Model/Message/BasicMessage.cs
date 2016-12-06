using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Model.Message
{
    public class BasicMessage
    {
        private bool result;
        private List<string> msgContent;
        private string msgText;


        public BasicMessage()
        {
            result = true;
        }

        public bool Result
        {
            get { return result; }
            set { result = value; }
        }

        public List<string> MsgContent
        {
            get
            {
                if (msgContent == null)
                    msgContent = new List<string>();

                return msgContent;
            }
            set
            {
                if (value != null)
                    msgContent = value;
            }

        }

        public string MsgText
        {
            get
            {
                foreach (string msg in this.MsgContent)
                {
                    msgText += msg + "；";
                }
                return msgText.TrimEnd('；');
            }
        }
    }
}
