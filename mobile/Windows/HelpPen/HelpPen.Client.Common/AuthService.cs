using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using HelpPen.Client.Common.Model.API;
using Newtonsoft.Json;
using HttpClient = System.Net.Http.HttpClient;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;
using HttpStatusCode = System.Net.HttpStatusCode;
using Task = System.Threading.Tasks.Task;

namespace HelpPen.Client.Common
{
	/// <summary>
	///     Сервис аутентификации.
	/// </summary>
	public class AuthService : IAuthService
	{
		private static readonly MediaTypeFormatter _mediaTypeFormatter;

		/// <summary>
		///     Инициализирует статические поля класса <see cref="AuthService" />.
		/// </summary>
		static AuthService()
		{
			_mediaTypeFormatter = new JsonMediaTypeFormatter();
		}

		/// <summary>
		///     Осуществляет процедуру аутентификации по заданным <see cref="ICredentials" />.
		/// </summary>
		/// <param name="credentials">Удостоврение пользователя.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Возвращает задачу <see cref="Task{TResult}" />,  в рамках которой происходит получение <see cref="Session" />.</returns>
		/// <exception cref="AuthenticationException">
		///     Инициируется при невозможности получить сессию
		///     <see cref="Session" /> для указанного удостоверения <paramref name="credentials" />.
		/// </exception>
		public async Task<Session> Login(ICredentials credentials, CancellationToken cancellationToken)
		{
			using (var httpClient = new HttpClient())
			{
				var uri = new Uri(Settings.ServerUri, @"auth/token");

				NetworkCredential networkCredential = credentials.GetCredential(uri, null);

				var credentialsData = new CredentialsData(networkCredential.UserName, networkCredential.Password);

				HttpContent content = new ObjectContent<CredentialsData>(credentialsData, _mediaTypeFormatter);

				content.Headers.ContentType = MediaTypeHeaderValue.Parse(@"application/json");

				HttpResponseMessage responseMessage =
					await
						httpClient.PostAsync(uri, content,
							cancellationToken);

				string responceContent = await responseMessage.Content.ReadAsStringAsync();

				if (responseMessage.StatusCode == HttpStatusCode.OK)
				{
					AuthToken authToken = JsonConvert.DeserializeObject<AuthToken>(responceContent);

					if (!string.IsNullOrWhiteSpace(authToken.token))
					{
						CurrentSession = new Session(authToken.token, DateTimeOffset.Now, default(DateTimeOffset?));
						Credentials = credentials;

						return CurrentSession;
					}

					throw new AuthenticationException();
				}

				if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
				{
					string message = @"Неверный логин или пароль: " + responceContent;

					message = message.Trim(':', ' ');

					throw new InvalidCredentialException(message);
				}

				throw new AuthenticationException(responceContent);
			}
		}

		/// <summary>
		///     Осуществляет процедуру завершения актуальности сессии работы пользователя по его инициативе.
		/// </summary>
		/// <param name="session">Сессия работы с пользователем.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Возвращает задачу <see cref="Task{TResult}" />,  в рамках которой происходит завершение работы.</returns>
		public Task Logout(Session session, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///     Содержит текущую сессию работы системы или <c>null</c>, если не была выполнена процедура входа.
		/// </summary>
		public Session CurrentSession { get; private set; }

		/// <summary>
		///     Удостоверение пользователя.
		/// </summary>
		public ICredentials Credentials { get; private set; }

		private class CredentialsData
		{
			public CredentialsData(string username, string password)
			{
				this.username = username;
				this.password = password;
			}

			public string username { get; }

			public string password { get; }
		}

		private class AuthToken
		{
			public string token { get; set; }
		}
	}
}