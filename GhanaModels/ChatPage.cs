using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using FFImageLoading;
using Firebase.Auth;
using Firebase.Database;
using Java.Util;
using Refractored.Controls;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;

namespace GhanaModels
{
    [Activity(Label = "ChatPage", Theme = "@style/AppTheme.NoActionBar")]
    public class ChatPage : AppCompatActivity,IOnCompleteListener
    {
        CircleImageView UserProfileImage;
        FloatingActionButton UserSendBut;
        TextView UserNmae, UserDate;
        EditText userMessage;
        RecyclerView recyclermessages;
        public string RecieverUserId { get; private set; }
        FirebaseAuth auth;
        MessageAdapter messageAdapter;
        List<MessageModel> MessageList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ChatPage);
            V7Toolbar toolbar = FindViewById<V7Toolbar>(Resource.Id.Chattoolbar);
            SetSupportActionBar(toolbar);
            if (SupportActionBar != null)
            {
                SupportActionBar.SetDefaultDisplayHomeAsUpEnabled(true);
            }
            ReadMsgs();

            UserProfileImage = FindViewById<CircleImageView>(Resource.Id.MessageUserProfile);
            UserNmae = FindViewById<TextView>(Resource.Id.ProfileUsername);
            UserDate = FindViewById<TextView>(Resource.Id.meaasageTime);

            RecieverUserId = Intent.GetStringExtra("RecieverUserId");
            UserNmae.Text = Intent.GetStringExtra("UserName");
            UserDate.Text = Intent.GetStringExtra("DateCreated");
            var UserImageUrl = Intent.GetStringExtra("ProfileImages");
            if(UserImageUrl != null)
            {
                ImageService.Instance.LoadUrl(UserImageUrl).Retry(5, 200).Into(UserProfileImage);
            }

            recyclermessages = FindViewById<RecyclerView>(Resource.Id.chatRecyclerView);
            
            UserSendBut = FindViewById<FloatingActionButton>(Resource.Id.UserSendFab);
            UserSendBut.Click += delegate {
                if (string.IsNullOrEmpty(userMessage.Text))
                {

                }
                else
                {
                    SendMessage();
                }
            };
            userMessage = FindViewById<EditText>(Resource.Id.userMessage);
        }
        public void SendMessage()
        {
            FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
            string UserMsg = userMessage.Text;
            string UserIdInsert = user.Uid;

            HashMap SendUserMsg = new HashMap();
            SendUserMsg.Put("Sender", UserIdInsert);
            SendUserMsg.Put("Reciever", RecieverUserId);
            SendUserMsg.Put("Message", UserMsg);
            SendUserMsg.Put("MsgDateTime", DateTime.Now.ToString());


            var UserDataBase = AppDataHelper.GetDatabase().GetReference("Chats").Push();
            UserDataBase.SetValue(SendUserMsg).AddOnCompleteListener(this, this);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                userMessage.Text = string.Empty;
                return;
            }
            else
            {
                return;
            }
        }
        public void ShowMsgsRecycler()
        {
            recyclermessages.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(recyclermessages.Context));
            recyclermessages.HasFixedSize = true ;
        }
        private void ReadMsgs()
        {
            //MessageList = new List<MessageModel>();
            //DatabaseReference msgReference = AppDataHelper.GetDatabase().GetReference("Chats");
            //msgReference.AddValueEventListener(new ValueEventListener() );
        }
    }

    //public class ValueEventListener : IValueEventListener
    //{
    //    IntPtr IJavaObject.Handle => throw new NotImplementedException();
    //    List<MessageModel> MessageList;
    //    FirebaseAuth auth;
    //    void IDisposable.Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    void IValueEventListener.OnCancelled(DatabaseError error)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    void IValueEventListener.OnDataChange(DataSnapshot snapshot)
    //    {
    //        MessageList.Clear();
    //        if (snapshot.Value != null)
    //        {
    //            var child = snapshot.Children.ToEnumerable<DataSnapshot>();
    //            MessageList.Clear();
    //            foreach(DataSnapshot UserModels in child)
    //            {
    //                MessageModel chats = new MessageModel();
    //                if (chats.MsgSenderId.Equals(auth.Uid) && chats.MsgRecieverId.Equals(RecieverUserId))
    //                {
    //                    chats.MessageId = UserModels.Child("").Value.ToString();
    //                    chats.MsgRecieverId = UserModels.Child("").Value.ToString();
    //                    chats.MsgSenderId = UserModels.Child("").Value.ToString();
    //                    chats.UsersMessages = UserModels.Child("").Value.ToString();
    //                    chats.MsgDate = UserModels.Child("").Value.ToString();

    //                    MessageList.Add(chats);
    //                }

    //            }
    //        }
    //    }
    //}
}
