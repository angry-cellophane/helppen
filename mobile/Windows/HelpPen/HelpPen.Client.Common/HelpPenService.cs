using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HelpPen.Client.Common.Model.API;

namespace HelpPen.Client.Common
{
	/// <summary>
	/// Сервис работы с HelpPen.
	/// </summary>
	public class HelpPenService : IHelpPenService
	{
		/// <summary>
		/// Возвращает перечень имеющихся задач.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит получение списка задач <see cref="Model.API.Task"/>.</returns>
		public System.Threading.Tasks.Task AddTask(Model.API.Task task, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Добавляет новую задачу.
		/// </summary>
		/// <param name="task">Добавляемая задача.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит добавление новой задачи.</returns>
		/// <remarks>Идентификатор задачи <see cref="Model.API.Task.Id"/> должен быть сгенерирован на стороне клиента.</remarks>
		public System.Threading.Tasks.Task ChangeTask(Model.API.Task task, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Изменяет существующую задачу задачу.
		/// </summary>
		/// <param name="task">Изменяемая задача.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит изменение существующей задачи.</returns>
		/// <remarks>Идентификатор задачи <see cref="Model.API.Task.Id"/> должен принадлежать ранее созданной задаче.</remarks>
		public Task<ImmutableArray<Model.API.Task>> GetTasks(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Удаляет задачу <paramref name="taskId"/>.
		/// </summary>
		/// <param name="taskId">Идентфикатор задачи, которую требуется удалить.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача <see cref="Task{TResult}"/>, в рамках которой происходит удаление существующей задачи.</returns>
		public System.Threading.Tasks.Task RemoveTask(Guid taskId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
