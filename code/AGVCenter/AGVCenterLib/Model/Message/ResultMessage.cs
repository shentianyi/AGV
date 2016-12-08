using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.Message
{
    public enum MessageType
    {
        [Description("正确")]
        OK = 0,
        [Description("提醒")]
        Warn = 1,
        [Description("异常")]
        Exception = 2
    }

    [DataContract]
    public class ResultMessage
    {
        private MessageType messageType;
        private bool success;
        private string content;

        public ResultMessage()
        {
            this.messageType = MessageType.Warn;
            this.success = false;
        }

        [DataMember]
        public MessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        [DataMember]
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        [DataMember]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
}
