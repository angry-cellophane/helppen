using System;

namespace HelpPen.Client.Common
{
	/// <summary>
	///     Исключение, иницирующееся при некорректно заданном имени пользователя или пароле.
	/// </summary>
	public class InvalidCredentialException : AuthenticationException
	{
		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="InvalidCredentialException" />.
		/// </summary>
		public InvalidCredentialException()
		{
		}

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="InvalidCredentialException" />.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		public InvalidCredentialException(string message) : base(message)
		{
		}

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="InvalidCredentialException" />.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="innerException">Внутренее исключение.</param>
		public InvalidCredentialException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}