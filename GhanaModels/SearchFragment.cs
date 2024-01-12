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
using Android.Support.V7.Widget;
using Android.Text;

namespace GhanaModels
{
    public class SearchFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView searchrecycler;
        EditText SearchBox;
        List<UserAccountModel> searchList;
        MessageViewAdapter adapter;
        UserContactListeners userContactListeners;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static SearchFragment NewInstance()
        {
            var searchfrag = new SearchFragment { Arguments = new Bundle() };
            return searchfrag;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.SearchFragment, container, false);
            searchrecycler = view.FindViewById<RecyclerView>(Resource.Id.searchrecycler);
            SearchBox = view.FindViewById<EditText>(Resource.Id.SearchText);
            SearchBox.TextChanged += SearchUserData;
            V7toolbar toolbar = view.FindViewById<V7toolbar>(Resource.Id.searchtoolbar);
            RetrievedChat();

            return view;
        }

        private void SearchUserData(object sender, TextChangedEventArgs e)
        {
            
            List<UserAccountModel> SearchResult = (from searchUser in searchList
                                                   where searchUser.UserFirstName.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserLastName.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserName.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserContact.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserCountry.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserEmail.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserAge.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserFeetSize.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserBiography.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserHeight.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserWeight.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.UserComplexion.ToLower().Contains(SearchBox.Text.ToLower())
                                                   select searchUser).ToList();

            adapter = new MessageViewAdapter(SearchResult);
            searchrecycler.SetAdapter(adapter);
        }
        private void messageRecycler()
        {
            searchrecycler.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(searchrecycler.Context));
            MessageViewAdapter messageadapter = new MessageViewAdapter(searchList);
            messageadapter.ItemClick += UserContactClick;
            searchrecycler.SetAdapter(messageadapter);
        }
        public void RetrievedChat()
        {
            userContactListeners = new UserContactListeners();
            userContactListeners.SearchCreate();
            userContactListeners.UserChatRetrieve += userContactListeners_userRetrieved;
        }

        private void userContactListeners_userRetrieved(object sender, UserContactListeners.UserChatEventArgs e)
        {
            searchList = e.UserAccountModel;
            messageRecycler();
        }
        private void UserContactClick(object sender, UserMessageClickArgs e)
        {

        }        
    }
}