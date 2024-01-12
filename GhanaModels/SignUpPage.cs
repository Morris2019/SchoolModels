using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;

namespace GhanaModels
{
    [Activity(Label = "SignUpPage", Theme = "@style/AppTheme.NoActionBar")]
    public class SignUpPage : AppCompatActivity, IOnCompleteListener
    {
        RelativeLayout UserSignUpSnack;
        ProgressBar progressBar;
        Button GotoNextPage;
        FirebaseAuth auth;
        EditText UserEmailText, UserpasscodeText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.SignUpPage);
            auth = FirebaseAuth.Instance;

            GotoNextPage = FindViewById<Button>(Resource.Id.SignuPage1);
            GotoNextPage.Click += NextIntentPage;
            progressBar = FindViewById<ProgressBar>(Resource.Id.SignUpProgress);
            UserSignUpSnack = FindViewById<RelativeLayout>(Resource.Id.SignUpRelative);
            UserEmailText = FindViewById<EditText>(Resource.Id.SignUpUserEmail);
            UserpasscodeText = FindViewById<EditText>(Resource.Id.SignUppassword);
        }
        private void NextIntentPage(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserEmailText.Text))
            {
                Snackbar.Make(UserSignUpSnack, "Please Enter Your E-Mail.", Snackbar.LengthLong)
               .SetAction("OK", (view) => { })
               .Show();
            }
            else if (string.IsNullOrWhiteSpace(UserpasscodeText.Text))
            {
                Snackbar.Make(UserSignUpSnack, "Please Enter Your Password.", Snackbar.LengthLong)
               .SetAction("OK", (view) => { })
               .Show();
            }
            else
            {
                CreateNewUser(UserEmailText.Text, UserpasscodeText.Text);
            }
        }

        private void CreateNewUser(string email, string password)
        {
            progressBar.Visibility = ViewStates.Visible;
            auth.CreateUserWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                progressBar.Visibility = ViewStates.Gone;

                StartActivity(new Android.Content.Intent(this, typeof(NextSignUp)));
                Finish();
            }
            else
            {
                progressBar.Visibility = ViewStates.Gone;
                Snackbar.Make(UserSignUpSnack, "Something went Wrong. Please try again", Snackbar.LengthLong)
                  .SetAction("CLEAR", (view) => { UserEmailText.Text = string.Empty; UserpasscodeText.Text = string.Empty; })
                  .Show();
            }
        }
        public override void OnBackPressed()
        {
            StartActivity(new Android.Content.Intent(this, typeof(SignInPage)));
            Finish();
            base.OnBackPressed();
        }
    }
}
