using System;

namespace HelpPen.Client.Common
{
	/// <summary>
	///     Исключение, иницирующееся при процессе работы с веб-сервисами.
	/// </summary>
	public class WebServiceException : Exception
	{
		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="WebServiceException" />.
		/// </summary>
		public WebServiceException()
		{
		}

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="WebServiceException" />.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		public WebServiceException(string message) : base(message)
		{
		}

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="WebServiceException" />.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="innerException">Внутренее исключение.</param>
		public WebServiceException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}