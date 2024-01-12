using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Plugin.Connectivity;

namespace GhanaModels
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        BottomNavigationView bottomNavigation;

        FirebaseAuth auth;
        private int MyResultCode = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Firebase.FirebaseApp.InitializeApp(this);
            auth = FirebaseAuth.Instance;
            var IsUserLogedIn = auth.CurrentUser;

            if (IsUserLogedIn != null)
            {
                //
            }
            else
            {
                StartActivityForResult(new Android.Content.Intent(this, typeof(SignInPage)), MyResultCode);
            }
            if(!CrossConnectivity.Current.IsConnected)
            {
             
            }
            else
            {
            }
            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            LoadFragment(Resource.Id.nav_home);
        }
        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            Intent intent;

            switch (id)
            {
                case Resource.Id.nav_home:
                    fragment = HomeFragment.NewInstance();
                    break;
                case Resource.Id.nav_search:
                    fragment = SearchFragment.NewInstance();
                    break;
                case Resource.Id.nav_add:
                    intent = new Intent(this, typeof(NewItemAdd));
                    StartActivity(intent);
                    break;
                case Resource.Id.nav_message:
                    fragment = MessageFragment.NewInstance();
                    break;
                case Resource.Id.nav_account:
                    fragment = ProfileFragment.NewInstance();
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

