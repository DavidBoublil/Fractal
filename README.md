# Fractal visualisation

### An application to visualize fractals that works the followed way:
* First the user will define a shape with 2 or more points wich will be our model to use on each iteration.
* Then we define a closed shape with a variable number of edges defined by the user, on wich we will iterate to create the fractal.
* The algorithm then run between each edge of the shape (define by two adjacents points) and apply the Model's pattern on it.
* The algorithm will run recursively on the shape a variable number of time defined by the user.

The algorithm will, at first, be written in C#. That is to facilitate its usage in the UI app. It will then be rewritten in C++ for efficiency.

### The UI should be simple yet flexible in the possibilities it gives. 
The left side is the Model, the right side the fractal created with it. 
Also Their should be two controllers, one that define the number of recursive iterations of the fractal algorithm,
the second define the number of edges the fractal's shape has at iteration 0 
(after each iteration the number of edges grows exponentially).
The user should be able to add and move points in the Model on the fly just by clicking and draggin on it.

The UI should be written in C# WPF with MVVM architechture.

### Goals of the project
This project is a personal exercise which I hope will help me to make progress in 
my understanding in Computer Vision, Recursive Algorithms and optimizing them, 
building intuitives and useful UIs and, in a more advanced stage, interop between C++ implemention and C#.
