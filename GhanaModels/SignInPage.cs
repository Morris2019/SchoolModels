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
using static Android.Views.View;

namespace GhanaModels
{
    [Activity(Label = "SignInPage", Theme = "@style/AppTheme.NoActionBar")]
    public class SignInPage : AppCompatActivity, IOnClickListener, IOnCompleteListener
    {
        EditText UserNmae, UserPass;
        Button SignInBut, SignUpBut;
        TextView UserPassReset;
        FirebaseAuth auth;
        ProgressBar progressBar;
        RelativeLayout parentLayout;
        private long Backpresstime;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignInPage);
            FirebaseApp.InitializeApp(this);
            auth = FirebaseAuth.Instance;
            // Create your application here

            SignInBut = FindViewById<Button>(Resource.Id.signinbut);
            SignInBut.Click += delegate {
                if (string.IsNullOrEmpty(UserNmae.Text))
                {
                    Snackbar.Make(parentLayout, "Please Enter Your E-Mail.", Snackbar.LengthLong)
                   .SetAction("OK", (view) => { })
                   .Show();
                }
                else if (string.IsNullOrWhiteSpace(UserPass.Text))
                {
                    Snackbar.Make(parentLayout, "Please Enter Your Password.", Snackbar.LengthLong)
                   .SetAction("OK", (view) => { })
                   .Show();
                }
                else
                {
                    LoginUser(UserNmae.Text, UserPass.Text);
                }
            };
            SignUpBut = FindViewById<Button>(Resource.Id.SignUpText);
            SignUpBut.Click += delegate {
                StartActivity(new Android.Content.Intent(this, typeof(SignUpPage)));
                Finish();
            };
            UserPassReset = FindViewById<TextView>(Resource.Id.ForgotText);
            UserPassReset.Click += delegate {
                StartActivity(new Android.Content.Intent(this, typeof(UserPasswordReset)));
                Finish();
            };
            UserNmae = FindViewById<EditText>(Resource.Id.UserEmail);
            UserPass = FindViewById<EditText>(Resource.Id.Userpassword);
            progressBar = FindViewById<ProgressBar>(Resource.Id.circularProgress);
            parentLayout = FindViewById<RelativeLayout>(Resource.Id.SigninPageRelative);
        }
        private void LoginUser(string email, string password)
        {
            progressBar.Visibility = ViewStates.Visible;
            auth.SignInWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this);
        }
        public void OnClick(View v)
        {

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
                Snackbar.Make(parentLayout, "Wrong Password.", Snackbar.LengthLong)
                  .SetAction("CLEAR", (view) => { UserPass.Text = string.Empty; })
                  .Show();
            }
        }
        public override void OnBackPressed()
        {
            if (Backpresstime + 200 > SystemClock.CurrentThreadTimeMillis())
            {
                base.OnBackPressed();
                return;
            }
            else
            {

            }
            Backpresstime = SystemClock.CurrentThreadTimeMillis();

        }
    }
}
