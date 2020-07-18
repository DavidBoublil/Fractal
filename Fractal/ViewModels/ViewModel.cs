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

            NPoint shapTest = new NPoint(0, 0);
            NPoint iterator = shapTest;
            
            for (int i = 0; i < 1000; i++, iterator++)
            {
                iterator.Next = new NPoint(i * 4, 50 * Math.Sin(i * 0.01));
            }

            var t = new ShapeControlViewModel();
            t.Shape = shapTest;
            ShapeControlVm = t;
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
