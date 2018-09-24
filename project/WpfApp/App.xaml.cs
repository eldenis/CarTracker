using Newtonsoft.Json.Linq;
using Refit;
using Splat;
using System.Reactive.Linq;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetDependencyInjection();
            LogIn();

        }

        private void SetDependencyInjection()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => RestService.For<ITrackingService>(ServerUri), typeof(ITrackingService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new MainViewModel(), typeof(MainViewModel));
        }

        private static string Token;
        private const string ServerUri = "http://localhost:54587";
        private void LogIn()
        {
            try
            {
                var userInfo = new JObject { ["username"] = "user", ["password"] = "123" };

                Token = Locator.Current.GetService<ITrackingService>()
                    .Authenticate(userInfo)
                    .Wait();
            }
            catch
            {
                MessageBox.Show("There was an error validating the user. Is the service up?");
                Shutdown();
            }
        }

        internal static string GetToken()
        {
            return $"Bearer {Token}";
        }

    }
}
