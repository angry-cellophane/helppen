using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using HelpPen.Client.Common;
using HelpPen.Client.Common.Model.API;
using HelpPen.Client.Common.MVVM;
using HelpPen.Client.Windows.Pages.NewTask;

using Microsoft.Practices.ServiceLocation;

using Prism.Commands;

using Task = System.Threading.Tasks.Task;

namespace HelpPen.Client.Windows.Pages.TaskList
{
	/// <summary>
	///     Модель представления для <see cref="TaskListPage" />.
	/// </summary>
	internal sealed class TaskListViewModel : PageViewModel
	{
		#region Fields

		private readonly IHelpPenService _helpPenService;

		private bool _isWorking;


		private TaskViewModel _selectedTask;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="TaskListViewModel" />.
		/// </summary>
		public TaskListViewModel(Frame frame, IHelpPenService helpPenService)
			: base(frame)
		{
			_helpPenService = helpPenService;

			TryAddNewTaskCommand = new DelegateCommand(OnTryAddNewTaskCommand);
			UpTaskCommand = new DelegateCommand(OnUpTaskCommandExecute, OnCanUpTaskCommandExecute);
			RemoveTaskCommand = new DelegateCommand(OnRemoveTaskCommandExecute, () => SelectedTask != null);

			NotCompletedTasks = new ObservableCollection<TaskViewModel>();
			StashedTasks = new ObservableCollection<TaskViewModel>();
		}

		#endregion

		#region Properties

		/// <summary>
		///     Команда начала добавления новой задачи.
		/// </summary>
		public DelegateCommand TryAddNewTaskCommand { get; }

		/// <summary>
		///     Содержит признак занятости выполнением той или иной операции.
		/// </summary>
		public bool IsWorking
		{
			get
			{
				return _isWorking;
			}
			set
			{
				SetPropertyValue(_isWorking, value, newValue => _isWorking = newValue);
			}
		}

		/// <summary>
		///     Команда удаления задачи.
		/// </summary>
		public DelegateCommand RemoveTaskCommand { get; }

		/// <summary>
		///     Выбранная задача.
		/// </summary>
		public TaskViewModel SelectedTask
		{
			get
			{
				return _selectedTask;
			}
			set
			{
				SetPropertyValue(_selectedTask, value, newValue => _selectedTask = newValue);
				UpTaskCommand.RaiseCanExecuteChanged();
				RemoveTaskCommand.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		///     Перечень невыполненных задач текущего пользователя.
		/// </summary>
		public ObservableCollection<TaskViewModel> NotCompletedTasks { get; }

		/// <summary>
		///     Перечень чердачных задач текущего пользователя.
		/// </summary>
		public ObservableCollection<TaskViewModel> StashedTasks { get; }

		/// <summary>
		///     Команда поднятия уровня задачи.
		/// </summary>
		public DelegateCommand UpTaskCommand { get; }

		#endregion

		#region Public Methods and Operators

		public override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			IsWorking = true;

			try
			{
				ImmutableArray<Common.Model.API.Task> tasks = await _helpPenService.GetTasks(CancellationToken.None);

				NotCompletedTasks.Clear();
				foreach (Common.Model.API.Task task in tasks.Where(x => x.state == TaskState.NOT_COMPLITED).OrderBy(x => x, TaskOrderComparer.Instance))
				{
					NotCompletedTasks.Add(new TaskViewModel(task));
				}

				StashedTasks.Clear();
				foreach (Common.Model.API.Task task in tasks.Where(x => x.state == TaskState.STASH).OrderBy(x => x, TaskOrderComparer.Instance))
				{
					StashedTasks.Add(new TaskViewModel(task));
				}

				OrderTasksIfNeeded(NotCompletedTasks);
				OrderTasksIfNeeded(StashedTasks);
			}
			finally
			{
				IsWorking = false;
			}
		}

		public async Task RemoveTask(TaskViewModel taskViewModel)
		{
			IHelpPenService helpPenService = ServiceLocator.Current.GetInstance<IHelpPenService>();

			await helpPenService.RemoveTask(taskViewModel.Task.Id, CancellationToken.None);

			NotCompletedTasks.Remove(taskViewModel);
		}

		public async Task UpTask(TaskViewModel taskViewModel)
		{
			int index = NotCompletedTasks.IndexOf(taskViewModel);

			if (index == 0)
			{
				// ничего не делаем, она уже и так сверху
			}
			else if (index > 0)
			{
				IHelpPenService helpPenService = ServiceLocator.Current.GetInstance<IHelpPenService>();

				TaskViewModel prevTaskViewModel = NotCompletedTasks[index - 1];

				int temp = prevTaskViewModel.Task.orderNumber;
				prevTaskViewModel.Task.orderNumber = taskViewModel.Task.orderNumber;
				taskViewModel.Task.orderNumber = temp;

				NotCompletedTasks[index - 1] = taskViewModel;
				NotCompletedTasks[index] = prevTaskViewModel;

				bool isNeedToUpdate = OrderTasksIfNeeded(NotCompletedTasks);

				if (isNeedToUpdate)
				{
					foreach (TaskViewModel item in NotCompletedTasks)
					{
						await helpPenService.ChangeTask(item.Task, CancellationToken.None);
					}
				}
				else
				{
					await helpPenService.ChangeTask(prevTaskViewModel.Task, CancellationToken.None);
					await helpPenService.ChangeTask(taskViewModel.Task, CancellationToken.None);
				}
			}
			else
			{
				Debug.Fail(@"Указана несуществующая задача.");
			}
		}

		#endregion

		#region Methods

		private static bool OrderTasksIfNeeded(ObservableCollection<TaskViewModel> tasks)
		{
			bool isGoodOrdering = true;

			for (int i = 0; i < tasks.Count - 1; i++)
			{
				if (TaskOrderComparer.Instance.Compare(tasks[i].Task, tasks[i + 1].Task) >= 0)
				{
					isGoodOrdering = false;
					break;
				}
			}

			if (!isGoodOrdering)
			{
				TaskViewModel[] copy = tasks.ToArray();
				tasks.Clear();
				int index = 0;
				foreach (TaskViewModel taskViewModel in copy.OrderBy(x => x.Task, TaskOrderComparer.Instance))
				{
					taskViewModel.Task.orderNumber = index;
					tasks.Add(taskViewModel);

					index++;
				}
			}

			return !isGoodOrdering;
		}

		private void OnTryAddNewTaskCommand()
		{
			Frame.Navigate(typeof(NewTaskPage));
		}

		private bool OnCanUpTaskCommandExecute()
		{
			return SelectedTask != null && NotCompletedTasks.IndexOf(SelectedTask) > 0;
		}

		private async void OnRemoveTaskCommandExecute()
		{
			await RemoveTask(_selectedTask);
		}

		private async void OnUpTaskCommandExecute()
		{
			await UpTask(_selectedTask);
		}

		#endregion
	}
}