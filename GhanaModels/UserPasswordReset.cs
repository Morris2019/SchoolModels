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
using Firebase;
using Firebase.Auth;
using Xamarin.Essentials;

namespace GhanaModels
{
    [Activity(Label = "UserPasswordReset", Theme = "@style/AppTheme.NoActionBar")]
    public class UserPasswordReset : AppCompatActivity, IOnCompleteListener
    {
        EditText UserpassReset;
        Button Passreset;
        FirebaseAuth memberauth;
        ProgressBar circularProgress;
        RelativeLayout ResetLayout;

       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserPasswordReset);
            FirebaseApp.InitializeApp(this);
            memberauth = FirebaseAuth.Instance;
            // Create your application here
            UserpassReset = FindViewById<EditText>(Resource.Id.emailbox);
            circularProgress = FindViewById<ProgressBar>(Resource.Id.circularProgress);
            ResetLayout = FindViewById<RelativeLayout >(Resource.Id.MemberresetPage);
            Passreset = FindViewById<Button>(Resource.Id.MemberPasscodereset);
            Passreset.Click += delegate
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    if (string.IsNullOrEmpty(UserpassReset.Text))
                    {
                        Toast.MakeText(this, "Please Enter Your School Mail Address ", ToastLength.Short).Show();
                        Snackbar.Make(ResetLayout, "Please Enter Your E-Mail.", Snackbar.LengthLong)
                          .SetAction("OK", (view) => { })
                          .Show();
                    }
                    else
                    {
                        ResetPassword(UserpassReset.Text);
                    }
                }
                else
                {
                    Snackbar.Make(ResetLayout, "Please Check Your Network and Try Again.", Snackbar.LengthLong)
                   .SetAction("OK", (view) => { })
                   .Show();
                }
                
            };
        }

        private void ResetPassword(string ModelEmail)
        {
            circularProgress.Visibility = ViewStates.Visible;
            memberauth.SendPasswordResetEmail(ModelEmail).AddOnCompleteListener(this, this);
        }
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                circularProgress.Visibility = ViewStates.Gone;
                StartActivity(new Android.Content.Intent(this, typeof(SignInPage)));
                Finish();
            }
            else
            {
                circularProgress.Visibility = ViewStates.Gone;
                Toast.MakeText(this, "Something went wrong, Please Try Again", ToastLength.Short).Show();
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
