using System;

namespace HelpPen.Client.Common.Model.API
{
	/// <summary>
	/// Задача (API уровень).
	/// </summary>
	public sealed class Task
	{
		/// <summary>
		/// Идентификатор задачи.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Текстовое содержание задачи.
		/// </summary>
		public string text { get; set; }

		/// <summary>
		/// Состояние задачи.
		/// </summary>
		public TaskState State { get; set; }
	}
}
