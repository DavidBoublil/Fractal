using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converters.Models
{
    public class Model
    {
        // todo: change it to tuple<Point,Point>
        public NPoint First { get; set; }
        public NPoint Last { get; set; }

        // Return the distance between First and Last 
        public double Distance => First.DistanceTo(Last);

        // Return the pattern between the two points
        public Tuple<NPoint, NPoint> Pattern => new Tuple<NPoint, NPoint>(First.Next, Last.Previous);

        public Tuple<NPoint, NPoint> GetCopy()
        {
            NPoint first = First.GetCopy();
            NPoint last = first;

            NPoint iterator = First.Next;
            // todo: change to for
            while (iterator != null)
            {
                last.AddAfter(iterator.GetCopy());
                last++;
                iterator++;
            }

            return new Tuple<NPoint, NPoint>(first, last);
        }

        public void ApplyModel(Tuple<NPoint, NPoint> points) => ApplyModel(points.Item1, points.Item2);

        public void ApplyModel(NPoint a, NPoint b)
        {
            Tuple<NPoint, NPoint> copy = GetCopy();

            // Get the angle and distance (optimized function to calculate distance only once)
            double shapeAngle = a.AngleTo(b, out double shapeDistance);
            double scaleFactor = shapeDistance / Distance;
            
            Rescale(copy, scaleFactor);
            Rotate(copy, shapeAngle);

            // insert
            a.InsertRangeAfter(copy.Item1.Next, copy.Item2.Previous);
        }

        private void Rotate(Tuple<NPoint, NPoint> copy, double shapeAngle)
        {
            Rotate(copy.Item1, copy.Item2, shapeAngle);
        }

        private void Rotate(NPoint a, NPoint b, double shapeAngle)
        {
            // Move b to be around (0,0) instead of around a
            b -= a;

            double x = b.X * Math.Cos(shapeAngle) - b.Y * Math.Sin(shapeAngle);
            double y = b.X * Math.Sin(shapeAngle) + b.Y * Math.Cos(shapeAngle);

            // Re-move b to be around a
            b += a;
        }

        public void Rescale(Tuple<NPoint, NPoint> shape, double scaleFactor)
        {
            Rescale(shape.Item1, shape.Item2, scaleFactor);
        }

        public void Rescale(NPoint a, NPoint b, double scaleFactor)
        {
            // todo: a is rescalled to it's own place. can be optimized. 
            NPoint iterator = a;
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
