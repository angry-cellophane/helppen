using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Windows.UI.Xaml.Navigation;

using HelpPen.Client.Common;
using HelpPen.Client.Common.MVVM;

using Microsoft.Practices.ServiceLocation;

using Prism.Commands;

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

		private string _newTaskText;

		private TaskViewModel _selectedTask;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="TaskListViewModel" />.
		/// </summary>
		public TaskListViewModel(IHelpPenService helpPenService)
		{
			_helpPenService = helpPenService;

			BeginAddNewTaskCommand = new DelegateCommand(OnBeginAddNewTaskCommand);
			CommitAddNewTaskCommand = new DelegateCommand(OnCommitAddNewTaskCommandExecute, () => !string.IsNullOrWhiteSpace(NewTaskText));
			UpTaskCommand = new DelegateCommand(OnUpTaskCommandExecute, () => SelectedTask != null);
			RemoveTaskCommand = new DelegateCommand(OnRemoveTaskCommandExecute, () => SelectedTask != null);

			Tasks = new ObservableCollection<TaskViewModel>();
		}

		#endregion

		#region Properties

		/// <summary>
		///     Команда начала добавления новой задачи.
		/// </summary>
		public DelegateCommand BeginAddNewTaskCommand { get; }

		/// <summary>
		///     Команда подтверждения добавления новой задачи.
		/// </summary>
		public DelegateCommand CommitAddNewTaskCommand { get; }

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
		///     Текст новой, создаваемой, задачи.
		/// </summary>
		public string NewTaskText
		{
			get
			{
				return _newTaskText;
			}
			set
			{
				SetPropertyValue(_newTaskText, value, newValue => _newTaskText = newValue);
				CommitAddNewTaskCommand.RaiseCanExecuteChanged();
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
		///     Перечень задач текущего пользователя.
		/// </summary>
		public ObservableCollection<TaskViewModel> Tasks { get; }

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

				Tasks.Clear();
				foreach (Common.Model.API.Task task in tasks.OrderBy(x => x, TaskOrderComparer.Instance))
				{
					Tasks.Add(new TaskViewModel(task));
				}

				OrderTasksIfNeeded(Tasks);
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

			Tasks.Remove(taskViewModel);
		}

		public async Task UpTask(TaskViewModel taskViewModel)
		{
			int index = Tasks.IndexOf(taskViewModel);

			if (index == 0)
			{
				// ничего не делаем, она уже и так сверху
			}
			else if (index > 0)
			{
				IHelpPenService helpPenService = ServiceLocator.Current.GetInstance<IHelpPenService>();

				TaskViewModel prevTaskViewModel = Tasks[index - 1];

				int temp = prevTaskViewModel.Task.orderNumber;
				prevTaskViewModel.Task.orderNumber = taskViewModel.Task.orderNumber;
				taskViewModel.Task.orderNumber = temp;

				Tasks[index - 1] = taskViewModel;
				Tasks[index] = prevTaskViewModel;

				bool isNeedToUpdate = OrderTasksIfNeeded(Tasks);

				if (isNeedToUpdate)
				{
					foreach (TaskViewModel item in Tasks)
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

		private void OnBeginAddNewTaskCommand()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///     Обработчик выполнения команды <see cref="CommitAddNewTaskCommand" />.
		/// </summary>
		private async void OnCommitAddNewTaskCommandExecute()
		{
			IsWorking = true;

			try
			{
				Common.Model.API.Task task = new Common.Model.API.Task { text = NewTaskText };

				await _helpPenService.AddTask(task, CancellationToken.None);

				Tasks.Add(new TaskViewModel(task));

				NewTaskText = null;
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

		private async void OnUpTaskCommandExecute()
		{
			await UpTask(_selectedTask);
		}

		#endregion
	}
}