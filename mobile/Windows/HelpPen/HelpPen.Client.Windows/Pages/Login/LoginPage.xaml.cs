using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using App3.Pages.TaskList;
using HelpPen.Client.Common;
using HelpPen.Client.Common.Model.API;
using Task = System.Threading.Tasks.Task;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App3.Pages.Login
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
