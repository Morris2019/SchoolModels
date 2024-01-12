using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;

namespace GhanaModels
{
    public class HomeFragmentRecycler: RecyclerView.Adapter
    {
        public event EventHandler<HomeEventClickArga> ItemClick;
        List<UserAccountModel> UserProfile;
        Context activity;
        public string UserImageUrl { get; set; }

        public HomeFragmentRecycler(List<UserAccountModel> UserProfile)
        {
            this.UserProfile = UserProfile;
        }

        public override int ItemCount => UserProfile.Count;

        void OnClick (HomeEventClickArga args) => ItemClick?.Invoke(this, args);

        public class HomeViewHolder : RecyclerView.ViewHolder
        {
            public TextView UserFirstname { get; set; }
            public TextView UserLastname { get; set; }
            public TextView UserName { get; set; }
            public TextView UserAge { get; set; }
            public TextView UserComplexion { get; set; }
            public ImageView UserProfile { get; set; }

            public HomeViewHolder(View itemView, Action<HomeEventClickArga> ItemClick) : base(itemView)
            {

                UserProfile = itemView.FindViewById<ImageView>(Resource.Id.UserProfileImage);
                UserFirstname = itemView.FindViewById<TextView>(Resource.Id.homefirstname);
                UserLastname = itemView.FindViewById<TextView>(Resource.Id.homelastname);
                UserName = itemView.FindViewById<TextView>(Resource.Id.homeusername);
                UserAge = itemView.FindViewById<TextView>(Resource.Id.homeage);
                UserComplexion = itemView.FindViewById<TextView>(Resource.Id.homecomplex);

                itemView.Click += (sender, e) => ItemClick(new HomeEventClickArga { View = itemView, Position = AdapterPosition });

            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as HomeViewHolder;
            viewholder.UserFirstname.Text = UserProfile[position].UserFirstName;
            viewholder.UserLastname.Text = UserProfile[position].UserLastName;
            viewholder.UserName.Text = UserProfile[position].UserName;
            viewholder.UserAge.Text = UserProfile[position].UserAge;
            viewholder.UserComplexion.Text = UserProfile[position].UserComplexion;

            UserImageUrl = UserProfile[position].UserProfileImages;

            if (UserImageUrl != null)
            {
                ImageService.Instance.LoadUrl(UserImageUrl).Retry(5, 200).Into(viewholder.UserProfile);

            }
            else
            {
                Toast.MakeText(activity, "Something went wrong, Please Try Again" , ToastLength.Short).Show();
            }
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HomeCardView, parent, false);

            var viewsholders = new HomeViewHolder(view, OnClick);

            return viewsholders;
        }
    }
    public class HomeEventClickArga : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
