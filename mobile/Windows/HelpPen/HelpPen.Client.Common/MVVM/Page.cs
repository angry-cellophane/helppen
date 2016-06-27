using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace HelpPen.Client.Common.MVVM
{
	/// <summary>
	///     Страница <see cref="Windows.UI.Xaml.Controls.Page" /> для исполщьзования в модели MVVM.
	/// </summary>
	public class Page : Windows.UI.Xaml.Controls.Page
	{
		#region Constructors and Destructors

		/// <summary>
		///     Инициализирует новый экзмепляр класса <see cref="Page" />.
		/// </summary>
		public Page()
		{
			Loading += OnLoading;
			Loaded += OnLoaded;

			//EnsureViewModelCreated();
		}

		#endregion

		#region Properties

		/// <summary>
		///     Модель представления данной страницы.
		/// </summary>
		public PageViewModel ViewModel
		{
			get
			{
				return (PageViewModel)DataContext;
			}
			set
			{
				DataContext = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		///     Создает новый экземпляр модели представления.
		/// </summary>
		/// <returns>Созданный экземпляр модели представления.</returns>
		protected TViewMovel CreateInstance<TViewMovel>()
			where TViewMovel : PageViewModel
		{
			IUnityContainer unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();

			return unityContainer.Resolve<TViewMovel>(new ParameterOverride(@"frame", Frame));
		}

		/// <summary>
		///     Создает модель пердставления для данной страницы.
		/// </summary>
		/// <returns>Созданный экземпляр модели представления.</returns>
		protected virtual PageViewModel CreateViewModel()
		{
			return CreateInstance<EmptyViewModel>();
		}

		protected virtual void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
		}

		protected virtual void OnLoading(FrameworkElement sender, object args)
		{
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			EnsureViewModelCreated();

			base.OnNavigatedFrom(e);

			ViewModel.OnNavigatedFrom(e);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			EnsureViewModelCreated();

			base.OnNavigatedTo(e);

			ViewModel.OnNavigatedTo(e);
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			EnsureViewModelCreated();

			base.OnNavigatingFrom(e);

			ViewModel.OnNavigatingFrom(e);
		}

		private void EnsureViewModelCreated()
		{
			if (ViewModel == null || ViewModel is EmptyViewModel)
			{
				ViewModel = CreateViewModel();
			}
		}

		#endregion

		#region Nested Types

		private sealed class EmptyViewModel : PageViewModel
		{
			#region Constructors and Destructors

			public EmptyViewModel(Frame frame)
				: base(frame)
			{
			}

			#endregion
		}

		#endregion
	}
}