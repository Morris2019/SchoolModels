using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using FR.Ganfra.Materialspinner;
using Java.Util;
using Refractored.Controls;
using static Java.Util.Locale;

namespace GhanaModels
{
    [Activity(Label = "NextSignUp", Theme = "@style/AppTheme.NoActionBar")]
    public class NextSignUp : AppCompatActivity, IOnCompleteListener
    {
        Button CreateUser;
        MaterialSpinner UsersWeigth, UserHeight, UserFeetSize, UserComplexion;
        TextInputLayout First_name, last_name, UserName, phone, Usernational, UserBioggraphy, UserAge;
        CircleImageView ProfileImage;
        CoordinatorLayout SignUpSnack;
        ProgressBar progressBar;

        List<string> weightlist, heightlist, FeetSizelist, Complexionlist;
        ArrayAdapter<string> weightadapter, heightadapter, FeetSizelistAdapter, ComplexionAdapter;

        public string User_Weight { get; set; }
        public string user_Height { get; set; }
        public string User_FeetSize { get; set; }
        public string User_Complexion { get; set; }

        public int PICK_IMAGE_REQUSET = 1000;
        private Android.Net.Uri filePath;

        public string UserId { get; private set; }
        public string UserEmail { get; private set; }
        public object UserProfileImage { get; private set; }

        private FirebaseAuth auth;
        FirebaseStorage firebaseStorage;
        public StorageReference storage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NextSignUp);
            // Create your application here
            FirebaseApp.InitializeApp(this);
            auth = FirebaseAuth.Instance;
            firebaseStorage = FirebaseStorage.Instance;
            storage = firebaseStorage.Reference.Child("ProfileImages/" + Guid.NewGuid().ToString());

            
            SignUpSnack = FindViewById<CoordinatorLayout>(Resource.Id.SignUpSnack);
            //progressBar = FindViewById<ProgressBar>(Resource.Id.circularProgress);
            ProfileImage = FindViewById<CircleImageView>(Resource.Id.UserProfile);
            ProfileImage.Click += SelectProfiile;
            UsersWeigth = FindViewById<MaterialSpinner>(Resource.Id.UserWeight);
            WeightList();
            UserHeight = FindViewById<MaterialSpinner>(Resource.Id.UserHeight);
            HeighttList();
            UserFeetSize = FindViewById<MaterialSpinner>(Resource.Id.UserfeetSize);
            UserFeetSizeList();
            UserComplexion = FindViewById<MaterialSpinner>(Resource.Id.UseComplexion);
            UserComplexionList();
            First_name = FindViewById<TextInputLayout>(Resource.Id.First_name);
            last_name = FindViewById<TextInputLayout>(Resource.Id.last_name);
            UserName = FindViewById<TextInputLayout>(Resource.Id.UserName);
            phone = FindViewById<TextInputLayout>(Resource.Id.phone);
            UserAge = FindViewById<TextInputLayout>(Resource.Id.UserAge);
            UserBioggraphy = FindViewById<TextInputLayout>(Resource.Id.UserBio);
            Usernational = FindViewById<TextInputLayout>(Resource.Id.Usernational);
            //birthDate = FindViewById<TextInputEditText>(Resource.Id.UserDateofbith);
            CreateUser = FindViewById<Button>(Resource.Id.NextsignUp);
            CreateUser.Click += CreateUserNewUser;
        }

        private void SelectProfiile(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), PICK_IMAGE_REQUSET);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == PICK_IMAGE_REQUSET && (resultCode == Result.Ok) && (data != null))
            {
                UploadTask uploadTask = storage.PutFile(data.Data);
                var Task = uploadTask.ContinueWithTask(new MyAdvertContinueProfileTask(this))
                .AddOnCompleteListener(new MyAdvertCompleteProfileTask(this));
            }
            if (requestCode == PICK_IMAGE_REQUSET &&
                resultCode == Result.Ok &&
                data != null &&
                data.Data != null)
            {
                filePath = data.Data;
                try
                {
                    Bitmap bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, filePath);
                    ProfileImage.SetImageBitmap(bitmap);
                }
                catch (IOException ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
        }
        public async void CreateUserNewUser(object sender, EventArgs e)
        {
            string UserProfile = (string)await storage.GetDownloadUrlAsync();
            FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
            if (user != null)
            {
                UserEmail = user.Email;
                UserId = user.Uid;

                string ProfileImages = UserProfile;
                string firstName = First_name.EditText.Text;
                string lastName = last_name.EditText.Text;
                string User_Name = UserName.EditText.Text;
                string phone_Number = phone.EditText.Text;
                string User_national = Usernational.EditText.Text;
                string User_Email = UserEmail;
                string User_Age = UserAge.EditText.Text;
                string User_Biography = UserBioggraphy.EditText.Text;
                string UserWeight = User_Weight;
                string UserHeight = user_Height;
                string UserFeetSize = User_FeetSize;
                string UserComlexion = User_Complexion;
                string Date = DateTime.Now.ToString();

                HashMap userDataModel = new HashMap();
                userDataModel.Put("UserId", UserId);
                userDataModel.Put("ProfileImages", ProfileImages);
                userDataModel.Put("FirstName", firstName);
                userDataModel.Put("LastName", lastName);
                userDataModel.Put("UserName", User_Name);
                userDataModel.Put("UserContact", phone_Number);
                userDataModel.Put("UserCountry", User_national);
                userDataModel.Put("UserEmail", User_Email);
                userDataModel.Put("Feet_Size", UserFeetSize);
                userDataModel.Put("Age", User_Age);
                userDataModel.Put("Biography", User_Biography);
                userDataModel.Put("Height", UserHeight);
                userDataModel.Put("Weight", UserWeight);
                userDataModel.Put("Complexion", UserComlexion);
                userDataModel.Put("DateCreated", Date);

                if (string.IsNullOrEmpty(First_name.EditText.Text))
                {
                    Snackbar.Make(SignUpSnack, " Please Enter Your First Name", Snackbar.LengthLong)
                   .Show();
                }
                else if (string.IsNullOrEmpty(last_name.EditText.Text))
                {
                    Snackbar.Make(SignUpSnack, "Please Enter Your Last Name", Snackbar.LengthLong)
                   .Show();
                }
                else if (string.IsNullOrEmpty(UserName.EditText.Text))
                {
                    Snackbar.Make(SignUpSnack, "Please Enter Your User Name", Snackbar.LengthLong)
                   .Show();
                }
                else if (string.IsNullOrEmpty(phone.EditText.Text))
                {
                    Snackbar.Make(SignUpSnack, "Please Enter Your Phone Number", Snackbar.LengthLong)
                   .Show();
                }
                else if (string.IsNullOrEmpty(Usernational.EditText.Text))
                {
                    Snackbar.Make(SignUpSnack, "Please Enter Nationality", Snackbar.LengthLong)
                   .Show();
                }
                else if (string.IsNullOrEmpty(UserBioggraphy.EditText.Text))
                {
                    Snackbar.Make(SignUpSnack, "Please Enter Your Last Name", Snackbar.LengthLong)
                   .Show();
                }
                else if (string.IsNullOrEmpty(UserAge.EditText.Text))
                {
                    Snackbar.Make(SignUpSnack, "Please Enter Your Last Name", Snackbar.LengthLong)
                   .Show();
                }
                else
                {

                    var UserDataBase = AppDataHelper.GetDatabase().GetReference( "Users").Child(UserId);
                    UserDataBase.SetValue(userDataModel).AddOnCompleteListener(this, this);
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                }
            }
            else
            {
                var intent = new Intent(this, typeof(SignUpPage));
                StartActivity(intent);
            }
        }
        public void WeightList()
        {
            weightlist = new List<string>();
            weightlist.Add("30kg - 35kg");
            weightlist.Add("36kg - 40kg");
            weightlist.Add("41kg - 45kg");
            weightlist.Add("46kg - 50kg");
            weightlist.Add("51kg - 55kg");
            weightlist.Add("56kg - 60kg");
            weightlist.Add("61kg - 65kg");
            weightlist.Add("66kg - 70kg");
            weightlist.Add("71kg - 75kg");
            weightlist.Add("76kg - 80kg");

            weightadapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, weightlist);
            weightadapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            UsersWeigth.Adapter = weightadapter;
            UsersWeigth.ItemSelected += WeightSelected;
        }
        public void WeightSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != -1)
            {
                User_Weight = weightlist[e.Position];
            }
        }
        public void HeighttList()
        {
            heightlist = new List<string>();
            heightlist.Add("140cm - 145cm");
            heightlist.Add("146cm - 150cm");
            heightlist.Add("151cm - 155cm");
            heightlist.Add("156cm - 160cm");
            heightlist.Add("161cm - 165cm");
            heightlist.Add("166cm - 170cm");
            heightlist.Add("171cm - 175cm");
            heightlist.Add("176cm - 180cm");
            heightlist.Add("181cm - 185cm");
            heightlist.Add("186cm - 190cm");

            heightadapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, heightlist);
            heightadapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            UserHeight.Adapter = heightadapter;
            UserHeight.ItemSelected += heightSelected;
        }
        public void heightSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != -1)
            {
                user_Height = heightlist[e.Position];
            }
        }
        public void UserFeetSizeList()
        {
            FeetSizelist = new List<string>();
            FeetSizelist.Add("4");
            FeetSizelist.Add("5");
            FeetSizelist.Add("6");
            FeetSizelist.Add("7");
            FeetSizelist.Add("8");
            FeetSizelist.Add("9");
            FeetSizelist.Add("10");
            FeetSizelist.Add("11");

            FeetSizelistAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, FeetSizelist);
            FeetSizelistAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            UserFeetSize.Adapter = FeetSizelistAdapter;
            UserFeetSize.ItemSelected += FeetSizeSelected;
        }
        public void FeetSizeSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != -1)
            {
                User_FeetSize = heightlist[e.Position];
            }
        }
        public void UserComplexionList()
        {
            Complexionlist = new List<string>();
            Complexionlist.Add("Fair skin");
            Complexionlist.Add("Chocolate skin");
            Complexionlist.Add("Dark skin");

            ComplexionAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Complexionlist);
            ComplexionAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            UserComplexion.Adapter = ComplexionAdapter;
            UserComplexion.ItemSelected += ComplexionlistSelected;
        }
        public void ComplexionlistSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != -1)
            {
                User_Complexion = Complexionlist[e.Position];
            }
        }
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
               // progressBar.Visibility = ViewStates.Visible;
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
               // progressBar.Visibility = ViewStates.Gone;
            }
            else
            {
                Snackbar.Make(SignUpSnack, " Something went wrong", Snackbar.LengthLong)
                .Show();
            }
        }
        public override void OnBackPressed()
        {
            StartActivity(new Android.Content.Intent(this, typeof(SignInPage)));
            Finish();
            base.OnBackPressed();
        }

        internal class MyAdvertCompleteProfileTask : Java.Lang.Object, IOnCompleteListener
        {
            private NextSignUp userPostPage;

            public MyAdvertCompleteProfileTask(NextSignUp userPostPage)
            {
                this.userPostPage = userPostPage;
            }

            public string UserProfileImage { get; private set; }

            public void OnComplete(Task task)
            {
                var uri = task.Result as Android.Net.Uri;
                string Downloadurl = uri.ToString();
                Downloadurl = Downloadurl.Substring(0, Downloadurl.IndexOf("&token"));
                UserProfileImage = Downloadurl;
                //Picasso.With().Load(Downloadurl).Into(signUpPage.profileImage);
            }
        }

        internal class MyAdvertContinueProfileTask : Java.Lang.Object, IContinuation
        {
            private NextSignUp userPostPage;

            public MyAdvertContinueProfileTask(NextSignUp userPostPage)
            {
                this.userPostPage = userPostPage;
            }

            public Java.Lang.Object Then(Task task)
            {
                if (!task.IsSuccessful)
                {
                    Toast.MakeText(userPostPage, "Upload Failed", ToastLength.Short).Show();
                }
                return userPostPage.storage.GetDownloadUrl();
            }
        }
    }
}
