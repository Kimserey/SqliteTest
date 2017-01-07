using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using Xamarin.Forms;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SqliteTest
{
	public interface IPathProvider
	{
		string GetDbPath();
	}

	[Table("test_table")]
	public class Test {

		[Column("id"), PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Column("text"), Collation("NOCASE")]
		public string Text { get; set; }
	}

	public class TestCell : TextCell
	{
		public TestCell()
		{ 
			this.SetBinding(TextCell.TextProperty, "Text");

			ContextActions.Add(
				new ToolbarItem
				{
					Text = "DELETE",
					Command = new Command(Delete)
				}
			);
		}

		void Delete()
		{ 
			var item = (Test)BindingContext;
			using (var conn = App.GetSQliteConnection())
			{
				if (conn.Delete(item) > 0)
				{
					App.items.Remove(item);
				}
			}
		}
	}

	public class App : Application
	{
		ListView list;
		internal static ObservableCollection<Test> items = new ObservableCollection<Test>();

		public App()
		{
			list = new ListView { 
				IsPullToRefreshEnabled = true,
				RefreshCommand = new Command(() => Refresh(true)),
				ItemsSource = items,
				ItemTemplate = new DataTemplate(typeof(TestCell))
			};


			var entry = new Entry { Placeholder = "Type something to save here.", Keyboard = Keyboard.Text };
			var button = new Button { Text = "Insert" };
			button.Clicked += (sender, e) => {
				Insert(entry.Text);
			};
			var header = new AbsoluteLayout();
			header.Children.Add(entry, new Rectangle(0, 0, .8, 1), AbsoluteLayoutFlags.All);
			header.Children.Add(button, new Rectangle(1, 0, .2, 1), AbsoluteLayoutFlags.All);

			var content = new ContentPage
			{
				Title = "SqliteTest",
				Content = new StackLayout
				{
					Children = {
						header,
						list
					}
				}
			};

			MainPage = new NavigationPage(content);

			Refresh();
		}

		internal static SQLiteConnection GetSQliteConnection()
		{ 
			var path = DependencyService.Get<IPathProvider>().GetDbPath();
			var conn = new SQLiteConnection(path);
			conn.CreateTable<Test>();
			return conn;
		}

		void Insert(string text)
		{
			var newItem = new Test { Text = text };
			using (var conn = GetSQliteConnection())
			{
				if (conn.Insert(newItem) > 0)
				{
					items.Add(newItem);
				}
			}
		}

		void Refresh(bool isPullRefresh = false) 
		{
			items.Clear();

			using (var conn = GetSQliteConnection())
			{
				var tests = conn.DeferredQuery<Test>("SELECT * FROM test_table");
				foreach (var test in tests)
				{
					items.Add(test);	
				}
			}

			if (isPullRefresh)
			{
				list.EndRefresh();
			}
		}
	}
}
