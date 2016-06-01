

using System;
using Windows.UI.Xaml.Data;

namespace HelpPen.Client.Common.Converters
{
	/// <summary>
	///     Конвертер <see cref="IValueConverter" />, возвращающий <c>true</c>, если указано <c>false</c> и наоборот.
	/// </summary>
	public sealed class NotConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}