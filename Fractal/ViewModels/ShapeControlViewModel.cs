using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Fractal.Models;

namespace Fractal.ViewModels
{
    public class ShapeControlViewModel : INotifyPropertyChanged
    {
        private int _cornerRadius;
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = value; OnPropertyChanged("CornerRadius"); }
        }

        private Brush _backgroundColor;
        public Brush BackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; OnPropertyChanged(("BackgroundColor")); }
        }

        private Point _originPoint;
        public Point OriginPoint
        {
            get => _originPoint;
            set { _originPoint = value; OnPropertyChanged("OriginPoint"); }
        }

        private Point _shape;
        public Point Shape
        {
            get => _shape;
            set { _shape = value; OnPropertyChanged("Shape"); }
        }

        public ShapeControlViewModel()
        { 
            OriginPoint = new Point(50,50);
            BackgroundColor = Brushes.AliceBlue;
            CornerRadius = 10;

            //Commands
            UpdateOriginCommand = new RelayCommand(UpdateOrigin);

        }

        public ICommand UpdateOriginCommand { get; set; }
        public void UpdateOrigin(object param)
        {
            Canvas canvas = param as Canvas;
            var p = Mouse.GetPosition(canvas);
            OriginPoint = p;

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
