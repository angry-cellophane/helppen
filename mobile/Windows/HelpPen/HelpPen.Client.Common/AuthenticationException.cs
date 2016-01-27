using System;

namespace HelpPen.Client.Common
{
	/// <summary>
	///     Исключение, иницирующееся при процессе аутентификации.
	/// </summary>
	public class AuthenticationException : Exception
	{
		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="AuthenticationException" />.
		/// </summary>
		public AuthenticationException()
		{
		}

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="AuthenticationException" />.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		public AuthenticationException(string message) : base(message)
		{
		}

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="AuthenticationException" />.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="innerException">Внутренее исключение.</param>
		public AuthenticationException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}