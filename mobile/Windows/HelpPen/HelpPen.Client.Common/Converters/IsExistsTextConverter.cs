using System;
using Windows.UI.Xaml.Data;

namespace HelpPen.Client.Common.Converters
{
	/// <summary>
	///     Конвертер <see cref="IValueConverter" />, возвращающий <c>true</c>, если указанная строка не пуста.
	/// </summary>
	public sealed class IsExistsTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return string.IsNullOrWhiteSpace((string) value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}