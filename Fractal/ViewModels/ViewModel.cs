using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Converters.Models;

namespace Converters.ViewModels
{
    class ViewModel : INotifyPropertyChanged
    {
        private string _test;

        public string Test
        {
            get => _test;
            set { _test = value; OnPropertyChanged("Test"); }
        }

        private ShapeControlViewModel _shapeControlVm;

        public ShapeControlViewModel ShapeControlVm
        {
            get => _shapeControlVm;
            set { _shapeControlVm = value; OnPropertyChanged("ShapeControlVm"); }
        }

        

        public ViewModel()
        {
            Test = "Test";

            Point shapTest = new Point(0,0);
            shapTest.AddAfter(new Point(100,100));
            shapTest.Next.AddAfter(new Point(200,10));

            ShapeControlVm = new ShapeControlViewModel();

            ShapeControlVm.Shape = shapTest;

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
