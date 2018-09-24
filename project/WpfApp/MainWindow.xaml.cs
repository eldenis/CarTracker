using ReactiveUI;
using Splat;
using System;
using System.Reactive.Linq;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IViewFor<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = Locator.CurrentMutable.GetService<MainViewModel>();

            //ViewModel.UpdateCar
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Catch<int, Exception>(ex =>
            //    {
            //        MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        return null;
            //    })
            //    .Subscribe(newValue =>
            //    {
            //        ViewModel.FollowedCar = newValue;
            //    });

        }

        /// <summary>
        /// Allows the ViewModel to be used on the XAML via a dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MainViewModel), typeof(MainWindow),
                new PropertyMetadata(default(MainViewModel)));

        /// <summary>
        /// Implementation for the IViewFor interface
        /// </summary>
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)value;
        }

        /// <summary>
        /// Regular property to use the ViewModel from this class
        /// </summary>
        public MainViewModel ViewModel
        {
            get => (MainViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
    }
}
