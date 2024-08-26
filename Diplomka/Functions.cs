using System;

namespace Diplomka
{
    internal class Functions
    {
        static double g = 9.8;
        internal static double kineticEnergy(double m, double speed)
        {
            return 0.5 * m * speed * speed;
        }

        internal static double speedFromKE(double m, double kineticEnergy)
        {
            return Math.Sqrt(2 * kineticEnergy / m);
        }

        internal static double aerodynamicResistance(double airSpeed, double CdA = 0.3, double rho = 1.225)
        {
            return 0.5 * CdA * rho * airSpeed * Math.Abs(airSpeed);
        }

        internal static double rollingResistance(double m, double Crr = 0.003)
        {
            return m * Crr * g;
        }

        internal static double potentialEnergy(double m, double h)
        {
            return m * g * h;
        }

        internal static double calculateBearing(double lat1, double lon1, double lat2, double lon2)
        {
            double phi1 = lat1 * Math.PI / 180.0;
            double phi2 = lat2 * Math.PI / 180.0;
            double deltaLambda = (lon2 - lon1) * Math.PI / 180.0;

            double y = Math.Sin(deltaLambda) * Math.Cos(phi2);
            double x = Math.Cos(phi1) * Math.Sin(phi2) -
                       Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(deltaLambda);
            double bearing = Math.Atan2(y, x);

            bearing = bearing * 180.0 / Math.PI;
            bearing = (bearing + 360.0) % 360.0;

            return bearing;
        }

        internal static double calculateCircleRadius(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            double a = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            double b = Math.Sqrt(Math.Pow(x3 - x2, 2) + Math.Pow(y3 - y2, 2));
            double c = Math.Sqrt(Math.Pow(x1 - x3, 2) + Math.Pow(y1 - y3, 2));

            double s = (a + b + c) / 2;

            double area = Math.Sqrt(s * (s - a) * (s - b) * (s - c));

            double radius = (a * b * c) / (4 * area);

            if (area == 0)
                return double.PositiveInfinity;
            return radius;
        }
    }
}
