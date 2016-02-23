using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using HelpPen.Client.Common.Model.API;
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
				var uri = new Uri(Settings.ServerUri, @"auth/login");

				NetworkCredential networkCredential = credentials.GetCredential(uri, null);

				var credentialsData = new CredentialsData(networkCredential.UserName, networkCredential.Password);

				HttpResponseMessage responseMessage =
					await httpClient.PostAsync(uri, new ObjectContent<CredentialsData>(credentialsData, _mediaTypeFormatter), cancellationToken);

				if (responseMessage.StatusCode == HttpStatusCode.OK)
				{
					IEnumerable<string> values;
					if (!responseMessage.Headers.TryGetValues(@"Set-Cookie", out values))
					{
						throw new AuthenticationException();
					}

					if (values.Count() == 1)
					{
						string cookie = values.ElementAt(0);

						KeyValuePair<string, string>[] parts =
							cookie
								.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries)
								.Select(x => x.Trim().Split(new[] {'='}, StringSplitOptions.RemoveEmptyEntries))
								.Select(x => new KeyValuePair<string, string>(x[0].ToLowerInvariant(), x[1]))
								.ToArray();

						KeyValuePair<string, string> idPart = parts.FirstOrDefault(x => x.Key == @"sessionid");

						if (!Equals(idPart, default(KeyValuePair<string, string>)))
						{
							string sessionId = idPart.Value;

							DateTimeOffset? expirationTime = null;

							KeyValuePair<string, string> expiresPart = parts.FirstOrDefault(x => x.Key == @"expires");

							if (!Equals(expiresPart, default(KeyValuePair<string, string>)))
							{
								DateTimeOffset result;
								if (DateTimeOffset.TryParse(expiresPart.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
								{
									expirationTime = result;
								}
							}

							return new Session(sessionId, DateTimeOffset.Now, expirationTime);
						}
						else
						{
							throw new AuthenticationException();
						}
					}
					else
					{
						throw new AuthenticationException();
					}
				}
				else if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
				{
					throw new InvalidCredentialException(@"Неверный логин или пароль.");
				}
				else
				{
					throw new AuthenticationException();
				}
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
		public Session CurrentSession { get; }

		/// <summary>
		///     Удостоверение пользователя.
		/// </summary>
		public ICredentials Credentials { get; }

		private class CredentialsData
		{
			public CredentialsData(string userName, string password)
			{
				UserName = userName;
				Password = password;
			}

			public string UserName { get; }

			public string Password { get; }
		}
	}
}