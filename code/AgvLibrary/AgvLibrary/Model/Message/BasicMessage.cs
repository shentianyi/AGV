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

        

        public string MsgText
        {
            get
            {
                return msgText;
            }
            set
            {
                if (value != null)
                    MsgText = value;
            }
        }
    }
}
