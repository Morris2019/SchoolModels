using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V7toolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Android.Support.V7.Widget;
using DialogBuilder = Android.Support.V7.App.AlertDialog.Builder;
using Firebase.Database;

namespace GhanaModels
{
    public class ProfileFragment : Android.Support.V4.App.Fragment
    {
        ModelsListener modelsListener;
        UserContactListeners userContactListeners;
        RecyclerView UserItems, RecyProfileItems;
        List<UsePostModel> ModelPosts = new List<UsePostModel>();
        List<UserAccountModel> profileList ;
        public string UserId { get; set; }
        public string Username { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Biography { get; private set; }
        public string UserContact { get; private set; }
        public string UserImageUrl { get; private set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static ProfileFragment NewInstance()
        {
            var profilefrag = new ProfileFragment { Arguments = new Bundle() };
            return profilefrag;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ProfileFragment, container, false);
            UserItems = view.FindViewById<RecyclerView>(Resource.Id.profileRecyclerView);
            RecyProfileItems = view.FindViewById<RecyclerView>(Resource.Id.UserProfileRecycler);
            V7toolbar toolbar = view.FindViewById<V7toolbar>(Resource.Id.ProfileTollbar);

            toolbar.InflateMenu(Resource.Menu.profileItems);
            toolbar.MenuItemClick += MenuItemSelected;
            UserProfileRetrive();
            ModelPostRetrieve();

            return view;
        }
        private void ProfileRecycler()
        {
            var gridLayoutManager = new GridLayoutManager(Activity, 3);

            UserItems.SetLayoutManager(gridLayoutManager);
            UserItems.HasFixedSize = true;

            ProfileViewAdapter adapter = new ProfileViewAdapter(ModelPosts);
            adapter.ItemLongClick += DeletUserImage;
            UserItems.SetAdapter(adapter);
        }
        private void Profilerecycleritem()
        {
            RecyProfileItems.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(RecyProfileItems.Context));
            UserProfileAdapter profileadater = new UserProfileAdapter(profileList);
            RecyProfileItems.SetAdapter(profileadater);
        }
        private void DeletUserImage(object sender, UserImaggesClickArgs e)
        {
            string key = ModelPosts[e.Position].PostId;
            DialogBuilder Deletedialog = new DialogBuilder(Activity);
            Deletedialog.SetTitle("Delete Image");
            Deletedialog.SetMessage("Contents Deleted can not be reovered. Click on Continue to delete");
            Deletedialog.SetPositiveButton("Continue", (deleteAlertDialog, args) =>
            {
                modelsListener.DeleteUserPic(key);
            });
            Deletedialog.SetNegativeButton("Cancel", (deleteAlertDialog, args) =>
            {
                Deletedialog.Dispose();
            });
            Deletedialog.Show();
        }

        private void MenuItemSelected(object sender, V7toolbar.MenuItemClickEventArgs e)
        {
            
            ProfileItems(e.Item.ItemId);
        }
        void ProfileItems(int id)
        {
            Intent intent;
            switch (id)
            {
                case Resource.Id.nav_signuot:
                    intent = new Intent(Activity, typeof(MainActivity));
                    FirebaseAuth.Instance.SignOut();
                    StartActivity(intent);
                    break;
                case Resource.Id.nav_edit:
                    intent = new Intent(Activity, typeof(EditUserProfile));
                    
                    StartActivity(intent);
                    break;
            }

        }
        public void UserProfileRetrive()
        {
            FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
            if(user != null)
            {
                UserId = user.Uid;
                //UserId = profileList[e.Position].UserId;

                userContactListeners = new UserContactListeners();
                userContactListeners.SingleCreate();
                userContactListeners.UserChatRetrieve += Profile_userRetrieved;
            }
            else
            {
                return;
            }
            
        }

        private void Profile_userRetrieved(object sender, UserContactListeners.UserChatEventArgs e)
        {
            profileList = e.UserAccountModel;
            Profilerecycleritem();
        }

        public void ModelPostRetrieve()
        {
            modelsListener = new ModelsListener();
            modelsListener.ModelsPostDataCreate();
            modelsListener.ModelsPostRetrieve += ModelListener_Retrieve;
        }

        private void ModelListener_Retrieve(object sender, ModelsListener.ModelsPostEvent e)
        {
            ModelPosts= e.usePostModels;
            ProfileRecycler();
        }
    }
}