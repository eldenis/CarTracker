using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restapi
{
    public static class LocationHelper
    {
        internal static (double latitude, double longitude) GetCurrentLocation(int id)
        {

            if (!Locations.ContainsKey(id))
            {
                Locations.Add(id, default((double latitude, double longitude)));
            }

            //This method updates the last known location for the car and simulates its movement
            UpdateLocation(id);

            return Locations[id];
        }

        private static readonly Random Randomizer = new Random();
        private const double ValidLatitude = 90;
        private const double ValidLongitude = 180;

        private void UpdateLocation(int id)
        {
            (double latitude, double longitude) loc = Locations[id];

            //If the default value is found, randomly assign a starting point.
            if (loc.latitude == default(double) && loc.longitude == default(double))
            {
                Locations[id] = GetRandomStartingPoint();
            }
        }


        private (double latitude, double longitude) GetRandomStartingPoint()
        {
            var lat = Randomizer.NextDouble() * ValidLatitude * (Randomizer.Next(2) > 0 ? -1 : 1);
            var @long = Randomizer.NextDouble() * ValidLongitude * (Randomizer.Next(2) > 0 ? -1 : 1);

            return (lat, @long);
        }

        private static readonly Dictionary<int, (double latitude, double longitude)> Locations = new Dictionary<int, (double latitude, double longitude)>();

    }
}
