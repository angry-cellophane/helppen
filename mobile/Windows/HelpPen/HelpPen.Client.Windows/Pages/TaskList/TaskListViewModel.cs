using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using Windows.UI.Popups;
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

		private readonly Dictionary<ObservableCollection<TaskViewModel>, DragInfo> _dragInfos;

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

			_dragInfos = new Dictionary<ObservableCollection<TaskViewModel>, DragInfo>();

			TryAddNewTaskCommand = new DelegateCommand(OnTryAddNewTaskCommand);
			UpTaskCommand = new DelegateCommand(OnUpTaskCommandExecute, OnCanUpTaskCommandExecute);
			EditTaskCommand = new DelegateCommand(OnEditTaskCommandExecuted, () => SelectedTask != null);
			RemoveTaskCommand = new DelegateCommand(OnRemoveTaskCommandExecute, () => SelectedTask != null);
			ReloadTasksCommand = new DelegateCommand(OnReloadTasksCommandExecuted);
			MoveToStashTaskCommand = new DelegateCommand(OnMoveToStashTaskCommandExecuted, () => SelectedTask != null);

			NotCompletedTasks = new ObservableCollection<TaskViewModel>();
			StashedTasks = new ObservableCollection<TaskViewModel>();

			NotCompletedTasks.CollectionChanged += OnTasksCollectionChanged;
			StashedTasks.CollectionChanged += OnTasksCollectionChanged;
		}

		#endregion

		#region Properties

		/// <summary>
		///     Команда редактирования задачи.
		/// </summary>
		public DelegateCommand EditTaskCommand { get; }

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
		///     Команда переноса задачи в кладовку.
		/// </summary>
		public DelegateCommand MoveToStashTaskCommand { get; }

		/// <summary>
		///     Перечень невыполненных задач текущего пользователя.
		/// </summary>
		public ObservableCollection<TaskViewModel> NotCompletedTasks { get; }

		/// <summary>
		///     Команда обновления списка задач.
		/// </summary>
		public DelegateCommand ReloadTasksCommand { get; }

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
				EditTaskCommand.RaiseCanExecuteChanged();
				RemoveTaskCommand.RaiseCanExecuteChanged();
				MoveToStashTaskCommand.CanExecute();
			}
		}

		/// <summary>
		///     Перечень чердачных задач текущего пользователя.
		/// </summary>
		public ObservableCollection<TaskViewModel> StashedTasks { get; }

		/// <summary>
		///     Команда начала добавления новой задачи.
		/// </summary>
		public DelegateCommand TryAddNewTaskCommand { get; }

		/// <summary>
		///     Команда поднятия уровня задачи.
		/// </summary>
		public DelegateCommand UpTaskCommand { get; }

		#endregion

		#region Public Methods and Operators

		public void BeginDragItems(ObservableCollection<TaskViewModel> tasks)
		{
			_dragInfos.Add(tasks, new DragInfo());
		}

		public async Task EndDragItems(ObservableCollection<TaskViewModel> tasks)
		{
			DragInfo dragInfo;
			if (_dragInfos.TryGetValue(tasks, out dragInfo))
			{
				HashSet<TaskViewModel> changedTasks = new HashSet<TaskViewModel>();

				_dragInfos.Remove(tasks);

				int order = tasks[dragInfo.NewIndex].Task.orderNumber;

				if (dragInfo.OldIndex < dragInfo.NewIndex)
				{
					for (int i = dragInfo.OldIndex + 1; i <= dragInfo.NewIndex; i++)
					{
						tasks[i].Task.orderNumber = tasks[i - 1].Task.orderNumber;

						changedTasks.Add(tasks[i]);
					}
				}
				else if (dragInfo.OldIndex > dragInfo.NewIndex)
				{
					for (int i = dragInfo.NewIndex; i <= dragInfo.OldIndex - 1; i++)
					{
						tasks[i].Task.orderNumber = tasks[i + 1].Task.orderNumber;

						changedTasks.Add(tasks[i]);
					}
				}
				else
				{
					throw new InvalidOperationException();
				}

				tasks[dragInfo.OldIndex].Task.orderNumber = order;

				changedTasks.Add(tasks[dragInfo.OldIndex]);

				IHelpPenService helpPenService = ServiceLocator.Current.GetInstance<IHelpPenService>();

				foreach (TaskViewModel item in NotCompletedTasks)
				{
					await helpPenService.ChangeTask(item.Task, CancellationToken.None);
				}
			}
		}

		public override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			IsWorking = true;

			try
			{
				await LoadTasks();
			}
			finally
			{
				IsWorking = false;
			}
		}

		public async Task RemoveTask(TaskViewModel taskViewModel)
		{
			var dialog = new MessageDialog($"Действительно удалить задачу \"{taskViewModel.Text}\"?");

			UICommand okCommand = new UICommand("Ок") { Id = 0 };
			UICommand cancelCommand = new UICommand("Отмена") { Id = 1 };

			dialog.Commands.Add(okCommand);
			dialog.Commands.Add(cancelCommand);

			IUICommand selectedCommand = await dialog.ShowAsync();

			if (selectedCommand == okCommand)
			{
				IHelpPenService helpPenService = ServiceLocator.Current.GetInstance<IHelpPenService>();

				await helpPenService.RemoveTask(taskViewModel.Task.id, CancellationToken.None);

				NotCompletedTasks.Remove(taskViewModel);
				StashedTasks.Remove(taskViewModel);
			}
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

				SelectedTask = taskViewModel;
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

		private async Task LoadTasks()
		{
			ImmutableArray<Common.Model.API.Task> tasks = await _helpPenService.GetTasks(CancellationToken.None);

			NotCompletedTasks.Clear();

			foreach (
				Common.Model.API.Task task in
					tasks.Where(x => x.state == TaskState.NOT_COMPLITED).OrderBy(x => x, TaskOrderComparer.Instance))
			{
				NotCompletedTasks.Add(new TaskViewModel(task));
			}

			StashedTasks.Clear();
			foreach (
				Common.Model.API.Task task in tasks.Where(x => x.state == TaskState.STASH).OrderBy(x => x, TaskOrderComparer.Instance))
			{
				StashedTasks.Add(new TaskViewModel(task));
			}

			OrderTasksIfNeeded(NotCompletedTasks);
			OrderTasksIfNeeded(StashedTasks);
		}

		private bool OnCanUpTaskCommandExecute()
		{
			return SelectedTask != null && NotCompletedTasks.IndexOf(SelectedTask) > 0;
		}

		private void OnEditTaskCommandExecuted()
		{
			Frame.Navigate(typeof(NewAndEditTaskPage), SelectedTask);
		}

		private async void OnMoveToStashTaskCommandExecuted()
		{
			IsWorking = true;

			try
			{
				await StashTask(_selectedTask);
			}
			finally
			{
				IsWorking = false;
			}
		}

		private async void OnReloadTasksCommandExecuted()
		{
			IsWorking = true;

			try
			{
				await LoadTasks();
			}
			finally
			{
				IsWorking = false;
			}
		}

		private async void OnRemoveTaskCommandExecute()
		{
			await RemoveTask(_selectedTask);
		}

		private void OnTasksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ObservableCollection<TaskViewModel> tasks = (ObservableCollection<TaskViewModel>)sender;

			DragInfo dragInfo;
			if (_dragInfos.TryGetValue(tasks, out dragInfo))
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					Debug.Assert(ReferenceEquals((TaskViewModel)e.NewItems[0], dragInfo.Task));
					dragInfo.NewIndex = e.NewStartingIndex;
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					dragInfo.Task = (TaskViewModel)e.OldItems[0];
					dragInfo.OldIndex = e.OldStartingIndex;
				}
			}
		}

		private void OnTryAddNewTaskCommand()
		{
			Frame.Navigate(typeof(NewAndEditTaskPage));
		}

		private async void OnUpTaskCommandExecute()
		{
			IsWorking = true;

			try
			{
				await UpTask(_selectedTask);
			}
			finally
			{
				IsWorking = false;
			}
		}

		private async Task StashTask(TaskViewModel taskViewModel)
		{
			IHelpPenService helpPenService = ServiceLocator.Current.GetInstance<IHelpPenService>();

			TaskViewModel selectedTask = SelectedTask;

			selectedTask.Task.state = TaskState.STASH;

			await helpPenService.ChangeTask(taskViewModel.Task, CancellationToken.None);

			NotCompletedTasks.Remove(selectedTask);
			StashedTasks.Add(selectedTask);

			bool isNeedToUpdate = OrderTasksIfNeeded(StashedTasks);

			if (isNeedToUpdate)
			{
				foreach (TaskViewModel item in StashedTasks)
				{
					await helpPenService.ChangeTask(item.Task, CancellationToken.None);
				}
			}
		}

		#endregion

		#region Nested Types

		private class DragInfo
		{
			#region Fields

			/// <summary>
			///     Старый Номер элемента.
			/// </summary>
			public int OldIndex;

			public TaskViewModel Task;

			/// <summary>
			///     Новый номер элемента.
			/// </summary>
			public int NewIndex;

			#endregion
		}

		#endregion
	}
}