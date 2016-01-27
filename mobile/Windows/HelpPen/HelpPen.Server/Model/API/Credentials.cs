namespace HelpPen.Server.Model.API
{
	/// <summary>
	/// Учетные данные для схем проверки подлинности на основе пароля.
	/// </summary>
	public class Credentials
	{
		/// <summary>
		/// Имя пользователя, связанное с учетными данными.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Пароль пользователя, связанный с учетными данными.
		/// </summary>
		public string Password { get; set; }
	}
}