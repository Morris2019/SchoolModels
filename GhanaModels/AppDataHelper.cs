using System;
using Android.App;
using Firebase;
using Firebase.Database;

namespace GhanaModels
{
    public static class AppDataHelper
    {
        public static FirebaseDatabase GetDatabase()
        {

            var app = FirebaseApp.InitializeApp(Application.Context);
            FirebaseDatabase database;

            if (app == null)
            {
                var option = new FirebaseOptions.Builder()
                    .SetApplicationId("ghanamodels-1e760")
                    .SetApiKey("AIzaSyBJuI__JMTIwvxSGHN4y9Edor2yA7UXPIA")
                    .SetDatabaseUrl("https://ghanamodels-1e760.firebaseio.com")
                    .SetStorageBucket("ghanamodels-1e760.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, option);
                database = FirebaseDatabase.GetInstance(app);
            }
            else
            {
                database = FirebaseDatabase.GetInstance(app);
            }

            return database;
        }

    }
}
