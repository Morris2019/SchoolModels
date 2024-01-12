using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using FFImageLoading;

namespace GhanaModels
{
    [Activity(Label = "ProfileviewPagge", Theme = "@style/AppTheme.NoActionBar")]
    public class ProfileviewPagge : AppCompatActivity
    {
        TextView Userfirstname, Userlastname, Username, Uaserage, Usercomplexion, Usercontact, Userfootsize, Userweight, Userheight, Userbiography, Useremail;
        ImageView UserProfileImage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProfileviewPagge);
            // Create your application here

            Userfirstname = FindViewById<TextView>(Resource.Id.homeUserfirstname);
            Userlastname = FindViewById<TextView>(Resource.Id.homeUserlastname);
            Username = FindViewById<TextView>(Resource.Id.homeusernames);
            Uaserage = FindViewById<TextView>(Resource.Id.homeUserage);
            Usercomplexion = FindViewById<TextView>(Resource.Id.homeUsercomplex);
            Usercontact = FindViewById<TextView>(Resource.Id.homeContact);
            Userfootsize = FindViewById<TextView>(Resource.Id.homefootSize);
            Userweight = FindViewById<TextView>(Resource.Id.homeWeight);
            Userheight = FindViewById<TextView>(Resource.Id.homeHeight);
            Userbiography = FindViewById<TextView>(Resource.Id.homeBiography);
            Useremail = FindViewById<TextView>(Resource.Id.homeEmail);

            UserProfileImage = FindViewById<ImageView>(Resource.Id.UserProImage);


            Userfirstname.Text = Intent.GetStringExtra("Firstname");
            Userlastname.Text = Intent.GetStringExtra("Lastname");
            Username.Text = Intent.GetStringExtra("UserName");
            Useremail.Text = Intent.GetStringExtra("UserEmail");
            Uaserage.Text = Intent.GetStringExtra("UserAge");
            Usercomplexion.Text = Intent.GetStringExtra("Complexion");
            Usercontact.Text = Intent.GetStringExtra("UserContact");
            Userfootsize.Text = Intent.GetStringExtra("FootSize");
            Userweight.Text = Intent.GetStringExtra("Weight");
            Userheight.Text = Intent.GetStringExtra("Height");
            Userbiography.Text = Intent.GetStringExtra("Biography");

            var UserImageUrl = Intent.GetStringExtra("UserProfileImages");

            if (UserImageUrl != null)
            {
                ImageService.Instance.LoadUrl(UserImageUrl).Retry(5, 200).Into(UserProfileImage);
            }
        }
    }
}
