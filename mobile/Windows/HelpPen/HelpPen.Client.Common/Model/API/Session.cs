using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpPen.Client.Common.Model.API
{
	/// <summary>
	/// Сессия работы пользователя.
	/// </summary>
	public class Session
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="Session"/>.
		/// </summary>
		/// <param name="id">Идентификатор сесии.</param>
		/// <param name="createTime">Время создания сесии.</param>
		/// <param name="expirationTime">Время истечения сесии.</param>
		public Session(string id, DateTimeOffset createTime, DateTimeOffset expirationTime)
		{
			Id = id;
			CreateTime = createTime;
			ExpirationTime = expirationTime;
		}

		/// <summary>
		/// Идентификатор сесии.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Время создания сесии.
		/// </summary>
		public DateTimeOffset CreateTime { get; private set; }

		/// <summary>
		/// Время истечения сесии.
		/// </summary>
		public DateTimeOffset ExpirationTime { get; private set; }
	}
}
