using System;
using System.Collections.Generic;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Refractored.Controls;

namespace GhanaModels
{
    public class EditRecyclerView: RecyclerView.Adapter
    {
        List<UserAccountModel> EditProfile;
        public event EventHandler<EditProfileClickArgs> ItemLongClick;
        public string url { get; set; }

        public EditRecyclerView(List<UserAccountModel> EditProfile)
        {
            this.EditProfile = EditProfile;
        }

        public override int ItemCount => EditProfile.Count;

        void OnLongClick(EditProfileClickArgs args) => ItemLongClick?.Invoke(this, args);

        public  class EditRecyclerViewAdapter : RecyclerView.ViewHolder
        {
            public EditText EditFirst_name { get; set; }
            public EditText Editlast_name { get; set; }
            public EditText EditUserName { get; set; }
            public EditText Editphone { get; set; }
            public EditText EditUserBioggraphy { get; set; }
            public CircleImageView EditProileImage { get; set; }
            public Button UserUpdateButt { get; set; }

            public EditRecyclerViewAdapter(View itemView, Action<EditProfileClickArgs> ItemLongClick): base(itemView)
            {
                EditFirst_name = itemView.FindViewById<EditText>(Resource.Id.EditFirst_name);
                Editlast_name = itemView.FindViewById<EditText>(Resource.Id.Editlast_name);
                EditUserName = itemView.FindViewById<EditText>(Resource.Id.EditUserName);
                Editphone = itemView.FindViewById<EditText>(Resource.Id.Editphone);
                EditUserBioggraphy = itemView.FindViewById<EditText>(Resource.Id.EditUserBio);
                EditProileImage = itemView.FindViewById<CircleImageView>(Resource.Id.EditUserProfile);
                UserUpdateButt = itemView.FindViewById<Button>(Resource.Id.UserUpdate);
               

                itemView.LongClick += (sender, e) => ItemLongClick(new EditProfileClickArgs { View = itemView, Position = AdapterPosition });

            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as EditRecyclerViewAdapter;
            //holder.TextView.Text = items[position];
            viewholder.EditFirst_name.Text = EditProfile[position].UserFirstName;
            viewholder.Editlast_name.Text = EditProfile[position].UserLastName;
            viewholder.EditUserName.Text = EditProfile[position].UserName;
            viewholder.Editphone.Text = EditProfile[position].UserContact;
            viewholder.EditUserBioggraphy.Text = EditProfile[position].UserBiography;
            url = EditProfile[position].UserProfileImages;

            if (url != null)
            {
                ImageService.Instance.LoadUrl(url).Retry(5, 200).Into(viewholder.EditProileImage);
                // Picasso.With(activity).Load(url).Into(UserPostImage);
            }
           
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EditUserProfile, parent, false);

            var viewsholders = new EditRecyclerViewAdapter(view, OnLongClick);

            return viewsholders;
        }
    }
    public class EditProfileClickArgs :EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
