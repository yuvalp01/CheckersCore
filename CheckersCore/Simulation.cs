using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CheckersCore
{


    public class Simulation
    {


        public List<Movement> LoadSimulation(string path)
        {
            List<Movement> movements = new List<Movement>();
            try
            {
                string[] moveLines = File.ReadAllLines(path);
                foreach (var move in moveLines)
                {
                    List<int> points = move.Split(',').Select(a => Convert.ToInt32(a)).ToList();
                    movements.Add(new Movement(points[0], points[1], points[2], points[3]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error on loading the file exception: {ex.Message}");
            }

            return movements;
        }

    }

    public class Movement
    {
        public Movement(int from_x, int from_y, int to_x, int to_y)
        {
            From = new Square(from_x, from_y);
            To = new Square(to_x, to_y);
        }
        public Square From { get; }
        public Square To { get; }
    }
}


