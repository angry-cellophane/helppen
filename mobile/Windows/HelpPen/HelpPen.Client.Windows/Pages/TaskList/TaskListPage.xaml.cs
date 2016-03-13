using System.Windows.Input;

using Windows.UI.Xaml.Input;

using HelpPen.Client.Common.MVVM;

using Microsoft.Practices.ServiceLocation;

namespace HelpPen.Client.Windows.Pages.TaskList
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class TaskListPage
	{
		#region Methods

		/// <summary>
		///     Создает модель пердставления для данной страницы.
		/// </summary>
		/// <returns></returns>
		protected override PageViewModel CreateViewModel()
		{
			return ServiceLocator.Current.GetInstance<TaskListViewModel>();
		}

		private void OnNewTaskTextBoxKeyUp(object sender, KeyRoutedEventArgs e)
		{
			ICommand command = ((TaskListViewModel)ViewModel).AddNewTaskCommand;

			if (command.CanExecute(null))
			{
				command.Execute(null);
			}
		}

		#endregion
	}
}