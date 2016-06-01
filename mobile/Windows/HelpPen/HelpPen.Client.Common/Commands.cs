using System;

using Windows.UI.Popups;

using Prism.Commands;

namespace HelpPen.Client.Common
{
	/// <summary>
	///     Команды приложения.
	/// </summary>
	public class Commands
	{
		#region Constructors and Destructors

		public Commands()
		{
			NotImplemented = new DelegateCommand(OnNotImplementedExecuted);
		}

		#endregion

		#region Properties

		/// <summary>
		///     Показывает сообщение о нереализованности функции.
		/// </summary>
		public DelegateCommand NotImplemented { get; private set; }

		#endregion

		#region Methods

		private async void OnNotImplementedExecuted()
		{
			var dialog = new MessageDialog("Данная функция еще не реализована.");
			await dialog.ShowAsync();
		}

		#endregion
	}
}