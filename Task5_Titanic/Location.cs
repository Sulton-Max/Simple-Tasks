using System;
using System.Collections.Generic;
using System.Text;

namespace Task5_Titanic
{
    internal enum Poles : byte
    {
        N = 0, S = 1, W = 2, E = 3
    }

    internal class Location : ICloneable
    {
        // For latitude
        public int X_D { get; private set; }
        public int X_M { get; private set; }
        public int X_S { get; private set; }
        public Poles LatitudePole;

        // For longitude
        public int Y_D { get; private set; }
        public int Y_M { get; private set; }
        public int Y_S { get; private set; }
        public Poles LongitudePole;

        public Location(int x1, int x2, int x3, Poles xPole, int y1, int y2, int y3, Poles yPole)
        {
            if (xPole == Poles.W || xPole == Poles.E || yPole == Poles.N || yPole == Poles.S)
                throw new ArgumentException("Latitude or Longitude Pole value exception");

            X_D = x1;
            X_M = x2;
            X_S = x3;
            LatitudePole = xPole;

            Y_D = y1;
            Y_M = y2;
            Y_S = y3;
            LongitudePole = yPole;
        }

        public Location(double latitudeDD, Poles xPole, double longitudeDD, Poles yPole)
        {
            if (xPole == Poles.W || xPole == Poles.E || yPole == Poles.N || yPole == Poles.S)
                throw new ArgumentException("Latitude or Longitude Pole value exception");

            var value = ConvertDDtoDMS(latitudeDD);
            X_D = value.d;
            X_M = value.m;
            X_S = value.s;
            LatitudePole = xPole;

            value = ConvertDDtoDMS(longitudeDD);
            Y_D = value.d;
            Y_M = value.m;
            Y_S = value.s;
            LongitudePole = yPole;
        }

        public static (int d, int m, int s) ConvertDDtoDMS(double dd)
        {
            int d = (int)(dd);
            int m = (int)((dd - d) * 60);
            int s = (int)((dd - d - m / 60) * 3600);
            return (d, m, s);
        }

        public static double ConvertDMStoDD (int d, int m, int s)
        {
            double dd = (d + m / 60 + s / 3600);
            return dd;
        }

        public (double dd, Poles pole)  GetCoordinateInDD(bool latitude)
        {
            if(latitude)
            {
                var dd = ConvertDMStoDD(X_D, X_M, X_S);
                return (dd, LatitudePole);
            }else
            {
                var dd = ConvertDMStoDD(Y_D, Y_M, Y_S);
                return (dd, LongitudePole);
            }
        } 

        public (int d, int m, int s, Poles pole) GetCoordinateInDMS(bool latitude)
        {
            if (latitude)
            {
                return (X_D, X_M, X_S, LatitudePole);
            }
            else
            {
                return (Y_D, Y_M, Y_S, LongitudePole);
            }
        }

        public object Clone()
        {
            return new Location(X_D, X_M, X_S, LatitudePole, Y_D, Y_M, Y_S, LongitudePole);
        }

        public void Set_X(double dd)
        {
            var dms = ConvertDDtoDMS(dd);
            X_D = dms.d;
            X_M = dms.m;
            X_S = dms.s;
        }

        public void Set_Y(double dd)
        {
            var dms = ConvertDDtoDMS(dd);
            Y_D = dms.d;
            Y_M = dms.m;
            Y_S = dms.s;
        }
    }
}
