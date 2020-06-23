using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractal
{
    class ViewModel : INotifyPropertyChanged
    {
        private string _test;

        public string Test
        {
            get { return _test; }
            set { _test = value; OnPropertyChanged("Test"); }
        }

        public ViewModel()
        {
            Test = "Test";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }
    }
}
