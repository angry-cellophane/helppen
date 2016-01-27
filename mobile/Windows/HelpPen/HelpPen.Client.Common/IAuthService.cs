﻿using HelpPen.Client.Common.Model.API;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HelpPen.Client.Common
{
	/// <summary>
	/// Сервис аутентификации.
	/// </summary>
	public interface IAuthService
	{
		/// <summary>
		/// Осуществляет процедуру аутентификации по заданным <see cref="ICredentials"/>.
		/// </summary>
		/// <param name="credentials">Удостоврение пользователя.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Возвращает задачу <see cref="Task{TResult}"/>,  в рамках которой происходит получение <see cref="Session"/>.</returns>
		/// <exception cref="System.Security.Authentication.AuthenticationException">Инициируется при невозможности получить сессию <see cref="Session"/> для указанного удостоверения <paramref name="credentials"/>.</exception>
		Task<Session> Login(ICredentials credentials, CancellationToken cancellationToken);

		/// <summary>
		/// Осуществляет процедуру завершения актуальности сессии работы пользователя по его инициативе.
		/// </summary>
		/// <param name="session">Сессия работы с пользователем.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Возвращает задачу <see cref="Task{TResult}"/>,  в рамках которой происходит завершение работы.</returns>
		System.Threading.Tasks.Task Logout(Session session, CancellationToken cancellationToken);

		/// <summary>
		/// Содержит текущую сессию работы системы или <c>null</c>, если не была выполнена процедура входа.
		/// </summary>
		Session CurrentSession { get; }

		/// <summary>
		/// Удостоверение пользователя.
		/// </summary>
		ICredentials Credentials { get; }
	}
}
