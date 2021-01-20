using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Fractals;
using Temp;

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

            // link the last point to the firs to create a close shape
            iterator.AddAfter(first);

            // Apply Fractal
            ApplyFractal(first, ModelShapeControlVm.Shape, Iterations);
            /*var model = new Fractals.Model() { First = ModelShapeControlVm.PointsList.First(), Last = ModelShapeControlVm.PointsList.Last() };
            Fractals.Fractal.CreateFractal(model, first, Iterations);*/

            ShapeShapeControlVm.Shape = first;
        }

        private void ApplyFractal(NPoint baseShape, NPoint seed, int iterations)
        {
            var seed_first_last_dist = seed.DistanceTo(seed.Last()/*check if works*/);
            for (int i = 0; i < iterations; i++)
            {
                NPoint it = baseShape.Next;
                do
                {
                    NPoint p1 = it.Previous;
                    NPoint p2 = it;

                    // get a copy of the seed
                    var seed_copy = seed.GetCopy(false);
                    var seed_copy_last = seed_copy.Last();

                    // calculate usefull data
                    var angle = p1.AngleTo(p2, out double p1_p2_distance);
                    var scaleFactor = p1_p2_distance / seed_first_last_dist;

                    var xDst = p1.X - seed_copy.X;
                    var yDst = p1.Y - seed_copy.Y;
                    // move copy to be around p1
                    //for (var sc_it = seed_copy; sc_it!=null && sc_it.Next != seed_copy; sc_it++)
                    var sc_it = seed_copy;
                    do
                    {
                        sc_it.X += xDst;
                        sc_it.Y += yDst;
                        sc_it++;
                    } while (sc_it != null && sc_it != seed_copy);

                    // rescale seed_copy
                    sc_it = seed_copy;
                    do
                    {
                        // move the shape to start from 0,0
                        sc_it.X -= seed_copy.X;
                        sc_it.Y -= seed_copy.Y;

                        sc_it.X *= scaleFactor;
                        sc_it.Y *= scaleFactor;

                        // remove it to it's place
                        sc_it.X += seed_copy.X;
                        sc_it.Y += seed_copy.Y;

                        // NX = (X - X1) * SF + X1
                        // NY = (Y - Y1) * SF + Y1
                        sc_it++;
                    } while (sc_it != null && sc_it != seed_copy);

                    sc_it = seed_copy;
                    do
                    {
                        // move the shape to start from 0,0
                        sc_it.X -= seed_copy.X;
                        sc_it.Y -= seed_copy.Y;

                        var nX = sc_it.X * Math.Cos(angle) - sc_it.Y * Math.Sin(angle);
                        var nY = sc_it.X * Math.Sin(angle) + sc_it.Y * Math.Cos(angle);

                        sc_it.X = nX + seed_copy.X;
                        sc_it.Y = nY + seed_copy.Y;
                        sc_it++;
                    } while (sc_it != null && sc_it != seed_copy);


                    // insert the points
                    p1.InsertRangeAfter(seed_copy.Next, seed_copy_last.Previous);

                    it++;
                } while (it != null && it.Previous != baseShape);
            }
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

            var t = new ShapeControlViewModel();
            t.Shape = shapTest;
            t.Scale = 0.3;
            ShapeShapeControlVm = t;


            // Init shape VM
            NPoint first = new NPoint(0,0);
            var it = first.AddAfter(new NPoint(30, 60));
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
