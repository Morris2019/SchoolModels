using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;
using Firebase.Database;

namespace GhanaModels
{
    public class ModelsListener : Java.Lang.Object, IValueEventListener
    {
        List<UsePostModel> ModelPosts = new List<UsePostModel>();
        public string UserId { get; private set; }
        FirebaseAuth auth;

        public event EventHandler<ModelsPostEvent> ModelsPostRetrieve;

        public class ModelsPostEvent : EventArgs
        {
            public List<UsePostModel> usePostModels { get; set; }
        }
        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Value != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                ModelPosts.Clear();
                foreach (DataSnapshot UserModels in child)
                {
                    UsePostModel usePostModel = new UsePostModel();

                    usePostModel.PostId = UserModels.Key;
                    usePostModel.User_Id = UserModels.Child("UserId").Value.ToString();
                    usePostModel.PostCaption = UserModels.Child("PostCaption").Value.ToString();
                    usePostModel.DateCreated = UserModels.Child("DateCreated").Value.ToString();
                    usePostModel.PostImage = UserModels.Child("PostImage").Value.ToString();


                    ModelPosts.Add(usePostModel);
                }
                 
                    ModelsPostRetrieve.Invoke(this, new ModelsPostEvent { usePostModels = ModelPosts });
         
            }
            else
            {
                return;
            }
        }

        public void ModelsPostDataCreate()
        {
            FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
            if (user != null)
            {
                UserId = user.Uid;
                DatabaseReference profileref = AppDataHelper.GetDatabase().GetReference("UsersPosts");
                Query dataOrderBy = profileref.OrderByChild("UserId").EqualTo(FirebaseAuth.Instance.CurrentUser.Uid.ToString());
                dataOrderBy.AddValueEventListener(this);

            }
        }
        public void DeleteUserPic(string key)
        {
            FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
            if (user != null)
            {
                UserId = user.Uid;
                DatabaseReference profileref = AppDataHelper.GetDatabase().GetReference("UsersPosts/" + key);
                profileref.RemoveValue();

            }
        }
    }
}
