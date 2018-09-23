using System;
using System.Collections.Generic;

namespace restapi
{
    public static class LocationHelper
    {
        private static readonly Random Randomizer = new Random();
        private const double ValidLatitude = 90;
        private const double ValidLongitude = 180;
        private const double KmChange = 0.1;
        private const double EarthRadius = 6378;


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

        private static void UpdateLocation(int id)
        {
            (double latitude, double longitude) loc = Locations[id];

            //If the default value is found, randomly assign a starting point.
            if (loc.latitude == default(double) && loc.longitude == default(double))
            {
                loc = Locations[id] = GetRandomStartingPoint();
            }

            if (Randomizer.Next(2) > 0)
            {
                //In this scenario we simulate an updated latitude
                loc.latitude = loc.latitude + (KmChange / EarthRadius) * (180d / Math.PI);
            }
            else
            {
                //Simulated longitude change
                loc.longitude = (loc.longitude + (KmChange / EarthRadius) * (180d / Math.PI)) / Math.Cos(loc.latitude * Math.PI / 180d);
            }

            Locations[id] = loc;
        }


        private static (double latitude, double longitude) GetRandomStartingPoint()
        {
            var lat = Randomizer.NextDouble() * ValidLatitude * (Randomizer.Next(2) > 0 ? -1 : 1);
            var @long = Randomizer.NextDouble() * ValidLongitude * (Randomizer.Next(2) > 0 ? -1 : 1);

            return (lat, @long);
        }

        private static readonly Dictionary<int, (double latitude, double longitude)> Locations = new Dictionary<int, (double latitude, double longitude)>();

    }
}
