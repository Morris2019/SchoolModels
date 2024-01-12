using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Storage;
using Java.IO;
using Java.Util;

namespace GhanaModels
{
    [Activity(Label = "NewItemAdd", Theme = "@style/AppTheme.NoActionBar")]
    public class NewItemAdd : AppCompatActivity,IOnCompleteListener
    {
        Button AddUserPost;
        TextInputLayout UserCaption;
        ImageView UserImageSlider1;
        CoordinatorLayout postSnack;
        public int MyResultCode = 1;
        public int PICK_IMAGE_REQUSET = 1000;
        private Android.Net.Uri filePath;
        ProgressBar progressBar;

        public StorageReference storageRef;
        FirebaseAuth auth;
        FirebaseStorage storage;

        List<Android.Net.Uri> ImageList = new List<Android.Net.Uri>();
        Android.Net.Uri ImageUri;
        private int UploadCount = 0;
        public string UserId { get; private set; }
        private string DownloadUrl { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NewItemAdd);
            // Create your application here
            storageRef = FirebaseStorage.Instance.GetReference("Advert_Images/" + Guid.NewGuid().ToString());
            auth = FirebaseAuth.Instance;
            //UserImageAdd = FindViewById<ViewPager>(Resource.Id.UserImages);
            //UserImageAdd.Click += ViewPagerClicEvent;
            postSnack = FindViewById<CoordinatorLayout>(Resource.Id.postitems);
            progressBar = FindViewById<ProgressBar>(Resource.Id.UserPost);
            AddUserPost = FindViewById<Button>(Resource.Id.UserItemAdd);
            AddUserPost.Click += AddUserClick;
            UserCaption = FindViewById<TextInputLayout>(Resource.Id.UserCaptionText);
            UserImageSlider1 = FindViewById<ImageView>(Resource.Id.UserImageSlider1);
            UserImageSlider1.Click += delegate
            {
                Intent intent = new Intent(Intent.ActionGetContent);
                intent.PutExtra(Intent.ExtraAllowMultiple, true);
                intent.SetType("image/*");
                intent.SetAction(Intent.ActionPick);
                StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), PICK_IMAGE_REQUSET);
            };
        }

        private async void AddUserClick(object sender, EventArgs e)
        {
            string UserPostImages = (string)await storageRef.GetDownloadUrlAsync();
            FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
            if (user != null)
            {

                UserId = user.Uid;

                string PostImage = UserPostImages.Substring(0, UserPostImages.IndexOf("&token"));
                string PostCaption = UserCaption.EditText.Text;
                string Date = DateTime.Now.ToString();

                HashMap PostDataModel = new HashMap();
                PostDataModel.Put("UserId", UserId);
                PostDataModel.Put("PostImage", PostImage);
                PostDataModel.Put("PostCaption", PostCaption);
                PostDataModel.Put("DateCreated", Date);


                if (string.IsNullOrEmpty(UserCaption.EditText.Text))
                {
                    Snackbar.Make(postSnack, " Please Enter Your First Name", Snackbar.LengthLong)
                   .Show();
                }
                else
                {
                    progressBar.Visibility = ViewStates.Visible;

                    var UserDataBase = AppDataHelper.GetDatabase().GetReference("UsersPosts").Push();
                    UserDataBase.SetValue(PostDataModel).AddOnCompleteListener(this, this);
                }
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == PICK_IMAGE_REQUSET && (resultCode == Result.Ok) && (data != null))
            {
                UploadTask uploadTask = storageRef.PutFile(data.Data);
                var Task = uploadTask.ContinueWithTask(new MyAdvertContinueTask(this))
                .AddOnCompleteListener(new MyAdvertCompleteTask(this));
            }
            if (requestCode == PICK_IMAGE_REQUSET && resultCode == Result.Ok && data != null && data.Data != null)
            {
                //ArrayList<Image> images = data.GetParcelableArrayListExtra(Constants);
                //Android.Net.Uri[] uri = new Android.Net.Uri[images.Size()];

                filePath = data.Data;
                try
                {
                    Bitmap bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, filePath);
                    UserImageSlider1.SetImageBitmap(bitmap);
                }
                catch (IOException ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                progressBar.Visibility = ViewStates.Gone;

                StartActivity(new Android.Content.Intent(this, typeof(MainActivity)));
                Finish();
            }
            else
            {
                progressBar.Visibility = ViewStates.Gone;
                Snackbar.Make(postSnack, "Something went Wrong. Please try again", Snackbar.LengthLong)
                  .SetAction("CLEAR", (view) => { UserCaption.EditText.Text = string.Empty; })
                  .Show();
            }
        }
    }
    internal class MyAdvertCompleteTask : Java.Lang.Object, IOnCompleteListener
    {
        private NewItemAdd userPostPage;
        private string imageURL;

        public MyAdvertCompleteTask(NewItemAdd userPostPage)
        {
            this.userPostPage = userPostPage;
        }

        public void OnComplete(Task task)
        {
            var uri = task.Result as Android.Net.Uri;
            string Downloadurl = uri.ToString();
            Downloadurl = Downloadurl.Substring(0, Downloadurl.IndexOf("&token"));
            imageURL = Downloadurl;
            //Picasso.With().Load(Downloadurl).Into(signUpPage.profileImage);
        }
    }

    internal class MyAdvertContinueTask : Java.Lang.Object, IContinuation
    {
        private NewItemAdd userPostPage;

        public MyAdvertContinueTask(NewItemAdd userPostPage)
        {
            this.userPostPage = userPostPage;
        }

        public Java.Lang.Object Then(Task task)
        {
            if (!task.IsSuccessful)
            {
                Toast.MakeText(userPostPage, "Upload Failed", ToastLength.Short).Show();
            }
            return userPostPage.storageRef.GetDownloadUrl();
        }
    }
}


