using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using HelpPen.Client.Common;
using HelpPen.Client.Windows.Pages.TaskList;
using Microsoft.Practices.ServiceLocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HelpPen.Client.Windows.Pages
{
	/// <summary>
	///     Страница аутентификации.
	/// </summary>
	public sealed partial class LoginPage
	{
		public static readonly DependencyProperty IsInputEnabledProperty = DependencyProperty.Register(
			@"IsInputEnabled", typeof (bool), typeof (LoginPage), new PropertyMetadata(true));

		private readonly IAuthService _authService;

		public LoginPage()
		{
			InitializeComponent();

			_authService = ServiceLocator.Current.GetInstance<IAuthService>();

			DataContext = this;
		}

		public bool IsInputEnabled
		{
			get { return (bool) GetValue(IsInputEnabledProperty); }
			set { SetValue(IsInputEnabledProperty, value); }
		}

		private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
		{
			await TryLogin();
		}

		private async void OnTextBoxKeyUp(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
			{
				await TryLogin();
			}
		}

		private async Task TryLogin()
		{
			IsInputEnabled = false;
			ProgressRing.IsActive = true;

			try
			{
				var credential = new NetworkCredential(UserNameTextBox.Text, PasswordTextBox.Password);

				await _authService.Login(credential, CancellationToken.None);

				Frame.Navigate(typeof (TaskListPage));
			}
			catch (InvalidCredentialException ex)
			{
				var dialog = new MessageDialog(
					ex.Message,
					"Попытка входа в систему не удалась");

				dialog.Commands.Add(new UICommand("OK"));

				await dialog.ShowAsync();
			}

			IsInputEnabled = true;
			ProgressRing.IsActive = false;
		}

		private async void OnDebugInfoTapped(object sender, TappedRoutedEventArgs e)
		{
			UserNameTextBox.Text = @"DebugUser";
			PasswordTextBox.Password = @"password";
			LoginButton.Focus(FocusState.Keyboard);
		}
	}
}