using System.Threading;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using HelpPen.Client.Common;
using HelpPen.Client.Common.Model.API;
using HelpPen.Client.Common.MVVM;
using HelpPen.Client.Windows.Pages.TaskList;

using Prism.Commands;

namespace HelpPen.Client.Windows.Pages.NewTask
{
	/// <summary>
	///     Модель представления для <see cref="NewAndEditTaskPage" />.
	/// </summary>
	internal sealed class NewAndEditTaskViewModel : PageViewModel
	{
		#region Fields

		private readonly IHelpPenService _helpPenService;

		private TaskViewModel _existsTask;

		private bool _isWorking;

		private string _taskText;

		#endregion

		#region Constructors and Destructors

		public NewAndEditTaskViewModel(Frame frame, IHelpPenService helpPenService)
			: base(frame)
		{
			_helpPenService = helpPenService;

			AcceptCommand = new DelegateCommand(OnAcceptCommandExecute, () => !string.IsNullOrWhiteSpace(TaskText));
			CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
		}

		#endregion

		#region Properties

		/// <summary>
		///     Команда подтверждения добавления новой задачи.
		/// </summary>
		public DelegateCommand AcceptCommand { get; }

		/// <summary>
		///     Команда отмены добавления новой задачи.
		/// </summary>
		public DelegateCommand CancelCommand { get; }

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
		public string TaskText
		{
			get
			{
				return _taskText;
			}
			set
			{
				SetPropertyValue(_taskText, value, newValue => _taskText = newValue);
				AcceptCommand.RaiseCanExecuteChanged();
			}
		}

		#endregion

		#region Public Methods and Operators

		public override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			_existsTask = e.Parameter as TaskViewModel;

			if (_existsTask != null)
			{
				TaskText = _existsTask.Text;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		///     Обработчик выполнения команды <see cref="AcceptCommand" />.
		/// </summary>
		private async void OnAcceptCommandExecute()
		{
			IsWorking = true;

			try
			{
				if (_existsTask != null)
				{
					_existsTask.Task.text = TaskText;

					await _helpPenService.ChangeTask(_existsTask.Task, CancellationToken.None);

					_existsTask.Text = TaskText;
				}
				else
				{
					Task task = new Task { text = TaskText };

					await _helpPenService.AddTask(task, CancellationToken.None);

					TaskText = null;
				}
			}
			finally
			{
				IsWorking = false;
			}

			Frame.GoBack();
		}

		private void OnCancelCommandExecuted()
		{
			Frame.GoBack();
		}

		#endregion
	}
}