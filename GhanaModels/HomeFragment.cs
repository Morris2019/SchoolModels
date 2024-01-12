
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GhanaModels
{
    public class HomeFragment : Android.Support.V4.App.Fragment
    {
        public string UserId { get; set; }
        public string Username { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string UserAge { get; set; }
        public string Biography { get; private set; }
        public string UserContact { get; private set; }
        public string UserImageUrl { get; private set; }
        public string FootSize { get; private set; }
        public string Weight { get; private set; }
        public string Complexion { get; private set; }
        public string Height { get; set; }
        public string UserEmail { get; set; }

        RecyclerView UsersProfile;
        List<UserAccountModel> profilelist;
        private UserAccountModel Profiledata;
        UserContactListeners userContactListeners;
        HomeFragmentRecycler homeviewadapter;

        //RecyclerView MessageRecyclerView;
        public string RecieverUserId { get; private set; }
        public string lastSeen { get; private set; }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static HomeFragment NewInstance()
        {
            var homefrag = new HomeFragment { Arguments = new Bundle() };
            return homefrag;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.HomeFragment, container, false);

            UsersProfile = view.FindViewById<RecyclerView>(Resource.Id.HomeRecyclerView);
            //MessageRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.MessageRecyclerView);

            //  ModelPostRetrieve();
            RetrievedChat();
            return view;
        }
        private void messageRecycler()
        {
            UsersProfile.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(UsersProfile.Context));
            HomeFragmentRecycler messageadapter = new HomeFragmentRecycler(profilelist);
            messageadapter.ItemClick += Messageadapter_ItemClick;
            UsersProfile.SetAdapter(messageadapter);
        }

        private void Messageadapter_ItemClick(object sender, HomeEventClickArga e)
        {
            UserAccountModel profiledata = profilelist[e.Position];
            Profiledata = profiledata;
            RecieverUserId = profiledata.UserId;
            UserImageUrl = profiledata.UserProfileImages;
            Username = profiledata.UserName;
            UserAge = profiledata.UserAge;
            Firstname = profiledata.UserFirstName;
            Lastname = profiledata.UserLastName;
            UserContact = profiledata.UserContact;
            Biography = profiledata.UserBiography;
            FootSize = profiledata.UserFeetSize;
            Weight = profiledata.UserWeight;
            Complexion = profiledata.UserComplexion;
            Height = profiledata.UserHeight;
            UserEmail = profiledata.UserEmail;

            var intent = new Intent(Activity, typeof(ProfileviewPagge));
            intent.PutExtra("UserProfileImages", UserImageUrl);
            intent.PutExtra("UserName", Username);
            intent.PutExtra("Firstname", Firstname);
            intent.PutExtra("Lastname", Lastname);
            intent.PutExtra("UserAge", UserAge);
            intent.PutExtra("UserContact", UserContact);
            intent.PutExtra("Biography", Biography);
            intent.PutExtra("FootSize", FootSize);
            intent.PutExtra("Weight", Weight);
            intent.PutExtra("Complexion", Complexion);
            intent.PutExtra("Height", Height);
            intent.PutExtra("UserEmail", UserEmail);

            this.StartActivity(intent);

        }

        public void RetrievedChat()
        {
            userContactListeners = new UserContactListeners();
            userContactListeners.SearchCreate();
            userContactListeners.UserChatRetrieve += userContactListeners_userRetrieved;
        }

        private void userContactListeners_userRetrieved(object sender, UserContactListeners.UserChatEventArgs e)
        {
            profilelist = e.UserAccountModel;
            messageRecycler();
        }
       
    }
}