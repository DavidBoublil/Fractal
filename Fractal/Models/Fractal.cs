using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractal.Models
{
    public static class Fractal
    {
        /// <summary>
        /// Creat a fractal on a base shape from a model
        /// </summary>
        public static void CreateFractal(Model model, Point shape, int iteration)
        {
            // Fractal's number of iterations
            for (int i = 0; i < iteration; i++)
            {
                Point current = shape.Next;
                // Support non-closed shapes
                while (current?.Next !=null && current != shape) 
                {
                    // Aplly the pattern between the two points
                    model.ApplyModel(current.Previous, current);
                    current++;
                }
            }
        }
    }
}
