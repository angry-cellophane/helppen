using System;
using System.Collections.Immutable;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace HelpPen.Client.Common
{
	/// <summary>
	///     Сервис работы с HelpPen.
	/// </summary>
	public class HelpPenService : IHelpPenService
	{
		#region Fields

		private readonly IAuthService _authService;

		private readonly Uri _serviceUri;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="HelpPenService" />.
		/// </summary>
		/// <param name="authService">Сервис <see cref="IAuthService" />.</param>
		public HelpPenService(IAuthService authService)
		{
			_authService = authService;
			_serviceUri = Settings.ServerUri;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Добавляет новую задачу.
		/// </summary>
		/// <param name="task">Добавляемая задача.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}" />, в рамках которой происходит добавление новой задачи.</returns>
		/// <remarks>Идентификатор задачи <see cref="Model.API.Task.Id" /> должен быть сгенерирован на стороне клиента.</remarks>
		public async Task AddTask(Model.API.Task task, CancellationToken cancellationToken)
		{
			await OnClient(
				async delegate(HttpClientHandler httpClientHandler, HttpClient httpClient)
					{
						await AddTaskInternal(task, httpClientHandler, httpClient, cancellationToken);
					},
				cancellationToken);
		}

		/// <summary>
		///     Изменяет существующую задачу задачу.
		/// </summary>
		/// <param name="task">Изменяемая задача.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}" />, в рамках которой происходит изменение существующей задачи.</returns>
		/// <remarks>Идентификатор задачи <see cref="Model.API.Task.Id" /> должен принадлежать ранее созданной задаче.</remarks>
		public async Task ChangeTask(Model.API.Task task, CancellationToken cancellationToken)
		{
			await OnClient(
				async delegate(HttpClientHandler httpClientHandler, HttpClient httpClient)
					{
						await ChangeTaskInternal(task, httpClientHandler, httpClient, cancellationToken);
					},
				cancellationToken);
		}

		/// <summary>
		///     Возвращает перечень имеющихся задач.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>
		///     Задача <see cref="Task{TResult}" />, в рамках которой происходит получение списка задач
		///     <see cref="Model.API.Task" />.
		/// </returns>
		public async Task<ImmutableArray<Model.API.Task>> GetTasks(CancellationToken cancellationToken)
		{
			ImmutableArray<Model.API.Task> tasks = ImmutableArray<Model.API.Task>.Empty;

			await OnClient(
				async delegate(HttpClientHandler httpClientHandler, HttpClient httpClient)
					{
						tasks = await GetTasksInternal(httpClientHandler, httpClient, cancellationToken);
					},
				cancellationToken);

			return tasks;
		}

		/// <summary>
		///     Удаляет задачу <paramref name="taskId" />.
		/// </summary>
		/// <param name="taskId">Идентфикатор задачи, которую требуется удалить.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}" />, в рамках которой происходит удаление существующей задачи.</returns>
		public async Task RemoveTask(Guid taskId, CancellationToken cancellationToken)
		{
			await OnClient(
				async delegate(HttpClientHandler httpClientHandler, HttpClient httpClient)
					{
						await RemoveTaskInternal(taskId, httpClientHandler, httpClient, cancellationToken);
					},
				cancellationToken);
		}

		#endregion

		#region Methods

		/// <summary>
		///     Добавляет новую задачу.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>
		///     Задача <see cref="Task{TResult}" />, в рамках которой происходит получение списка задач
		///     <see cref="Model.API.Task" />.
		/// </returns>
		private async Task AddTaskInternal(
			Model.API.Task task,
			HttpClientHandler httpClientHandler,
			HttpClient httpClient,
			CancellationToken cancellationToken)
		{
			string json = JsonConvert.SerializeObject(task);

			byte[] data = Encoding.UTF8.GetBytes(json);

			using (HttpContent httpContent = new ByteArrayContent(data))
			{
				httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse(@"application/json");

				HttpResponseMessage httpResponseMessage =
					await httpClient.PostAsync(new Uri(_serviceUri, "/api/tasks"), httpContent, cancellationToken);

				string message = await httpResponseMessage.Content.ReadAsStringAsync();

				if (httpResponseMessage.IsSuccessStatusCode)
				{
					using (TextReader textReader = new StringReader(message))
					{
						using (JsonReader jsonReader = new JsonTextReader(textReader))
						{
							JsonSerializer serializer = new JsonSerializer();

							serializer.Populate(jsonReader, task);
						}
					}
				}
				else
				{
					throw new WebServiceException(message);
				}
			}
		}

		/// <summary>
		///     Изменяет существующую задачу задачу.
		/// </summary>
		/// <param name="task">Изменяемая задача.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}" />, в рамках которой происходит изменение существующей задачи.</returns>
		/// <remarks>Идентификатор задачи <see cref="Model.API.Task.Id" /> должен принадлежать ранее созданной задаче.</remarks>
		private async Task ChangeTaskInternal(
			Model.API.Task task,
			HttpClientHandler httpClientHandler,
			HttpClient httpClient,
			CancellationToken cancellationToken)
		{
			string json = JsonConvert.SerializeObject(task);

			byte[] data = Encoding.UTF8.GetBytes(json);

			using (HttpContent httpContent = new ByteArrayContent(data))
			{
				HttpResponseMessage httpResponseMessage =
					await httpClient.PutAsync(new Uri(_serviceUri, "/api/tasks/" + IdToString(task.Id)), httpContent, cancellationToken);

				if (httpResponseMessage.IsSuccessStatusCode)
				{
				}
				else
				{
					string message = await httpResponseMessage.Content.ReadAsStringAsync();

					throw new WebServiceException(message);
				}
			}
		}

		/// <summary>
		///     Убеждается в наличии текущей сессии работы с сервером. Если таковой нет, то проводит аутентификацию по имеющимуся
		///     удосстоверению пользователя.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="System.Threading.Tasks.Task" />, в рамках которой происходит проверка.</returns>
		private async Task EnsureAuthentificated(CancellationToken cancellationToken)
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
		///     Возвращает перечень имеющихся задач.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>
		///     Задача <see cref="Task{TResult}" />, в рамках которой происходит получение списка задач
		///     <see cref="Model.API.Task" />.
		/// </returns>
		private async Task<ImmutableArray<Model.API.Task>> GetTasksInternal(
			HttpClientHandler httpClientHandler,
			HttpClient httpClient,
			CancellationToken cancellationToken)
		{
			using (Stream stream = await httpClient.GetStreamAsync(new Uri(_serviceUri, "/api/tasks")))
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

		private string IdToString(Guid id)
		{
			return id.ToString(@"d");
		}

		private async Task OnClient(Func<HttpClientHandler, HttpClient, Task> action, CancellationToken cancellationToken)
		{
			await EnsureAuthentificated(cancellationToken);

			using (HttpClientHandler httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.CookieContainer.Add(_serviceUri, new Cookie(@"authToken", _authService.CurrentSession.Id));

				httpClientHandler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

				using (HttpClient httpClient = new HttpClient(httpClientHandler))
				{
					await action(httpClientHandler, httpClient);
				}
			}
		}

		/// <summary>
		///     Удаляет задачу <paramref name="taskId" />.
		/// </summary>
		/// <param name="taskId">Идентфикатор задачи, которую требуется удалить.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}" />, в рамках которой происходит удаление существующей задачи.</returns>
		private async Task RemoveTaskInternal(
			Guid taskId,
			HttpClientHandler httpClientHandler,
			HttpClient httpClient,
			CancellationToken cancellationToken)
		{
			HttpResponseMessage httpResponseMessage =
				await httpClient.DeleteAsync(new Uri(_serviceUri, "/api/tasks" + IdToString(taskId)), cancellationToken);

			string message = await httpResponseMessage.Content.ReadAsStringAsync();

			if (httpResponseMessage.IsSuccessStatusCode)
			{
			}
		}

		#endregion
	}
}