using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using V7toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Util;
using Android.Views;
using Android.Widget;


namespace GhanaModels
{
    public class MessageFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView MessageRecyclerView;
        List<UserAccountModel> chatlist;
        private UserAccountModel usersData;
        UserContactListeners userContactListeners;
        public string RecieverUserId { get; private set; }
        public string Username { get; private set; }
        public string lastSeen { get; private set; }
        public string UserImageUrl { get; private set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static MessageFragment NewInstance()
        {
            var message = new MessageFragment { Arguments = new Bundle() };
            return message;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.MessageFragment, container, false);
            MessageRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.MessageRecyclerView);
            V7toolbar toolbar = view.FindViewById<V7toolbar>(Resource.Id.ProfileTollbar);

            RetrievedChat();

            return view;
        }
        private void messageRecycler()
        {
            MessageRecyclerView.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(MessageRecyclerView.Context));
            MessageViewAdapter messageadapter = new MessageViewAdapter(chatlist);
            messageadapter.ItemClick += UserContactClick;
            MessageRecyclerView.SetAdapter(messageadapter);
        }

        private void UserContactClick(object sender, UserMessageClickArgs e)
        {
            UserAccountModel userchat = chatlist[e.Position];
            usersData = userchat;
            RecieverUserId = userchat.UserId;
            Username = userchat.UserName;
            lastSeen = userchat.UserDateCreated;
            UserImageUrl = userchat.UserProfileImages;

            var intent = new Intent(Activity, typeof(ChatPage));
            intent.PutExtra("RecieverUserId", RecieverUserId);
            intent.PutExtra("ProfileImages", UserImageUrl);
            intent.PutExtra("UserName", Username);
            intent.PutExtra("DateCreated", lastSeen);

            this.StartActivity(intent);
        }

        public void RetrievedChat()
        {
            userContactListeners = new UserContactListeners();
            userContactListeners.CreateChat();
            userContactListeners.UserChatRetrieve += userContactListeners_userRetrieved;
        }

        private void userContactListeners_userRetrieved(object sender, UserContactListeners.UserChatEventArgs e)
        {
            chatlist = e.UserAccountModel;
            messageRecycler();
        }
    }
}