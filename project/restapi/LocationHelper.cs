using System;
using System.Collections.Generic;

namespace restapi
{
    public static class LocationHelper
    {
        private static readonly Random Randomizer = new Random();
        private const double PositionDelta = 0.0001d;


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
                loc.latitude = loc.latitude + PositionDelta;
            }
            else
            {
                //Simulated longitude change
                loc.longitude = loc.longitude + PositionDelta;
            }

            Locations[id] = loc;
        }
        

        private static (double latitude, double longitude) GetRandomStartingPoint()
        {
            //Set inside the continental US
            return (Randomizer.Next(31, 49), Randomizer.Next(-121, -75));
        }

        private static readonly Dictionary<int, (double latitude, double longitude)> Locations = new Dictionary<int, (double latitude, double longitude)>();

    }
}
