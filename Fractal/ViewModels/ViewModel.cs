using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractal.ViewModels
{
    class ViewModel : INotifyPropertyChanged
    {
        private string _test;

        public string Test
        {
            get => _test;
            set { _test = value; OnPropertyChanged("Test"); }
        }

        private ShapeControlViewModel _shapeControlVM;

        public ShapeControlViewModel ShapeControlVM
        {
            get => _shapeControlVM;
            set { _shapeControlVM = value; OnPropertyChanged("ShapeControlVM");}
        }

        public ViewModel()
        {
            Test = "Test";
            ShapeControlVM = new ShapeControlViewModel();
        }

        #region INotify boiler plate
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        } 
        #endregion
    }
}
