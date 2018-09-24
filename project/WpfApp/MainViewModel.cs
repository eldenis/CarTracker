using ReactiveUI;
using System.Reactive.Linq;
using System.Windows;

namespace WpfApp
{
    public class MainViewModel : ReactiveObject
    {

        public MainViewModel()
        {
            UpdateCar = ReactiveCommand.Create(() =>
            {
                var parsedCorrectly = int.TryParse(NewCarToFollow, out int newCar);
                NewCarToFollow = null;
                if (!parsedCorrectly)
                {
                    MessageBox.Show("There was an error reading the number of the car to follow. Please, review it.",
                        "", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }

                FollowedCar = newCar;
            }, canExecute: this.WhenAnyValue(x => x.NewCarToFollow).Select(x => !string.IsNullOrWhiteSpace(x)));

        }

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


        private ReactiveCommand _updateCar;
        public ReactiveCommand UpdateCar
        {
            get => _updateCar;
            set => this.RaiseAndSetIfChanged(ref _updateCar, value);
        }

    }
}
