using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Converters.Models;

namespace Converters.ViewModels
{
    public class ShapeControlViewModel : INotifyPropertyChanged
    {
        public NPoint _originPoint;
        private NPoint _shape;
        private double _scale;

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
                y += OriginPoint.Y;

                // Update the entry in the list
                while (PointsList.Count <= i)
                    PointsList.Add(new NPoint());

                NPoint entry = PointsList[i];
                entry.X = x;
                entry.Y = y;
            }
        }

        public ShapeControlViewModel()
        {
            OriginPoint = new NPoint(50, 50);
            Scale = 1;

            PointsList = new ObservableCollection<NPoint>();

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
