using Newtonsoft.Json.Linq;
using Refit;
using System;

namespace WpfApp
{
    public interface ITrackingService
    {

        [Post("/api/values/authenticate")]
        IObservable<string> Authenticate([Body] JObject user);


        [Get("/api/values/{id}")]
        IObservable<JObject> GetLocation(int id, [Header("Authorization")] string authorization);
    }
}
