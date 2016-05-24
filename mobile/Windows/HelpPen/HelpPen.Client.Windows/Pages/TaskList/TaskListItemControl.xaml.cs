using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

using HelpPen.Client.Common;
using HelpPen.Client.Common.Model.API;

using Microsoft.Practices.ServiceLocation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HelpPen.Client.Windows.Pages.TaskList
{
	public sealed partial class TaskListItemControl : UserControl
	{
		public TaskListItemControl()
		{
			this.InitializeComponent();
		}

		private void OnStateIndicatorTapped(object sender, TappedRoutedEventArgs e)
		{
			Rectangle rectangle = e.OriginalSource as Rectangle;

			if (rectangle != null)
			{
				Task task = (Task)rectangle.DataContext;

				if (task.state == TaskState.NOT_COMPLITED)
				{
					task.state = TaskState.COMPLITED;
				}
			}
		}
	}
}
