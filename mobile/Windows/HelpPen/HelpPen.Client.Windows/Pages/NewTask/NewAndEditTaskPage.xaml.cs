using System.Windows.Input;

using Windows.System;
using Windows.UI.Xaml.Input;

using HelpPen.Client.Common.MVVM;
using HelpPen.Client.Windows.Pages.TaskList;

using Microsoft.Practices.ServiceLocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HelpPen.Client.Windows.Pages.NewTask
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class NewAndEditTaskPage
	{
		#region Constructors and Destructors

		public NewAndEditTaskPage()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Methods
		
		/// <summary>
		///     Создает модель пердставления для данной страницы.
		/// </summary>
		/// <returns></returns>
		protected override PageViewModel CreateViewModel()
		{
			return CreateInstance<NewAndEditTaskViewModel>();
		}

		private void OnNewTaskTextBoxKeyUp(object sender, KeyRoutedEventArgs e)
		{
			//if (e.Key == VirtualKey.Enter && )
			//{
			//	ICommand command = ((NewAndEditTaskViewModel)DataContext).AcceptCommand;

			//	if (command.CanExecute(null))
			//	{
			//		command.Execute(null);
			//	}
			//}
		}

		#endregion
	}
}