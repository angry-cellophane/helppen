using System;
using System.Collections.ObjectModel;

using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using HelpPen.Client.Common.MVVM;

namespace HelpPen.Client.Windows.Pages.TaskList
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class TaskListPage
	{
		#region Fields

		private int _index;

		#endregion

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

		private async void OnListViewDragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs e)
		{
			ListView listView = (ListView)sender;
			if (e.Items.Count > 0)
			{
				TaskListViewModel taskListViewModel = (TaskListViewModel)listView.DataContext;

				await taskListViewModel.EndDragItems((ObservableCollection<TaskViewModel>)listView.ItemsSource);
			}
		}

		private void OnListViewDragItemsStarting(object sender, DragItemsStartingEventArgs e)
		{
			ListView listView = (ListView)sender;

			if (e.Items.Count > 0)
			{
				TaskListViewModel taskListViewModel = (TaskListViewModel)listView.DataContext;

				taskListViewModel.BeginDragItems((ObservableCollection<TaskViewModel>)listView.ItemsSource);
			}
		}

		private async void UIElement_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			var dialog = new MessageDialog("UIElement_OnRightTapped");

			await dialog.ShowAsync();
		}

		#endregion
	}
}