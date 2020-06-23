using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractal.Models
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point Next { get; set; }
        public Point Previous { get; set; }

        public bool IsFirst => Previous == null;
        public bool IsLast => Next == null;

        public void AddAfter(Point p)
        {
            Next = p;
            p.Previous = this;
        }
        public void AddBefore(Point p)
        {
            Previous = p;
            p.Next = this;
        }

        public void InsertAfter(Point p)
        {
            Next?.AddBefore(p);
            AddAfter(p);
        }
        public void InsertBefore(Point p)
        {
            Previous?.AddAfter(p);
            AddBefore(p);
        }

        public void InsertRangeAfter(Point a, Point b)
        {
            Next?.AddBefore(b);
            AddAfter(a);
        }
        public void InsertRangeBefore(Point a, Point b)
        {
            Previous?.AddAfter(a);
            AddBefore(b);
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
