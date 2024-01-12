package crc64c3d97de63376fbd1;


public class NextSignUp_MyAdvertCompleteProfileTask
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.tasks.OnCompleteListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onComplete:(Lcom/google/android/gms/tasks/Task;)V:GetOnComplete_Lcom_google_android_gms_tasks_Task_Handler:Android.Gms.Tasks.IOnCompleteListenerInvoker, Xamarin.GooglePlayServices.Tasks\n" +
			"";
		mono.android.Runtime.register ("GhanaModels.NextSignUp+MyAdvertCompleteProfileTask, GhanaModels", NextSignUp_MyAdvertCompleteProfileTask.class, __md_methods);
	}


	public NextSignUp_MyAdvertCompleteProfileTask ()
	{
		super ();
		if (getClass () == NextSignUp_MyAdvertCompleteProfileTask.class)
			mono.android.TypeManager.Activate ("GhanaModels.NextSignUp+MyAdvertCompleteProfileTask, GhanaModels", "", this, new java.lang.Object[] {  });
	}

	public NextSignUp_MyAdvertCompleteProfileTask (crc64c3d97de63376fbd1.NextSignUp p0)
	{
		super ();
		if (getClass () == NextSignUp_MyAdvertCompleteProfileTask.class)
			mono.android.TypeManager.Activate ("GhanaModels.NextSignUp+MyAdvertCompleteProfileTask, GhanaModels", "GhanaModels.NextSignUp, GhanaModels", this, new java.lang.Object[] { p0 });
	}


	public void onComplete (com.google.android.gms.tasks.Task p0)
	{
		n_onComplete (p0);
	}

	private native void n_onComplete (com.google.android.gms.tasks.Task p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
