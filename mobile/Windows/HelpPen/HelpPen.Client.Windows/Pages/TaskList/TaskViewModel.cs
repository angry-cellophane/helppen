using HelpPen.Client.Common.Model.API;
using HelpPen.Client.Common.MVVM;

namespace HelpPen.Client.Windows.Pages.TaskList
{
	/// <summary>
	///     Модель представления для отдельной задачи.
	/// </summary>
	internal sealed class TaskViewModel : ViewModel
	{
		#region Constructors and Destructors

		/// <summary>
		///     Иницаилизирует новый экземпляр класса <see cref="TaskViewModel" />.
		/// </summary>
		/// <param name="task">Задача.</param>
		public TaskViewModel(Task task)
		{
			Task = task;
		}

		#endregion

		#region Properties

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

		#endregion
	}
}