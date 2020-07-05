using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converters.Models
{
    public static class Fractal
    {
        /// <summary>
        /// Create a fractal on a base shape from a model
        /// </summary>
        public static void CreateFractal(Model model, Point shape, int iteration)
        {
            // Fractal's number of iterations
            for (int i = 0; i < iteration; i++)
            {
                // Support non-closed shapes
                for (Point current = shape.Next; current?.Next != null && current != shape; current++ )
                {
                    // Apply the pattern between the two points
                    model.ApplyModel(current.Previous, current);
                }
            }
        }
    }
}
