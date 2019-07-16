using System.Collections.Generic;

namespace CheckersCore
{
    public class Initilizer
    {

        public Dictionary<Square, Player> InitBoard()
        {
            Dictionary<Square, Player> board = new Dictionary<Square, Player>();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    //Don't deploy to white squares
                    if (!((x + y) % 2 == 0))
                    {
                        Square square = new Square(x, y);
                        if (y == 5 || y == 6 || y == 7)
                        {
                            board.Add(square, Player.Black);
                        }
                        else if (y == 0 || y == 1 || y == 2)
                        {
                            board.Add(square, Player.White);
                        }
                        else
                        {
                            board.Add(square, Player.None);
                        }
                    }
                }
            }
            return board;
        }

        public Dictionary<Square, Player> InitEmptyBoard()
        {
            Dictionary<Square, Player> board = new Dictionary<Square, Player>();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    //Don't deploy to white squares
                    if (!((x + y) % 2 == 0))
                    {
                        Square square = new Square(x, y);
                        board.Add(square, Player.None);
                    }
                }
            }
            return board;
        }



    }
}
