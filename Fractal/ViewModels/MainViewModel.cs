using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Converters.Models;

namespace Converters.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private string _test;
        private int _iterations;
        private int _vertices;
        private double _radius;
        private ShapeControlViewModel _shapeShapeShapeShapeControlVm;
        private ShapeControlViewModel _modelShapeControlVm;

        public string Test
        {
            get => _test;
            set { _test = value; OnPropertyChanged("Test"); }
        }
        public ShapeControlViewModel ShapeShapeControlVm
        {
            get => _shapeShapeShapeShapeControlVm;
            set { _shapeShapeShapeShapeControlVm = value; OnPropertyChanged("ShapeShapeControlVm"); }
        }
        public ShapeControlViewModel ModelShapeControlVm
        {
            get => _modelShapeControlVm;
            set { _modelShapeControlVm = value; OnPropertyChanged("ModelShapeControlVm"); }
        }
        public int Iterations
        {
            get { return _iterations; }
            set { _iterations = value; OnPropertyChanged(nameof(Iterations));}
        }
        public int Vertices
        {
            get { return _vertices; }
            set { _vertices = value; OnPropertyChanged(nameof(Vertices));}
        }
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; OnPropertyChanged(nameof(Radius)); }
        }

        // Commands
        public ICommand ApplyCommand { get; set; }

        public void Apply(object param)
        {
            // Create shape with "Vertices" number of vertices
            
            NPoint iterator = new NPoint(Radius,0);
            NPoint first = iterator;

            for (int i = 1; i < Vertices; i++, iterator++)
            {
                double angle = ((2 * Math.PI) / Vertices) * i;
                iterator.AddAfter(new NPoint()
                {
                    X = Radius * Math.Cos(angle),
                    Y = Radius * Math.Sin(angle)
                });
            }

            iterator.AddAfter(first);

            // Apply Fractal
            for (int i = 0; i < Iterations; i++)
            {
                // todo: fractal here

            }

            ShapeShapeControlVm.Shape = first;
        }

        public MainViewModel()
        {
            ApplyCommand = new RelayCommand(Apply);

            // Default values todo: set them from config
            Vertices = 3;
            Radius = 200;

            Test = "Test";

            NPoint shapTest = new NPoint(0, 0);
            NPoint iterator = shapTest;

            int rand = 50000;
            int R = 500;
            Random random = new Random((int)DateTime.Now.Ticks);

            var referenceAngle = Math.Asin(1 / Math.PI);
            for (int i = 0; i < rand;i++)
            {
                // Create point in a ractangle W: PI*R H: R
                double x = random.Next(0, (int)(R * Math.PI));
                double y = random.Next(0, R);

                // Convert the point to an equilateral triangle W: 2*PI*R H: R
                var z = Math.Sqrt(x * x + y * y);
                var angle = Math.Asin(y / z);
               
                if(angle > referenceAngle)
                {
                    x =  Math.PI * R + x;
                    y = R - y;
                }

                // Convert to a circle with radius R
                double b = (R - y) * 2 * Math.PI ;
                var rho = y;
                var theta = (x / b) * 2 * Math.PI - (y - 2 * Math.PI);

                x = Math.Cos(theta) * rho;
                y = Math.Sin(theta) * rho;

                iterator.Next = new NPoint(x, y);
                iterator++;

            }

            var t = new ShapeControlViewModel();
            t.Shape = shapTest;
            t.LinesVisible = false;
            ShapeShapeControlVm = t;


            // Init shape VM
            NPoint first = new NPoint(0,0);
            var it = first.AddAfter(new NPoint(20,0));
            it = it.AddAfter(new NPoint(30, 60));
            it = it.AddAfter(new NPoint(40, 0));
            it = it.AddAfter(new NPoint(60, 0));

            ModelShapeControlVm = new ShapeControlViewModel()
            {
                Shape = first
            };
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
