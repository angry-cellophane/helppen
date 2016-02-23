using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HelpPen.Client.Common;
using HelpPen.Client.Common.Model.API;
using HelpPen.Client.Windows.Pages.TaskList;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HelpPen.Client.Windows.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LoginPage : Page
	{
		private readonly IAuthService _authService;

		public LoginPage()
		{
			this.InitializeComponent();

			_authService = new AuthService();
		}

		private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
		{
			ProgressRing.IsActive = true;

			Session session = await TryLogin(UserNameTextBox.Text, PasswordTextBox.Password, CancellationToken.None);

			ProgressRing.IsActive = false;

			Frame.Navigate(typeof(TaskListPage));
		}

		private async Task<Session> TryLogin(string userName, string password, CancellationToken cancellationToken)
		{
			NetworkCredential credential = new NetworkCredential(userName, password);

			Session session = await _authService.Login(credential, cancellationToken);

			return session;
		}
	}
}
