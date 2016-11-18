using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms;

[assembly: Dependency(typeof(SqliteTest.Droid.PathProvider))]
namespace SqliteTest.Droid
{
	public class PathProvider: IPathProvider
	{
		public string GetDbPath()
		{
			return Path.Combine(Android.OS.Environment.DataDirectory.AbsolutePath, "test.db");
		}
	}

	[Activity(Label = "SqliteTest.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());
		}
	}
}
