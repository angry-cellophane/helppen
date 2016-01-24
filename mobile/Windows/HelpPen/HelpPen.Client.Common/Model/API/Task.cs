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
		/// Краткое содержание задачи.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Текстовое содержание задачи.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Состояние задачи.
		/// </summary>
		public TaskState State { get; set; }
	}
}
