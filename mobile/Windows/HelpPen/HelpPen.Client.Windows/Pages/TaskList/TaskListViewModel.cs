using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

using Windows.UI.Xaml.Navigation;

using HelpPen.Client.Common;
using HelpPen.Client.Common.Model.API;
using HelpPen.Client.Common.MVVM;

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

		private ObservableCollection<Task> _tasks;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="TaskListViewModel" />.
		/// </summary>
		public TaskListViewModel(IHelpPenService helpPenService)
		{
			_helpPenService = helpPenService;

			AddNewTaskCommand = new DelegateCommand(OnAddNewTaskCommandExecute, () => !string.IsNullOrWhiteSpace(NewTaskText));

			// BUG: Без первоначального создания пустого списка в адльнейшем привязка игнориурет изменения значения свойства Tasks...
			Tasks = new ObservableCollection<Task>();
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
		public ObservableCollection<Task> Tasks
		{
			get
			{
				return _tasks;
			}
			set
			{
				SetPropertyValue(_tasks, value, newValue => _tasks = newValue);
			}
		}

		#endregion

		#region Public Methods and Operators

		public override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			IsWorking = true;

			try
			{
				ImmutableArray<Task> tasks = await _helpPenService.GetTasks(CancellationToken.None);

				Tasks = new ObservableCollection<Task>(tasks);
			}
			finally
			{
				IsWorking = false;
			}
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

				Tasks.Add(task);

				NewTaskText = null;
			}
			finally
			{
				IsWorking = false;
			}
		}

		#endregion
	}
}