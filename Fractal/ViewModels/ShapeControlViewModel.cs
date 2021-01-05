using Converters.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Converters.ViewModels
{
    public class ShapeControlViewModel : INotifyPropertyChanged
    {
        public NPoint _originPoint;
        private NPoint _shape;
        private double _scale;
        private bool _pointsVisible;
        private bool _linesVisible;




        public NPoint OriginPoint
        {
            get => _originPoint;
            set
            {
                _originPoint = value;
                Update();
                OnPropertyChanged("OriginPoint");
            }
        }
        public NPoint Shape
        {
            get => _shape;
            set
            {
                _shape = value;
                Update();
                OnPropertyChanged("Shape");
            }
        }
        public double Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                Update();
                OnPropertyChanged(nameof(Scale));
            }
        }
        public bool PointsVisible
        {
            get { return _pointsVisible; }
            set { _pointsVisible = value; OnPropertyChanged(nameof(PointsVisible)); }
        }
        public bool LinesVisible
        {
            get { return _linesVisible; }
            set { _linesVisible = value; OnPropertyChanged(nameof(LinesVisible)); }
        }

        public ObservableCollection<NPoint> PointsList { get; set; }

        public ICommand UpdateOriginCommand { get; set; }
        public void UpdateOrigin(object param)
        {
            Canvas canvas = param as Canvas;
            var mousePosition = Mouse.GetPosition(canvas);
            OriginPoint.X = mousePosition.X;
            OriginPoint.Y = mousePosition.Y;
            Update();
        }

        public void Update()
        {
            int i = 0;
            for (NPoint iterator = Shape; iterator != null; iterator++, i++)
            {
                var x = iterator.X;
                var y = iterator.Y;

                // rescale 
                x *= Scale;
                y *= Scale;

                // relocate around origin
                x += OriginPoint.X;
                y = OriginPoint.Y - y; // due to the nature of UI elements to have inverted axis

                // Update the entry in the list
                if (PointsList.Count <= i)
                {
                    var p = new NPoint();
                    PointsList.Add(new NPoint());
                }

                NPoint entry = PointsList[i];
                entry.X = x;
                entry.Y = y;

                // Circle case
                if (iterator == Shape && i != 0)
                {
                    PointsList[0].AddBefore(PointsList.Last());
                    break;
                }
            }

            if (PointsList != null)
                RemoveUnnecessaryPoints(Math.Max(0, PointsList.Count - i));
        }

        private void RemoveUnnecessaryPoints(int count)
        {
            for (int i = 0; i < count; i++)
            {
                PointsList.Remove(PointsList.Last());
            }
        }

        private void UpdatePointCount(int count)
        {
            while (PointsList.Count <= count)
                PointsList.Add(new NPoint());
        }

        public ShapeControlViewModel()
        {
            OriginPoint = new NPoint(50, 300);
            Scale = 1;
            PointsVisible = true;
            LinesVisible = true;

            PointsList = new ObservableCollection<NPoint>();
            PointsList.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add && PointsList.Count > 1)
                {
                    PointsList[PointsList.Count - 2].AddAfter(PointsList.Last());
                }
            };

            //Commands
            UpdateOriginCommand = new RelayCommand(UpdateOrigin);
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
