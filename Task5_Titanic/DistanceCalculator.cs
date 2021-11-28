using System;
using System.Collections.Generic;
using System.Text;

namespace Task5_Titanic
{
    internal static class DistanceCalculator
    {
        public static double Calculate(Location loc1, Location loc2)
        {
            // Calculates Haversine Function value
            double GetHavFuncValue(double value)
            {
                return Math.Pow(Math.Sin(value / 2), 2);

                // Can cause NAN
                //return (1 - Math.Cos(value) / 2);
            }

            // Calculates Haversine Formula value
            double GetHavFormulaValue(double f1, double f2, double l1, double l2)
            {
                // Cos is best for long distances
                return GetHavFuncValue(f2 - f1) + Math.Cos(f1) * Math.Cos(f2) * GetHavFuncValue(l2 - l1);

                // Best for small distances 
                //return GetHavFuncValue(f2 - f1) + (1 + GetHavFuncValue(f1 - f2) - GetHavFuncValue(f1 + f2)) * GetHavFuncValue(l2 - l1);
            }

            // Calculates Distance from Haversin Formula value
            double GetDistance(double havValue, double earthRadius)
            {
                return (2 * earthRadius * Math.Asin(Math.Sqrt(havValue)));
            }

            double GetRad(double degree)
            {
                return degree * (Math.PI / 180);
            }

            void FixLocations(Location location1, Location location2)
            {
                if (location1.LatitudePole != location2.LatitudePole)
                {
                    var sum = location1.GetCoordinateInDD(true).dd + location2.GetCoordinateInDD(true).dd;
                    location1.Set_X(0);
                    location2.Set_X(sum);
                }

                if (location1.LongitudePole != location2.LongitudePole)
                {
                    var sum = location1.GetCoordinateInDD(false).dd + location2.GetCoordinateInDD(false).dd;
                    location1.Set_Y(0);
                    location2.Set_Y(sum);
                }
            }

            // Fix location based on poles
            var location1 = (Location)loc1.Clone();
            var location2 = (Location)loc2.Clone();
            FixLocations(location1, location2);

            // Get fi and lambda values in DD format
            double f1 = GetRad(location1.GetCoordinateInDD(true).dd);
            double f2 = GetRad(location2.GetCoordinateInDD(true).dd);
            double l1 = GetRad(location1.GetCoordinateInDD(false).dd);
            double l2 = GetRad(location2.GetCoordinateInDD(false).dd);

            double havValue = GetHavFormulaValue(f1, f2, l1, l2);
            double earthRadius = GetEarthRadius(true);
            double distance = GetDistance(havValue, earthRadius);

            return Math.Round(distance, 2);
        }

        private static double GetEarthRadius(bool inMiles)
        {
            double seaLevelRadius = 6378.137;
            if (inMiles)
            {
                return seaLevelRadius * 0.6213;
            }

            return seaLevelRadius;
        }
    }
}
