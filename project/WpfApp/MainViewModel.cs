using ReactiveUI;
using Splat;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;

namespace WpfApp
{
    public class MainViewModel : ReactiveObject
    {
        #region Private Members

        private readonly ITrackingService _service;
        private readonly ISubject<(double latitude, double longitude)> _locationUpdate;

        #endregion

        #region Methods

        public MainViewModel()
        {
            _service = Locator.Current.GetService<ITrackingService>();
            _locationUpdate = new Subject<(double latitude, double longitude)>();

            UpdateCar = ReactiveCommand.Create(() =>
            {
                var parsedCorrectly = int.TryParse(NewCarToFollow, out int newCar);
                NewCarToFollow = null;
                if (!parsedCorrectly)
                {
                    MessageBox.Show("There was an error reading the number of the car to follow. Please, review it.",
                        "Car Tracking Service", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }

                FollowedCar = newCar;
            }, canExecute: this.WhenAnyValue(x => x.NewCarToFollow).Select(x => !string.IsNullOrWhiteSpace(x)));


            Scheduler.Default.SchedulePeriodic(TimeSpan.FromMilliseconds(500),
                () => _service.GetLocation(FollowedCar, App.GetToken())
                    .Select(jo =>
                    (
                        latitude: double.Parse(jo["Latitude"].ToString()),
                        longitude: double.Parse(jo["Longitude"].ToString())
                    )).Subscribe(newLocation => _locationUpdate.OnNext(newLocation)));
        }

        #endregion

        #region Properties

        private string _newCarToFollow;
        public string NewCarToFollow
        {
            get => _newCarToFollow;
            set => this.RaiseAndSetIfChanged(ref _newCarToFollow, value);
        }

        private int _followedCar = 1;
        public int FollowedCar
        {
            get => _followedCar;
            set => this.RaiseAndSetIfChanged(ref _followedCar, value);
        }

        public IObservable<(double latitude, double longitude)> LocationUpdate => _locationUpdate;

        private ReactiveCommand _updateCar;
        public ReactiveCommand UpdateCar
        {
            get => _updateCar;
            set => this.RaiseAndSetIfChanged(ref _updateCar, value);
        }

        #endregion

    }
}
