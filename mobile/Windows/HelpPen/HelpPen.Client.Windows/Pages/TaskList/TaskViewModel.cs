using HelpPen.Client.Common.Model.API;
using HelpPen.Client.Common.MVVM;

using Prism.Commands;

namespace HelpPen.Client.Windows.Pages.TaskList
{
	/// <summary>
	///     Модель представления для отдельной задачи.
	/// </summary>
	internal sealed class TaskViewModel : ViewModel
	{
		#region Fields

		private readonly TaskListViewModel _taskListViewModel;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Иницаилизирует новый экземпляр класса <see cref="TaskViewModel" />.
		/// </summary>
		/// <param name="taskListViewModel">Модель представления родительско списка задач.</param>
		/// <param name="task">Задача.</param>
		public TaskViewModel(TaskListViewModel taskListViewModel, Task task)
		{
			_taskListViewModel = taskListViewModel;
			Task = task;

			UpTaskCommand = new DelegateCommand(OnUpTaskCommandExecute);
			RemoveTaskCommand = new DelegateCommand(OnRemoveTaskCommandExecute);
		}

		#endregion

		#region Properties

		/// <summary>
		///     Команда удаления задачи.
		/// </summary>
		public DelegateCommand RemoveTaskCommand { get; }

		/// <summary>
		///     Задача.
		/// </summary>
		public Task Task { get; }

		/// <summary>
		///     Текст задачи.
		/// </summary>
		public string Text
		{
			get
			{
				return Task.text;
			}
		}

		/// <summary>
		///     Команда поднятия уровня задачи.
		/// </summary>
		public DelegateCommand UpTaskCommand { get; }

		#endregion

		#region Methods

		private async void OnRemoveTaskCommandExecute()
		{
			await _taskListViewModel.RemoveTask(this);
		}

		private async void OnUpTaskCommandExecute()
		{
			await _taskListViewModel.UpTask(this);
		}

		#endregion
	}
}