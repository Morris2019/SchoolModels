using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Refractored.Controls;

namespace GhanaModels
{
    [Activity(Label = "EditUserProfile", Theme = "@style/AppTheme.NoActionBar")]
    public class EditUserProfile : AppCompatActivity
    {
        //TextInputLayout EditFirst_name, Editlast_name, EditUserName, Editphone, EditUserBioggraphy;
        //CircleImageView EditProileImage;
        RecyclerView EditPageRecycler;
        List<UserAccountModel> chatlist;
        UserContactListeners userContactListeners;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.EditPageRecycler);


            EditPageRecycler = FindViewById<RecyclerView>(Resource.Id.EditPageRecycler);
            
            RetrievedChat();
        }
        private void EditPageRecyclerView()
        {
            EditPageRecycler.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(EditPageRecycler.Context));
            EditRecyclerView editprofileadapter = new EditRecyclerView(chatlist);
           // messageadapter.ItemClick += UserContactClick;
            EditPageRecycler.SetAdapter(editprofileadapter);
        }
        public void RetrievedChat()
        {
            userContactListeners = new UserContactListeners();
            userContactListeners.UserAccountEditCreate();
            userContactListeners.UserChatRetrieve += userContactListeners_userRetrieved;
        }

        private void userContactListeners_userRetrieved(object sender, UserContactListeners.UserChatEventArgs e)
        {
            chatlist = e.UserAccountModel;
            EditPageRecyclerView();
        }
    }
}
