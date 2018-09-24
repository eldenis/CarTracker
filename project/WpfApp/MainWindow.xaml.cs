using Microsoft.Maps.MapControl.WPF;
using ReactiveUI;
using Splat;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media;

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

            ViewModel.LocationUpdate
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(SetLocation);
        }

        private void SetLocation((double latitude, double longitude) newLocation)
        {
            //New location for the tracked vehicle.
            var location = new Location(newLocation.latitude, newLocation.longitude);
            //Remove previous pin
            myMap.Children.Clear();
            //Center pin and keep same Zoom Level
            myMap.SetView(location, myMap.ZoomLevel);

            var pin = new Pushpin
            {
                Location = location,
                Background = Brushes.Green
            };
            //Add new pin to the map
            myMap.Children.Add(pin);
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
