using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HelpPen.Client.Common.MVVM
{
	/// <summary>
	///     Базовый класс для моделей представления MVVM <see cref="Page" />.
	/// </summary>
	public abstract class PageViewModel : ViewModel
	{
		protected PageViewModel(Frame frame)
		{
			Frame = frame;
		}

		#region Public Methods and Operators

		public Frame Frame { get; private set; }

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