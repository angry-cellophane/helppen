using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using HelpPen.Client.Common.Annotations;

namespace HelpPen.Client.Common.MVVM
{
	/// <summary>
	///     Базовый класс для моделей представления MVVM.
	/// </summary>
	public abstract class ViewModel : INotifyPropertyChanging, INotifyPropertyChanged
	{
		#region Methods

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
		{
			PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		public event PropertyChangingEventHandler PropertyChanging;

		protected virtual void SetPropertyValue<TValue>(TValue oldValue, TValue newValue, Action<TValue> updateValue, [CallerMemberName] string propertyName = null)
		{
			if (!Equals(oldValue, newValue))
			{
				// ReSharper disable once ExplicitCallerInfoArgument
				OnPropertyChanging(propertyName);

				updateValue(newValue);

				OnPropertyChanged(propertyName);
			}
		}
	}
}