using System;

using Windows.UI.Xaml.Navigation;

namespace HelpPen.Client.Common.MVVM
{
	/// <summary>
	///     Базовый класс для моделей представления MVVM <see cref="Page" />.
	/// </summary>
	public abstract class PageViewModel : ViewModel
	{
		#region Public Methods and Operators

		public virtual void OnNavigatedFrom(NavigationEventArgs e)
		{
		}

		public virtual void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		public virtual void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
		}

		#endregion
	}
}