using System.Collections.Generic;

using HelpPen.Client.Common.Model.API;

namespace HelpPen.Client.Common
{
	/// <summary>
	///     Компаратор для сравнивания порядковых номеров задач <see cref="Task" />.
	/// </summary>
	public class TaskOrderComparer : IComparer<Task>
	{
		#region Constructors and Destructors

		/// <summary>
		///     Инициализирует статические поля класса <see cref="TaskOrderComparer" />.
		/// </summary>
		static TaskOrderComparer()
		{
			Instance = new TaskOrderComparer();
		}

		#endregion

		#region Properties

		/// <summary>
		///     Экземпляр класса <see cref="TaskOrderComparer" />.
		/// </summary>
		public static TaskOrderComparer Instance { get; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Сравнение двух объектов и возврат значения, указывающего, является ли один объект меньшим, равным или большим
		///     другого.
		/// </summary>
		/// <returns>
		///     Знаковое целое число, которое определяет относительные значения параметров <paramref name="x" /> и
		///     <paramref name="y" />, как показано в следующей таблице.Значение Значение Меньше нуляЗначение параметра
		///     <paramref name="x" /> меньше значения параметра <paramref name="y" />.ZeroЗначения параметров <paramref name="x" />
		///     и <paramref name="y" /> равны.Больше нуля.Значение <paramref name="x" /> больше значения <paramref name="y" />.
		/// </returns>
		/// <param name="x">Первый сравниваемый объект.</param>
		/// <param name="y">Второй сравниваемый объект.</param>
		public int Compare(Task x, Task y)
		{
			return y.orderNumber.CompareTo(x.orderNumber);
		}

		#endregion
	}
}