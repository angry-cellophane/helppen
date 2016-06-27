using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using HelpPen.Client.Common.Model.API;

using Task = System.Threading.Tasks.Task;

namespace HelpPen.Client.Common.Tracing
{
	/// <summary>
	///     Трассируемый сервис аутентификации.
	/// </summary>
	public sealed class TraceableAuthService : IAuthService
	{
		#region Fields

		private readonly IAuthService _authService;

		#endregion

		#region Constructors and Destructors

		public TraceableAuthService(IAuthService authService)
		{
			_authService = authService;
		}

		#endregion

		#region Properties

		/// <summary>
		///     Удостоверение пользователя.
		/// </summary>
		public ICredentials Credentials
		{
			get
			{
				return _authService.Credentials;
			}
		}

		/// <summary>
		///     Содержит текущую сессию работы системы или <c>null</c>, если не была выполнена процедура входа.
		/// </summary>
		public Session CurrentSession
		{
			get
			{
				return _authService.CurrentSession;
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Осуществляет процедуру аутентификации по заданным <see cref="ICredentials" />.
		/// </summary>
		/// <param name="credentials">Удостоврение пользователя.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Возвращает задачу <see cref="Task{TResult}" />,  в рамках которой происходит получение <see cref="Session" />.</returns>
		/// <exception cref="AuthenticationException">
		///     Инициируется при невозможности получить сессию <see cref="Session" /> для
		///     указанного удостоверения <paramref name="credentials" />.
		/// </exception>
		public async Task<Session> Login(ICredentials credentials, CancellationToken cancellationToken)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();

			Session session;

			try
			{
				session = await _authService.Login(credentials, cancellationToken);
			}
			finally
			{
				stopwatch.Stop();

				AuthEventSource.Instance.LoginTime(AuthService.AUTH_TOKEN_SERVICE_URI, (uint)stopwatch.ElapsedMilliseconds);
			}

			return session;
		}

		/// <summary>
		///     Осуществляет процедуру завершения актуальности сессии работы пользователя по его инициативе.
		/// </summary>
		/// <param name="session">Сессия работы с пользователем.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Возвращает задачу <see cref="Task{TResult}" />,  в рамках которой происходит завершение работы.</returns>
		public Task Logout(Session session, CancellationToken cancellationToken)
		{
			return _authService.Logout(session, cancellationToken);
		}

		#endregion
	}
}