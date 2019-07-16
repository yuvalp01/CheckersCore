using System;

namespace CheckersCore
{
    public struct Square
    {

        public Square(int x, int y)
        {
            if ((x + y) % 2 == 0)
            {
                throw new ArgumentException("It is not posible to use white squares");
            }
            if (x > 7 || y > 7)
            {
                throw new IndexOutOfRangeException("Square does not exist on the board");

            }
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
