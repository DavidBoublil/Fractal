using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fractal.Models
{
    public class Point : IEnumerable<Point>
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

        public Point GetCopy() => new Point(X, Y);

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

        public double DistanceTo(Point p) => Math.Sqrt((X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y));
        public double AngleTo(Point p) => AngleTo(p, out _);
        public double AngleTo(Point p, out double distance)
        {
            distance = DistanceTo(p); 
            return Math.Asin(Math.Abs(Y - p.Y) / distance);
        }

        public static Point operator ++(Point p) => p.Next;
        public static Point operator --(Point p) => p.Previous;

        // todo: check if works
        public static Point operator +(Point a, Point b)
        {
            a.X += b.X;
            a.Y += b.Y;
            return a;
        }
        public static Point operator -(Point a, Point b)
        {
            a.X -= b.X;
            a.Y -= b.Y;
            return a;
        }

        public static implicit operator Point(System.Windows.Point p) => new Point(p.X,p.Y);

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Point()
        {

        }

        public IEnumerator<Point> GetEnumerator()
        {
            return new pointEnum(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class pointEnum : IEnumerator<Point>
    {
        public pointEnum(Point p)
        {
            org = p;
            point = p;

        }
        Point org;
        public Point point;
        public Point Current => point;

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            point++;
            return point != null; // todo: cycle implementation
        }

        public void Reset()
        {
            point = new Point() { Next = org };
        }
    }
}
