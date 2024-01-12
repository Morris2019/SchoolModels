using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase.Auth;

namespace GhanaModels
{
    public class MessageAdapter : RecyclerView.Adapter
    {
        List<MessageModel> MessageList;
        public event EventHandler<UserMessagesClickArgs> ItemLongClick;
        public static int MessageLeft = 0;
        public static int MessageRight = 1;

        public MessageAdapter(List<MessageModel> MessageList)
        {
          this.MessageList = MessageList;
        }

        public override int ItemCount => MessageList.Count;

        void OnLongClick(UserMessagesClickArgs args) => ItemLongClick?.Invoke(this, args);

        public class MessageViewAdapter : RecyclerView.ViewHolder
        {
            public TextView SenderMessage { get; set; }

            public MessageViewAdapter(View itemView, Action<UserMessagesClickArgs> ItemLongClick) : base(itemView)
            {
                SenderMessage = itemView.FindViewById<TextView>(Resource.Id.Messages);

                itemView.LongClick += (sender, e) => ItemLongClick(new UserMessagesClickArgs { View = itemView, Position = AdapterPosition });
            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as MessageViewAdapter;
            //holder.TextView.Text = items[position];
            viewholder.SenderMessage.Text = MessageList[position].UsersMessages;
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if(viewType == MessageLeft)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ChatItemLeft, parent, false);

                var viewsholders = new MessageViewAdapter(view, OnLongClick);

                return viewsholders;
            }
            else
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ChatItemRight, parent, false);

                var viewsholders = new MessageViewAdapter(view, OnLongClick);

                return viewsholders;
            }
            
        }
        //public int GetItemViewType(int Position)
        //{
        //    FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
        //    if (MessageList.GetType(Position).)
        //}
    }
    public class UserMessagesClickArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }

}
