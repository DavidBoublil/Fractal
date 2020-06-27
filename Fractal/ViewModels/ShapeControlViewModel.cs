using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Fractal.ViewModels
{
    public class ShapeControlViewModel : INotifyPropertyChanged
    {
        private int _cornerRadius;
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = value;OnPropertyChanged("CornerRadius"); }
        }

        private Brush _backgroundColor;
        public Brush BackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; OnPropertyChanged(("BackgroundColor")); }
        }



        public ShapeControlViewModel()
        {
            BackgroundColor = Brushes.AliceBlue;
            CornerRadius = 10;

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
