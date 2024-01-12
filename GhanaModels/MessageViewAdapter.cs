using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Refractored.Controls;

namespace GhanaModels
{
    public class MessageViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<UserMessageClickArgs> ItemClick;
        List<UserAccountModel> UserMessage;
        Context activity;

        public MessageViewAdapter(List<UserAccountModel> UserMessage)
        {
            this.UserMessage = UserMessage;
        }

        public override int ItemCount => UserMessage.Count;

        void OnClick(UserMessageClickArgs args) => ItemClick?.Invoke(this, args);

        public class UserMessageViewHolder : RecyclerView.ViewHolder
        {
            public CircleImageView UserProfileImagge { get; set; }
            public TextView UserName { get; set; }
            public TextView MessageTime {get;set;}

            public UserMessageViewHolder(View itemView, Action<UserMessageClickArgs> itemClick) : base(itemView)
            {
                UserProfileImagge = itemView.FindViewById<CircleImageView>(Resource.Id.MessageUserProfile);
                UserName = itemView.FindViewById<TextView>(Resource.Id.ProfileUsername);
                MessageTime = itemView.FindViewById<TextView>(Resource.Id.meaasageTime);

                itemView.Click += (sender, e) => itemClick(new UserMessageClickArgs { View = itemView, Position = AdapterPosition });
            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as UserMessageViewHolder;
            //holder.TextView.Text = items[position];
            viewholder.UserName.Text = UserMessage[position].UserName;
            viewholder.MessageTime.Text = UserMessage[position].UserDateCreated;
            var ProfilePicUrl = UserMessage[position].UserProfileImages;

            if (ProfilePicUrl != null)
            {
                ImageService.Instance.LoadUrl(ProfilePicUrl).Retry(5, 200).Into(viewholder.UserProfileImagge);
                // Picasso.With(activity).Load(url).Into(UserPostImage);
            }
            else
            {
                Toast.MakeText(activity, "Something went wrong, Please Try Again" , ToastLength.Short).Show();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.UserChatList, parent, false);

            var viewsholders = new UserMessageViewHolder(view, OnClick);

            return viewsholders;
        }
    }
    public class UserMessageClickArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
