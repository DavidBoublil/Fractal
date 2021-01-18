using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temp
{
    static public class TempClass
    {
        public static double referenceAngle = Math.Asin(1 / Math.PI);

        public static Random rand = new Random((int)DateTime.Now.Ticks);

        public static Tuple<double,double> RandomPointInCircle(double radius)
        {
            // Create point in a ractangle W: PI*R H: R
            double x = rand.Next(0, (int)(radius * Math.PI));
            double y = rand.Next(0, (int)radius);

            // Convert the point to an equilateral triangle W: 2*PI*R H: R
            var z = Math.Sqrt(x * x + y * y);
            var angle = Math.Asin(y / z);

            if (angle > referenceAngle)
            {
                x = Math.PI * radius + x;
                y = radius - y;
            }

            // Convert to a circle with radius R
            double b = (radius - y) * 2 * Math.PI;
            var rho = radius - y;
            var theta = (x / b) * 2 * Math.PI - (y - 2 * Math.PI);

            x = Math.Cos(theta) * rho;
            y = Math.Sin(theta) * rho;

            return new Tuple<double, double>(x, y);
        }
    }
}
