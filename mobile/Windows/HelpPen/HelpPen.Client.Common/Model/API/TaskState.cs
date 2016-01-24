namespace HelpPen.Client.Common.Model.API
{
	/// <summary>
	/// Состояние задачи.
	/// </summary>
	public enum TaskState
	{
		/// <summary>
		/// Задача не завершена.
		/// </summary>
		NotCompleted = 1,

		/// <summary>
		/// Задача завершена.
		/// </summary>
		Completed = 2,

		/// <summary>
		/// Задача отложена.
		/// </summary>
		Stash = 3
	}
}
