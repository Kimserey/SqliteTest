using System;
using SQLite;
using Xamarin.Forms;

namespace SqliteTest
{
	public interface IPathProvider
	{
		string GetDbPath();
	}

	[Table("test_tabe")]
	public class Test {
		
		[Column("number")]
		public int Number { get; set; }
	}

	public class App : Application
	{
		public App()
		{
			var content = new ContentPage
			{
				Title = "SqliteTest",
				Content = new StackLayout
				{
					Children = {
						new Label {
							Text = "Test Sqlite"
						}
					}
				}
			};


			var path = DependencyService.Get<IPathProvider>().GetDbPath();

			var conn = new SQLiteConnection(path);
			conn.CreateTable<Test>();
			conn.Insert(new Test { Number = 10 });

			MainPage = new NavigationPage(content);
		}
	}
}
