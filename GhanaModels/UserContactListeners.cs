using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;
using Firebase.Database;

namespace GhanaModels
{
    public class UserContactListeners : Java.Lang.Object, IValueEventListener
    {
        List<UserAccountModel> chatlist = new List<UserAccountModel>();
        public string UserItemId { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; private set; }
        FirebaseAuth auth;

        public event EventHandler<UserChatEventArgs> UserChatRetrieve;

        public class UserChatEventArgs : EventArgs
        {
            public List<UserAccountModel> UserAccountModel { get; set; }
        }

        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.GetValue(true) != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                //FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
                //UserId = user.Uid;

                chatlist.Clear();
                foreach (DataSnapshot ChatData in child)
                {
                    UserAccountModel userAccountModel = new UserAccountModel
                    {
                        UserId = ChatData.Key,
                        UserFirstName = ChatData.Child("FirstName").Value.ToString(),
                        UserLastName = ChatData.Child("LastName").Value.ToString(),
                        UserName = ChatData.Child("UserName").Value.ToString(),
                        UserAge = ChatData.Child("Age").Value.ToString(),
                        UserContact = ChatData.Child("UserContact").Value.ToString(),
                        UserProfileImages = ChatData.Child("ProfileImages").Value.ToString(),
                        UserCountry = ChatData.Child("UserCountry").Value.ToString(),
                        UserEmail = ChatData.Child("UserEmail").Value.ToString(),
                        UserBiography = ChatData.Child("Biography").Value.ToString(),
                        UserFeetSize = ChatData.Child("Feet_Size").Value.ToString(),
                        UserHeight = ChatData.Child("Height").Value.ToString(),
                        UserWeight = ChatData.Child("Weight").Value.ToString(),
                        UserComplexion = ChatData.Child("Complexion").Value.ToString(),
                        UserDateCreated = ChatData.Child("DateCreated").Value.ToString()
                   };

                    chatlist.Add(userAccountModel);

                }
                UserChatRetrieve.Invoke(this, new UserChatEventArgs { UserAccountModel = chatlist });
            }
        }
        public void Create()
        {
                DatabaseReference ChatListRef = AppDataHelper.GetDatabase().GetReference("Users");
                //Query sortUserData = ChatListRef.OrderByKey().EqualTo(auth.CurrentUser.Uid
                //    .ToString());
            ChatListRef.AddValueEventListener(this);
          
        }
        public void SingleCreate()
        {
            //FirebaseAuth userauth;
            string userID = FirebaseAuth.Instance.CurrentUser.Uid;

            DatabaseReference ChatListRef = AppDataHelper.GetDatabase().GetReference("Users");
            Query sortUserData = ChatListRef.OrderByKey().EqualTo(FirebaseAuth.Instance.CurrentUser.Uid.ToString());
            sortUserData.AddValueEventListener(this);

        }
        public void SearchCreate()
        { 
            DatabaseReference ChatListRef = AppDataHelper.GetDatabase().GetReference("Users");
            Query sortUserData = ChatListRef.OrderByChild("UserId");
            sortUserData.AddValueEventListener(this);
        }
        public void CreateChat()
        {
            DatabaseReference ChatListRef = AppDataHelper.GetDatabase().GetReference("Users");
            ChatListRef.AddValueEventListener(this);
        }
        public void UserAccountEditCreate()
        {
            DatabaseReference ChatListRef = AppDataHelper.GetDatabase().GetReference("Users/" + UserId);
            Query editQuerry = ChatListRef.OrderByKey().EqualTo(FirebaseAuth.Instance.CurrentUser.Uid.ToString());
            editQuerry.AddValueEventListener(this);
            
        }
    }
}
