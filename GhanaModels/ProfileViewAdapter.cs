using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Square.Picasso;

namespace GhanaModels
{
    public class ProfileViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<UserImaggesClickArgs> ItemLongClick;
        List<UsePostModel> ModelPosts;
        public string url { get; set; }
        Context activity;


        public ProfileViewAdapter(List<UsePostModel> ModelPosts)
        {
            this.ModelPosts = ModelPosts;

        }

        public override int ItemCount => ModelPosts.Count;
        void OnLongClick(UserImaggesClickArgs args) => ItemLongClick?.Invoke(this, args);


        public class ProfileViewHolder : RecyclerView.ViewHolder
        {
            public ImageView UserPostImage { get; set; }


            public ProfileViewHolder(View itemView, Action<UserImaggesClickArgs> ItemLongClick ) : base(itemView)
            {
                UserPostImage = itemView.FindViewById<ImageView>(Resource.Id.userPostitem1);

                 itemView.LongClick += (sender, e) => ItemLongClick(new UserImaggesClickArgs { View = itemView, Position = AdapterPosition});
             }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as ProfileViewHolder;
            //holder.TextView.Text = items[position];
            //viewholder.UserPostImage = ModelPosts[position].PostImage;
            url = ModelPosts[position].PostImage;

            if (url != null)
            {
                ImageService.Instance.LoadUrl(url).Retry(5, 200).Into(viewholder.UserPostImage);
               // Picasso.With(activity).Load(url).Into(UserPostImage);
            }
            else
            {
                Toast.MakeText(activity, "Something went wrong, Please Try Again" + url, ToastLength.Short).Show();
            }   
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.UserPostCardView, parent, false);

            var viewsholders = new ProfileViewHolder(view, OnLongClick);

            return viewsholders;
        }
    }
    public class UserImaggesClickArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
