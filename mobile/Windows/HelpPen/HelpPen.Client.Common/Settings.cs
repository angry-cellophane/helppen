using System;

namespace HelpPen.Client.Common
{
	/// <summary>
	///     Общие настройки приложения.
	/// </summary>
	public static class Settings
	{
		static Settings()
		{
			ServerUri = new Uri(@"http://main.atomnas.ru:8963/api/");
		}

		/// <summary>
		///     Адрес сервера.
		/// </summary>
		public static Uri ServerUri { get; }
	}
}