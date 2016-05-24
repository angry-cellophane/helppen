using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Input;

using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

using HelpPen.Client.Common;
using HelpPen.Client.Common.Model.API;
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

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="TaskListViewModel" />.
		/// </summary>
		public TaskListViewModel(IHelpPenService helpPenService)
		{
			_helpPenService = helpPenService;

			AddNewTaskCommand = new DelegateCommand(OnAddNewTaskCommandExecute, () => !string.IsNullOrWhiteSpace(NewTaskText));

			Tasks = new ObservableCollection<TaskViewModel>();
		}

		#endregion

		#region Properties

		/// <summary>
		///     Команда добавления новой задачи.
		/// </summary>
		public DelegateCommand AddNewTaskCommand { get; }

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
				AddNewTaskCommand.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		///     Перечень задач текущего пользователя.
		/// </summary>
		public ObservableCollection<TaskViewModel> Tasks { get; }

		#endregion

		#region Public Methods and Operators

		public override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			IsWorking = true;

			try
			{
				ImmutableArray<Task> tasks = await _helpPenService.GetTasks(CancellationToken.None);

				Tasks.Clear();
				foreach (Task task in tasks.OrderBy(x => x.orderNumber))
				{
					Tasks.Add(new TaskViewModel(this, task));
				}

				OrderTasksIfNeeded(Tasks);
			}
			finally
			{
				IsWorking = false;
			}
		}

		private static bool OrderTasksIfNeeded(ObservableCollection<TaskViewModel> tasks)
		{
			bool isGoodOrdering = true;

			for (int i = 0; i < tasks.Count - 1; i++)
			{
				if (tasks[i].Task.orderNumber >= tasks[i + 1].Task.orderNumber)
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
				foreach (TaskViewModel taskViewModel in copy.OrderBy(x => x.Task.orderNumber))
				{
					taskViewModel.Task.orderNumber = index;
					tasks.Add(taskViewModel);

					index++;
				}
			}

			return !isGoodOrdering;
		}

		#endregion

		#region Methods

		/// <summary>
		///     Обработчик выполнения команды <see cref="AddNewTaskCommand" />.
		/// </summary>
		private async void OnAddNewTaskCommandExecute()
		{
			IsWorking = true;

			try
			{
				Task task = new Task { text = NewTaskText };

				await _helpPenService.AddTask(task, CancellationToken.None);

				Tasks.Add(new TaskViewModel(this, task));

				NewTaskText = null;
			}
			finally
			{
				IsWorking = false;
			}
		}

		#endregion

		public async System.Threading.Tasks.Task UpTask(TaskViewModel taskViewModel)
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

		public async System.Threading.Tasks.Task RemoveTask(TaskViewModel taskViewModel)
		{
			IHelpPenService helpPenService = ServiceLocator.Current.GetInstance<IHelpPenService>();

			await helpPenService.RemoveTask(taskViewModel.Task.Id, CancellationToken.None);

			Tasks.Remove(taskViewModel);
		}
	}
}