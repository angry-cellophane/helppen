using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace HelpPen.Client.Common
{
	/// <summary>
	/// Сервис работы с HelpPen.
	/// </summary>
	public interface IHelpPenService
	{
		/// <summary>
		/// Возвращает перечень имеющихся задач.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит получение списка задач <see cref="Model.API.Task"/>.</returns>
		Task<ImmutableArray<Model.API.Task>> GetTasks(CancellationToken cancellationToken);

		/// <summary>
		/// Добавляет новую задачу.
		/// </summary>
		/// <param name="task">Добавляемая задача.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит добавление новой задачи.</returns>
		/// <remarks>Идентификатор задачи <see cref="Model.API.Task.Id"/> должен быть сгенерирован на стороне клиента.</remarks>
		Task AddTask(Model.API.Task task, CancellationToken cancellationToken);

		/// <summary>
		/// Изменяет существующую задачу задачу.
		/// </summary>
		/// <param name="task">Изменяемая задача.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит изменение существующей задачи.</returns>
		/// <remarks>Идентификатор задачи <see cref="Model.API.Task.Id"/> должен принадлежать ранее созданной задаче.</remarks>
		Task ChangeTask(Model.API.Task task, CancellationToken cancellationToken);

		/// <summary>
		/// Удаляет задачу <paramref name="taskId"/>.
		/// </summary>
		/// <param name="taskId">Идентфикатор задачи, которую требуется удалить.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит удаление существующей задачи.</returns>
		Task RemoveTask(Guid taskId, CancellationToken cancellationToken);
	}
}
