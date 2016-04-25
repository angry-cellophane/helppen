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
		public TaskState state { get; set; }

		public int ownerId { get; set; }

		public int orderNumber { get; set; }

		public DateTimeOffset creationDateTime { get; set; }

		public DateTimeOffset lastChangeDateTime { get; set; }
	}
}
