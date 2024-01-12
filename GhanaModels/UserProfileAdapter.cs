using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;

namespace GhanaModels
{
    public class UserProfileAdapter : RecyclerView.Adapter
    {
        List<UserAccountModel> UserMessage;
        Context activity;

        public UserProfileAdapter(List<UserAccountModel> UserMessage)
        {
            this.UserMessage = UserMessage;
        }
        public override int ItemCount => UserMessage.Count;

        public class UserProfileAdapterViewHolder : RecyclerView.ViewHolder
        {
            public TextView userfirstname { get; set; }
            public TextView userlastname { get; set; }
            public TextView username { get; set; }
            public TextView userBigraphy { get; set; }
            public ImageView userprofileimage { get; set; }


            public UserProfileAdapterViewHolder(View itemView) : base(itemView)
            {
                userfirstname = itemView.FindViewById<TextView>(Resource.Id.UserFirstName);
                userlastname = itemView.FindViewById<TextView>(Resource.Id.UserLastName);
                username = itemView.FindViewById<TextView>(Resource.Id.UserName);
                userBigraphy = itemView.FindViewById<TextView>(Resource.Id.UserBiography);
                userprofileimage = itemView.FindViewById<ImageView>(Resource.Id.UserProfileImage);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as UserProfileAdapterViewHolder;
            //holder.TextView.Text = items[position];
            viewholder.userfirstname.Text = UserMessage[position].UserFirstName;
            viewholder.userlastname.Text = UserMessage[position].UserLastName;
            viewholder.username.Text = UserMessage[position].UserName;
            viewholder.userBigraphy.Text = UserMessage[position].UserBiography;

            var ProfilePicUrl = UserMessage[position].UserProfileImages;

            if (ProfilePicUrl != null)
            {
                ImageService.Instance.LoadUrl(ProfilePicUrl).Retry(5, 200).Into(viewholder.userprofileimage);
                // Picasso.With(activity).Load(url).Into(UserPostImage);
            }
            else
            {
                Toast.MakeText(activity, "Something went wrong, Please Try Again", ToastLength.Short).Show();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.UserProfiileCard, parent, false);

            var viewsholders = new UserProfileAdapterViewHolder(view);

            return viewsholders;
        }
    }
}
