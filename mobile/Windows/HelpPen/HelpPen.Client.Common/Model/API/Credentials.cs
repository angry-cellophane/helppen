namespace HelpPen.Client.Common.Model.API
{
	/// <summary>
	/// Удостоверение пользвателя.
	/// </summary>
	public class Credentials
	{
		/// <summary>
		/// Имя пользователя.
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Пароль.
		/// </summary>
		public string Password { get; set; }
	}
}
