using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HelpPen.Client.Common.Model.API;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace HelpPen.Client.Common
{
	/// <summary>
	/// Сервис работы с HelpPen.
	/// </summary>
	public class HelpPenService : IHelpPenService
	{
		private readonly IAuthService _authService;
		private readonly Uri _serviceUri;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="HelpPenService"/>.
		/// </summary>
		/// <param name="authService">Сервис <see cref="IAuthService"/>.</param>
		public HelpPenService(IAuthService authService)
		{
			_authService = authService;
			_serviceUri = new Uri("helppen.atomnas.ru");
		}

		/// <summary>
		/// Добавляет новую задачу.
		/// </summary>
		/// <param name="task">Добавляемая задача.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит добавление новой задачи.</returns>
		/// <remarks>Идентификатор задачи <see cref="Model.API.Task.Id"/> должен быть сгенерирован на стороне клиента.</remarks>
		public async System.Threading.Tasks.Task AddTask(Model.API.Task task, CancellationToken cancellationToken)
		{
			await OnClient(
				async delegate (HttpClientHandler httpClientHandler, HttpClient httpClient)
				{
					await AddTaskInternal(task, httpClientHandler, httpClient, cancellationToken);
				},
				cancellationToken);
		}

		/// <summary>
		/// Изменяет существующую задачу задачу.
		/// </summary>
		/// <param name="task">Изменяемая задача.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит изменение существующей задачи.</returns>
		/// <remarks>Идентификатор задачи <see cref="Model.API.Task.Id"/> должен принадлежать ранее созданной задаче.</remarks>
		public async System.Threading.Tasks.Task ChangeTask(Model.API.Task task, CancellationToken cancellationToken)
		{
			await OnClient(
				async delegate (HttpClientHandler httpClientHandler, HttpClient httpClient)
				{
					await ChangeTaskInternal(task, httpClientHandler, httpClient, cancellationToken);
				},
				cancellationToken);
		}

		/// <summary>
		/// Возвращает перечень имеющихся задач.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит получение списка задач <see cref="Model.API.Task"/>.</returns>
		public async Task<ImmutableArray<Model.API.Task>> GetTasks(CancellationToken cancellationToken)
		{
			ImmutableArray<Model.API.Task> tasks;

			await OnClient(
				async delegate (HttpClientHandler httpClientHandler, HttpClient httpClient)
				{
					tasks = await GetTasksInternal(httpClientHandler, httpClient, cancellationToken);
				},
				cancellationToken);

			return tasks;
		}

		/// <summary>
		/// Удаляет задачу <paramref name="taskId"/>.
		/// </summary>
		/// <param name="taskId">Идентфикатор задачи, которую требуется удалить.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит удаление существующей задачи.</returns>
		public async System.Threading.Tasks.Task RemoveTask(Guid taskId, CancellationToken cancellationToken)
		{
			await OnClient(
				async delegate (HttpClientHandler httpClientHandler, HttpClient httpClient)
				{
					await RemoveTaskInternal(taskId, httpClientHandler, httpClient, cancellationToken);
				},
				cancellationToken);
		}

		private async System.Threading.Tasks.Task OnClient(Func<HttpClientHandler, HttpClient, System.Threading.Tasks.Task> action, CancellationToken cancellationToken)
		{
			await EnsureAuthentificated(cancellationToken);

			using (HttpClientHandler httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.CookieContainer.Add(_serviceUri, new Cookie(@"auth", _authService.CurrentSession.Id));
				httpClientHandler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

				using (HttpClient httpClient = new HttpClient(httpClientHandler))
				{
					await action(httpClientHandler, httpClient);
				}
			}
		}

		/// <summary>
		/// Убеждается в наличии текущей сессии работы с сервером. Если таковой нет, то проводит аутентификацию по имеющимуся удосстоверению пользователя.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="System.Threading.Tasks.Task"/>, в рамках которой происходит проверка.</returns>
		private async System.Threading.Tasks.Task EnsureAuthentificated(CancellationToken cancellationToken)
		{
			if (_authService.Credentials == null)
			{
				throw new InvalidOperationException("Необходимо хотя бы раз произвести явную процедуру входа.");
			}

			if (_authService.CurrentSession == null)
			{
				await _authService.Login(_authService.Credentials, cancellationToken);
			}
		}

		/// <summary>
		/// Добавляет новую задачу.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит получение списка задач <see cref="Model.API.Task"/>.</returns>
		private async System.Threading.Tasks.Task AddTaskInternal(Model.API.Task task, HttpClientHandler httpClientHandler, HttpClient httpClient, CancellationToken cancellationToken)
		{
			string json = JsonConvert.SerializeObject(task);

			byte[] data = Encoding.UTF8.GetBytes(json);

			using (HttpContent httpContent = new ByteArrayContent(data))
			{
				await httpClient.PostAsync(new Uri(_serviceUri, "Tasks"), httpContent, cancellationToken);
			}
		}

		/// <summary>
		/// Изменяет существующую задачу задачу.
		/// </summary>
		/// <param name="task">Изменяемая задача.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит изменение существующей задачи.</returns>
		/// <remarks>Идентификатор задачи <see cref="Model.API.Task.Id"/> должен принадлежать ранее созданной задаче.</remarks>
		private async System.Threading.Tasks.Task ChangeTaskInternal(Model.API.Task task, HttpClientHandler httpClientHandler, HttpClient httpClient, CancellationToken cancellationToken)
		{
			string json = JsonConvert.SerializeObject(task);

			byte[] data = Encoding.UTF8.GetBytes(json);

			using (HttpContent httpContent = new ByteArrayContent(data))
			{
				await httpClient.PutAsync(new Uri(_serviceUri, "Tasks/" + task.Id.ToString(@"n")), httpContent, cancellationToken);
			}
		}

		/// <summary>
		/// Возвращает перечень имеющихся задач.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит получение списка задач <see cref="Model.API.Task"/>.</returns>
		private async Task<ImmutableArray<Model.API.Task>> GetTasksInternal(HttpClientHandler httpClientHandler, HttpClient httpClient, CancellationToken cancellationToken)
		{
			using (Stream stream = await httpClient.GetStreamAsync(new Uri(_serviceUri, "Tasks")))
			{
				using (StreamReader streamReader = new StreamReader(stream))
				{
					using (JsonReader jsonReader = new JsonTextReader(streamReader))
					{
						JsonSerializer serializer = new JsonSerializer();

						Model.API.Task[] tasks = serializer.Deserialize<Model.API.Task[]>(jsonReader);

						return tasks.ToImmutableArray();
					}
				}
			}
		}

		/// <summary>
		/// Удаляет задачу <paramref name="taskId"/>.
		/// </summary>
		/// <param name="taskId">Идентфикатор задачи, которую требуется удалить.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит удаление существующей задачи.</returns>
		private async System.Threading.Tasks.Task RemoveTaskInternal(Guid taskId, HttpClientHandler httpClientHandler, HttpClient httpClient, CancellationToken cancellationToken)
		{
			await httpClient.DeleteAsync(new Uri(_serviceUri, "Tasks/" + taskId.ToString(@"n")), cancellationToken);
		}
	}
}
