using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HelpPen.Client.Common.Model.API
{
	/// <summary>
	/// Состояние задачи.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TaskState
	{
		/// <summary>
		/// Задача не завершена.
		/// </summary>
		NOT_COMPLETED = 0,

		/// <summary>
		/// Задача завершена.
		/// </summary>
		Completed = 1,

		/// <summary>
		/// Задача отложена.
		/// </summary>
		Stash = 2
	}
}
