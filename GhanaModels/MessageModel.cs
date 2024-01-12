using System;
namespace GhanaModels
{
    public class MessageModel
    {
        public string MessageId { get; set; }
        public string UsersMessages { get; set; }
        public string MsgRecieverId { get; set; }
        public string MsgSenderId { get; set; }
        public string MsgDate { get; set; }

        public MessageModel(string UsersMessages, string MsgRecieverId, string MsgSenderId)
        {
            this.UsersMessages = UsersMessages;
            this.MsgRecieverId = MsgRecieverId;
            this.MsgSenderId = MsgSenderId;
        }
        public MessageModel()
        {

        }
    }
}
