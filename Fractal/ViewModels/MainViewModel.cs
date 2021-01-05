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

            int rand = 100/2;
            int radius = 500;
            Random random = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < rand;)
            {
                int x = random.Next(-radius, radius);
                int y = random.Next(-radius, radius);
                if (x * x + y * y < radius * radius)
                {
                    iterator.Next = new NPoint(x, y);
                    iterator++;
                    i++;
                }

            }

            var t = new ShapeControlViewModel();
            t.Shape = shapTest;
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
