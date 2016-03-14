using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml;

namespace HelpPen.Client.Windows.Pages.TaskList
{
	public class TestVM1: INotifyPropertyChanged //DependencyObject
	{
		private string _newTaskText;

		//public static readonly DependencyProperty NewTaskTextProperty = DependencyProperty.Register(
		//	"NewTaskText",
		//	typeof(string),
		//	typeof(TestVM1),
		//	new PropertyMetadata(default(string)));

		//public string NewTaskText
		//{
		//	get
		//	{
		//		return (string)GetValue(NewTaskTextProperty);
		//	}
		//	set
		//	{
		//		SetValue(NewTaskTextProperty, value);
		//	}
		//} 

		public string NewTaskText
		{
			get
			{
				return _newTaskText;
			}
			set
			{
				_newTaskText = value;

				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}