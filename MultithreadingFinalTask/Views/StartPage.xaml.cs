using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MultithreadingFinalTask.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StartPage : Page
	{
		public StartPage()
		{
			this.InitializeComponent();
			
		}

		private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			FriendsViewer.Visibility = Visibility.Collapsed;
			MusicViewer.Visibility = Visibility.Collapsed;
		}

		private void RankButton_Click(object sender, RoutedEventArgs e)
		{
			MusicViewer.Visibility = Visibility.Visible;
			FriendsViewer.Visibility = Visibility.Collapsed;
		}

		private void FriendsButton_Click(object sender, RoutedEventArgs e)
		{ 
			FriendsViewer.Visibility = Visibility.Visible;
			MusicViewer.Visibility = Visibility.Collapsed;
		}

		private void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			FriendsViewer.Visibility = Visibility.Collapsed;
			MusicViewer.Visibility = Visibility.Collapsed;
		}
	}
}
