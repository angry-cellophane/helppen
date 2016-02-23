using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HelpPen.Client.Windows.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class UnexceptedErrorPage : Page
	{
		public UnexceptedErrorPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			Exception exception = (Exception) e.Parameter;

			while (exception.InnerException != null)
			{
				exception = exception.InnerException;
			}

			ErrorTitleBox.Text = exception.Message;
			ErrorTextBox.Text = exception.ToString();
		}
	}
}
