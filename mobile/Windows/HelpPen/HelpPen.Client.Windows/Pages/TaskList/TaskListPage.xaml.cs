using System.Windows.Input;

using Windows.System;
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
		#region Constructors and Destructors

		/// <summary>
		///     Initializes a new instance of the Page class.
		/// </summary>
		public TaskListPage()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		/// <summary>
		///     Создает модель пердставления для данной страницы.
		/// </summary>
		/// <returns></returns>
		protected override PageViewModel CreateViewModel()
		{
			return CreateInstance<TaskListViewModel>();
		}

		#endregion
	}
}