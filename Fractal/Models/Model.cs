using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractal.Models
{
    public class Model
    {
        // todo: change it to tuple<Point,Point>
        public Point First { get; set; }
        public Point Last { get; set; }

        // Return the distance between First and Last 
        public double Distance => First.DistanceTo(Last);

        // Return the pattern between the two points
        public Tuple<Point, Point> Pattern => new Tuple<Point, Point>(First.Next, Last.Previous);

        public Tuple<Point, Point> GetCopy()
        {
            Point first = First.GetCopy();
            Point last = first;

            Point iterator = First.Next;
            while (iterator != null)
            {
                last.AddAfter(iterator.GetCopy());
                last++;
                iterator++;
            }

            return new Tuple<Point, Point>(first, last);
        }

        public void ApplyModel(Point a, Point b)
        {
            var copy = GetCopy();

            // Get the angle and distance (optimized function to calculate distance only once)
            double shapeAngle = a.AngleTo(b, out double shapeDistance);

            double scaleFactor = shapeDistance / Distance;
            Rescale(copy, scaleFactor);
            Rotate(copy, shapeAngle);

            // insert
            a.InsertRangeAfter(copy.Item1.Next, copy.Item2.Previous);
            a.AddAfter(copy.Item1.Next);
            b.AddBefore(copy.Item2.Previous);
        }

        private void Rotate(Tuple<Point, Point> copy, double shapeAngle)
        {
            Rotate(copy.Item1, copy.Item2, shapeAngle);
        }

        private void Rotate(Point a, Point b, double shapeAngle)
        {
            // Move b to be around (0,0) instead of around a
            b -= a;

            double x = b.X * Math.Cos(shapeAngle) - b.Y * Math.Sin(shapeAngle);
            double y = b.X * Math.Sin(shapeAngle) + b.Y * Math.Cos(shapeAngle);

            // Re-move b to be around a
            b += a;
        }

        public void Rescale(Tuple<Point, Point> shape, double scaleFactor)
        {
            Rescale(shape.Item1, shape.Item2, scaleFactor);
        }

        public void Rescale(Point a, Point b, double scaleFactor)
        {
            // todo: a is rescalled to it's own place. can be optimized. 
            Point iterator = a;
            while (iterator != null && iterator.Previous != b)
            {
                iterator -= a;

                a.X *= scaleFactor;
                a.Y *= scaleFactor;

                iterator++;
                iterator += a;
            }
        }
    }
}
