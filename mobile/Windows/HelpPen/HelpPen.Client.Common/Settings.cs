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
			ServerUri = new Uri(@"http://dev.helppen.com");
		}

		/// <summary>
		///     Адрес API сервера.
		/// </summary>
		public static Uri ServerUri { get; }
	}
}