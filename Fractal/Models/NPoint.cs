using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Converters.Models
{
    public class NPoint : IEnumerable<NPoint>, INotifyPropertyChanged
    {
        double _x;
        double _y;
        NPoint _next;
        NPoint _previous;

        public double X
        {
            get => _x;
            set { _x = value; OnPropertyChanged(nameof(X)); }
        }
        public double Y
        {
            get => _y;
            set { _y = value; OnPropertyChanged(nameof(Y)); }
        }
        public NPoint Next
        {
            get => _next;
            set { _next = value; OnPropertyChanged(nameof(Next)); }
        }
        public NPoint Previous
        {
            get => _previous;
            set { _previous = value; OnPropertyChanged(nameof(Previous)); }
        }

        public bool IsFirst => Previous == null;
        public bool IsLast => Next == null;

        public NPoint AddAfter(NPoint p)
        {
            Next = p;
            p.Previous = this;
            return p;
        }

        public NPoint GetCopy() => new NPoint(X, Y);

        public NPoint AddBefore(NPoint p)
        {
            Previous = p;
            p.Next = this;
            return p;
        }

        public NPoint InsertAfter(NPoint p)
        {
            Next?.AddBefore(p);
            AddAfter(p);
            return p;
        }
        public NPoint InsertBefore(NPoint p)
        {
            Previous?.AddAfter(p);
            AddBefore(p);
            return p;
        }

        public void InsertRangeAfter(NPoint a, NPoint b)
        {
            Next?.AddBefore(b);
            AddAfter(a);
        }
        public void InsertRangeBefore(NPoint a, NPoint b)
        {
            Previous?.AddAfter(a);
            AddBefore(b);
        }

        public double DistanceTo(NPoint p) => Math.Sqrt((X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y));
        public double AngleTo(NPoint p) => AngleTo(p, out _);
        public double AngleTo(NPoint p, out double distance)
        {
            distance = DistanceTo(p);
            return Math.Asin(Math.Abs(Y - p.Y) / distance);
        }

        #region Operators

        public static NPoint operator ++(NPoint p) => p.Next;
        public static NPoint operator --(NPoint p) => p.Previous;

        public static NPoint operator +(NPoint a, NPoint b) => new NPoint(a.X + b.X, a.Y + b.Y);
        public static NPoint operator -(NPoint a, NPoint b) => new NPoint(a.X - b.X, a.Y - b.Y);

        public static implicit operator NPoint(System.Windows.Point p) => new NPoint(p.X, p.Y);
        #endregion

        public NPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        public NPoint()
        {

        }
        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        #region INotify boiler plate
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged == null) return;
            var e = new PropertyChangedEventArgs(propertyName);
            this.PropertyChanged(this, e);
        }
        #endregion

        public IEnumerator<NPoint> GetEnumerator()
        {
            return new pointEnum(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class pointEnum : IEnumerator<NPoint>
    {
        public pointEnum(NPoint p)
        {
            org = new NPoint() { Next = p };
            point = org;

        }
        NPoint org;
        public NPoint point;
        public NPoint Current => point;

        object IEnumerator.Current => Current;
        public void Dispose() { }
        public bool MoveNext()
        {
            if (point != null) point++;
            return point != null; // todo: cycle implementation
        }
        public void Reset()
        {
            point = new NPoint() { Next = org };
        }

        #region INotify boiler plate
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged == null) return;
            var e = new PropertyChangedEventArgs(propertyName);
            this.PropertyChanged(this, e);
        }
        #endregion
    }
}
