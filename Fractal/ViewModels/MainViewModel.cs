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
            set { _iterations = value; OnPropertyChanged(nameof(Iterations)); }
        }
        public int Vertices
        {
            get { return _vertices; }
            set { _vertices = value; OnPropertyChanged(nameof(Vertices)); }
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
            NPoint iterator = new NPoint(Radius, 0);
            NPoint first = iterator;

            for (int i = Vertices; i > 0; i--, iterator++)
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
            ApplyFractal2(first, ModelShapeControlVm.Shape, Iterations);

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
                        var nX = sc_it.X - seed_copy.X;
                        var nY = sc_it.Y - seed_copy.Y;

                        nX *= scaleFactor;
                        nY *= scaleFactor;

                        // remove it to it's place
                        sc_it.X = sc_it.X + seed_copy.X;
                        sc_it.Y = sc_it.Y + seed_copy.Y;

                        // NX = (X - X1) * SF + X1
                        // NY = (Y - Y1) * SF + Y1
                        sc_it++;
                    } while (sc_it != null && sc_it != seed_copy);

                    // rotate
                    sc_it = seed_copy;
                    do
                    {
                        // move the shape to start from 0,0
                        var nX = sc_it.X - seed_copy.X;
                        var nY = sc_it.Y - seed_copy.Y;

                        var nnX = nX * Math.Cos(angle) - nY * Math.Sin(angle);
                        var nnY = nX * Math.Sin(angle) + nY * Math.Cos(angle);

                        sc_it.X = nnX + seed_copy.X;
                        sc_it.Y = nnY + seed_copy.Y;
                        sc_it++;
                    } while (sc_it != null && sc_it != seed_copy);


                    // insert the points
                    p1.InsertRangeAfter(seed_copy, seed_copy_last.Previous);

                    it++;
                } while (it != null && it.Previous != baseShape);
            }
        }

        private void ApplyFractal2(NPoint baseShape, NPoint seed, int iterations)
        {
            var seed_first_last_dist = seed.DistanceTo(seed.Last());

            // foreach vertice
            for (int i = 0; i < iterations; i++)
            {
                var it = baseShape.Next;
                do
                {
                    var p1 = it.Previous;
                    var p2 = it;
                    var p1_p2_dst = p1.DistanceTo(p2);
                    var seed_copy = seed.GetCopy(false);
                    var seed_copy_last = seed_copy.Last();

                    // move the seed copy to be around 0,0
                    var sc_it = seed_copy;
                    do
                    {
                        sc_it.X -= seed.X;
                        sc_it.Y -= seed.Y;
                        sc_it++;
                    } while (sc_it != null && sc_it != seed_copy);

                    // rescale
                    var scale_factor = p1_p2_dst / seed_first_last_dist;
                    sc_it = seed_copy;
                    do
                    {
                        sc_it.X *= scale_factor;
                        sc_it.Y *= scale_factor;

                        sc_it++;
                    } while (sc_it != null && sc_it != seed_copy);


                    // rotate
                    var pp1 = p1.GetCopy(true);
                    var pp2 = p2.GetCopy(true);

                    pp2.X -= pp1.X;
                    pp2.Y -= pp1.Y;
                    pp1.X = 0;
                    pp1.Y = 0;

                    var theta = Math.Atan2(pp2.Y - pp1.Y, pp2.X - pp1.X);
                   

                    sc_it = seed_copy;
                    var pi = Math.PI;
                    var dg_90 = pi / 2;
                    do
                    {
                        var x = sc_it.X * Math.Cos(theta) - sc_it.Y * Math.Sin(theta);
                        var y = sc_it.X * Math.Sin(theta) + sc_it.Y * Math.Cos(theta);

                        sc_it.X = x;
                        sc_it.Y = y;

                        sc_it++;
                    } while (sc_it != null && sc_it != seed_copy);

                    // remove the seed copy to be around p1
                    sc_it = seed_copy;
                    do
                    {
                        sc_it.X += p1.X;
                        sc_it.Y += p1.Y;
                        sc_it++;
                    } while (sc_it != null && sc_it != seed_copy);

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
            Iterations = 1;

            Test = "Test";

            NPoint shapTest = new NPoint(0, 0);
            NPoint iterator = shapTest;

            var t = new ShapeControlViewModel();
            t.Shape = shapTest;
            t.Scale = 1;
            ShapeShapeControlVm = t;


            // Init shape VM
            NPoint first = new NPoint(0, 0);
            var it = first.AddAfter(new NPoint(30, 0));
            it = it.AddAfter(new NPoint(45, 30));
            it = it.AddAfter(new NPoint(60, 0));
            it = it.AddAfter(new NPoint(90, 0));

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
