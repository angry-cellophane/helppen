//using System.Collections.ObjectModel;
//using System.Threading;
//using System.Windows.Input;

//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Navigation;

//using HelpPen.Client.Common.Model.API;
//using HelpPen.Client.Common.MVVM;

//using Prism.Commands;

//namespace HelpPen.Client.Windows.Pages.TaskList
//{
//	public class TestVM : PageViewModel
//	{
//		#region Static Fields

//		/// <summary>
//		///     Свойство зависимостией для <see cref="Tasks" />.
//		/// </summary>
//		public static readonly DependencyProperty TasksProperty = DependencyProperty.Register(
//			"Tasks",
//			typeof(ObservableCollection<Task>),
//			typeof(TestVM),
//			new PropertyMetadata(null));

//		/// <summary>
//		///     Свойство зависимостией для <see cref="IsWorking" />.
//		/// </summary>
//		public static readonly DependencyProperty IsWorkingProperty = DependencyProperty.Register(
//			"IsWorking",
//			typeof(bool),
//			typeof(TestVM),
//			new PropertyMetadata(false));

//		/// <summary>
//		///     Свойство зависимостией для <see cref="NewTaskText" />.
//		/// </summary>
//		public static readonly DependencyProperty NewTaskTextProperty = DependencyProperty.Register(
//			"NewTaskText",
//			typeof(string),
//			typeof(TestVM),
//			new PropertyMetadata(null));

//		#endregion

//		public TestVM()
//		{
//			AddNewTaskCommand = new DelegateCommand(OnAddNewTaskCommandExecute, () => !string.IsNullOrWhiteSpace(NewTaskText));
//		}

//		#region Properties

//		/// <summary>
//		///     Команда добавления новой задачи.
//		/// </summary>
//		public ICommand AddNewTaskCommand { get; }

//		/// <summary>
//		///     Содержит признак занятости выполнением той или иной операции.
//		/// </summary>
//		public bool IsWorking
//		{
//			get
//			{
//				return (bool)GetValue(IsWorkingProperty);
//			}
//			set
//			{
//				SetValue(IsWorkingProperty, value);
//			}
//		}

//		/// <summary>
//		///     Текст новой, создаваемой, задачи.
//		/// </summary>
//		public string NewTaskText
//		{
//			get
//			{
//				return (string)GetValue(NewTaskTextProperty);
//			}
//			set
//			{
//				SetValue(NewTaskTextProperty, value);
//			}
//		}

//		/// <summary>
//		///     Перечень задач текущего пользователя.
//		/// </summary>
//		public ObservableCollection<Task> Tasks
//		{
//			get
//			{
//				return (ObservableCollection<Task>)GetValue(TasksProperty);
//			}
//			set
//			{
//				SetValue(TasksProperty, value);
//			}
//		}

//		#endregion
//		public override async void OnNavigatedTo(NavigationEventArgs e)
//		{
//			base.OnNavigatedTo(e);

//			IsWorking = true;

//			try
//			{
//				//ImmutableArray<Task> tasks = await _helpPenService.GetTasks(CancellationToken.None);

//				//tasks.Clear();

//				//foreach (Task task in tasks)
//				//{
//				//	Tasks.Add(task);
//				//}

//				Tasks = new ObservableCollection<Task>();
//				Tasks.Add(new Task() { text = "asd" });
//			}
//			finally
//			{
//				IsWorking = false;
//			}
//		}

//		#region Methods

//		/// <summary>
//		///     Обработчик выполнения команды <see cref="AddNewTaskCommand" />.
//		/// </summary>
//		private async void OnAddNewTaskCommandExecute()
//		{
//			IsWorking = true;

//			try
//			{
//				Task task = new Task { text = NewTaskText };

//				//await _helpPenService.AddTask(task, CancellationToken.None);

//				Tasks.Add(task);
//			}
//			finally
//			{
//				IsWorking = false;
//			}
//		}

//		#endregion
//	}
//}