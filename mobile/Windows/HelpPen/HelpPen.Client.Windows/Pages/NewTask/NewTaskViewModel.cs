using System;
using System.Threading;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using HelpPen.Client.Common;
using HelpPen.Client.Common.Model.API;
using HelpPen.Client.Common.MVVM;

using Prism.Commands;

namespace HelpPen.Client.Windows.Pages.NewTask
{
	/// <summary>
	///     Модель представления для <see cref="NewTaskPage" />.
	/// </summary>
	internal sealed class NewTaskViewModel : PageViewModel
	{
		#region Fields

		private readonly IHelpPenService _helpPenService;

		private bool _isWorking;

		private string _newTaskText;

		#endregion

		#region Constructors and Destructors

		public NewTaskViewModel(Frame frame, IHelpPenService helpPenService)
			: base(frame)
		{
			_helpPenService = helpPenService;

			AcceptCommand = new DelegateCommand(OnAcceptCommandExecute, () => !string.IsNullOrWhiteSpace(NewTaskText));
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
		public string NewTaskText
		{
			get
			{
				return _newTaskText;
			}
			set
			{
				SetPropertyValue(_newTaskText, value, newValue => _newTaskText = newValue);
				AcceptCommand.RaiseCanExecuteChanged();
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
				Task task = new Task { text = NewTaskText };

				await _helpPenService.AddTask(task, CancellationToken.None);

				NewTaskText = null;
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