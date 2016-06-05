using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Windows.Security.Credentials;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
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
		#region Static Fields

		public static readonly DependencyProperty IsInputEnabledProperty = DependencyProperty.Register(
			@"IsInputEnabled",
			typeof(bool),
			typeof(LoginPage),
			new PropertyMetadata(true));

		#endregion

		#region Fields

		private readonly IAuthService _authService;

		#endregion

		#region Constructors and Destructors

		public LoginPage()
		{
			InitializeComponent();

			_authService = ServiceLocator.Current.GetInstance<IAuthService>();

			DataContext = this;

			TryRestoreCredential();
		}

		#endregion

		#region Properties

		public bool IsInputEnabled
		{
			get
			{
				return (bool)GetValue(IsInputEnabledProperty);
			}
			set
			{
				SetValue(IsInputEnabledProperty, value);
			}
		}

		#endregion

		#region Methods


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

		private void RemoveCredential()
		{
			var vault = new PasswordVault();
			try
			{
				vault.Remove(vault.Retrieve(Settings.ServerUri.AbsoluteUri, UserNameTextBox.Text));
			}
			catch (Exception)
			{
			}
		}

		private void SaveCredential()
		{
			var vault = new PasswordVault();
			var credential = new PasswordCredential(Settings.ServerUri.AbsoluteUri, UserNameTextBox.Text, PasswordTextBox.Password);

			vault.Add(credential);
		}

		private async Task TryLogin()
		{
			IsInputEnabled = false;
			ProgressRing.IsActive = true;

			try
			{
				var credential = new NetworkCredential(UserNameTextBox.Text, PasswordTextBox.Password);

				await _authService.Login(credential, CancellationToken.None);

				SaveCredential();

				Frame.Navigate(typeof(TaskListPage));
			}
			catch (InvalidCredentialException ex)
			{
				RemoveCredential();

				var dialog = new MessageDialog(
					ex.Message,
					"Попытка входа в систему не удалась");

				dialog.Commands.Add(new UICommand("OK"));

				await dialog.ShowAsync();
			}

			IsInputEnabled = true;
			ProgressRing.IsActive = false;
		}

		private void TryRestoreCredential()
		{
			PasswordVault vault = new PasswordVault();

			try
			{
				PasswordCredential credential = vault.FindAllByResource(Settings.ServerUri.AbsoluteUri).FirstOrDefault();

				if (credential != null)
				{
					UserNameTextBox.Text = credential.UserName;

					PasswordCredential passwordCredential = vault.Retrieve(Settings.ServerUri.AbsoluteUri, credential.UserName);

					if (passwordCredential != null)
					{
						PasswordTextBox.Password = passwordCredential.Password;
					}
				}
			}
			catch (Exception)
			{
				// не сведений об учетных данных (FindAllByResource выбрасывает исключение в этом случае)
			}
		}

		#endregion
	}
}