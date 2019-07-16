using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckersCore
{
    public class Game
    {
        Dictionary<Square, Player> _board;
        public Game(Dictionary<Square, Player> board)
        {
            _board = board;
        }

        public List<KeyValuePair<Square, Player>> GetCapturePossibilities(Player player)
        {
            var currentPlayerCheckers = _board.Where(a => a.Value == player);
            List<KeyValuePair<Square, Player>> capturePossibilities = new List<KeyValuePair<Square, Player>>();
            foreach (var position in currentPlayerCheckers)
            {
                CheckNearSquar(position, 1, ref capturePossibilities);
                CheckNearSquar(position, -1, ref capturePossibilities);
            }
            return capturePossibilities;
        }

        private void CheckNearSquar(KeyValuePair<Square, Player> position, int offsetX, ref List<KeyValuePair<Square, Player>> capturePosibilities)
        {
            Square currentSquare = position.Key;
            Player player = position.Value;
            Player opponent = player == Player.Black ? Player.White : Player.Black;
            int OffsetY= player == Player.White ?  1 : - 1;
            var neighbour = _board.Where(a => a.Key.Y == currentSquare.Y+ OffsetY && a.Key.X == currentSquare.X + offsetX);
            if (neighbour.Count() == 1)
            {
                var nextSquare = neighbour.First();
                if (nextSquare.Value == opponent)
                {
                    var remoteSquare = _board.Where(a => a.Key.Y == currentSquare.Y + 2*OffsetY && a.Key.X == nextSquare.Key.X + offsetX);
                    if (remoteSquare.Count() == 1)
                    {
                        if (remoteSquare.First().Value == Player.None)
                        {
                            capturePosibilities.Add(nextSquare);
                        }
                    }
                }
            }
        }





        public MoveType Move(Player player, Square from, Square to)
        {
            MoveType moveType = ValidateMove(player, from, to);
            switch (moveType)
            {
                case MoveType.Normal:
                    _board[from] = Player.None;
                    _board[to] = player;
                    Console.WriteLine($"{player} checker has completed a normal move from {from} to:{to}");
                    break;
                case MoveType.Capture:
                    _board[from] = Player.None;
                    _board[to] = player;
                    Console.WriteLine($"{player} checker has completed a jump move from {from} to:{to}");
                    //opponent capture
                    int middleX = (from.X + to.X) / 2;
                    int middleY = (from.Y + to.Y) / 2;
                    Square middleSquar = new Square(middleX, middleY);
                    _board[middleSquar] = Player.None;
                    Player opponent = player == Player.White ? Player.Black : Player.White;
                    Console.WriteLine($"{opponent} checker has been captured in {middleSquar.ToString()}");
                    break;
                case MoveType.Invalid:
                    Console.WriteLine($"{player} move from {from} to {to} is invalid");
                    break;
                default:
                    break;
            }
            return moveType;
        }

        public MoveType ValidateMove(Player player, Square from, Square to)
        {
            if (ValidateFeasibility(from, to))
            {
                if (ValidateDirection(player, from, to))
                {
                    MoveType moveType = ValidateLength(player, from, to);
                    if (moveType != MoveType.Invalid)
                    {
                        return moveType;
                    }
                    else
                    {
                        Console.WriteLine("Movement is too lengthy:");
                        return MoveType.Invalid;
                    }

                }
                else
                {
                    Console.WriteLine("Movement possible only foreward:");
                    return MoveType.Invalid;
                }
            }
            else
            {
                Console.WriteLine("Movement is not feasible:");
                return MoveType.Invalid;
            }
        }

        /// <summary>
        /// Validate feasibility on the board
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool ValidateFeasibility(Square from, Square to)
        {
            if (_board[from] != Player.None)
            {
                if (_board[to] == Player.None)
                {
                    return true;
                }
                else
                {
                    //It is impossible to end a move on occupied square
                    return false;
                }
            }
            else
            {
                // There is not player in the original square
                return false;
            }
        }

        public bool ValidateDirection(Player player, Square from, Square to)
        {

            if (from.Y < to.Y)
            {
                if (player == Player.White)
                {
                    return true;
                }
                else if (player == Player.Black)
                {
                    return false;
                }
            }
            else
            {
                if (player == Player.White)
                {
                    return false;
                }
                else if (player == Player.Black)
                {
                    return true;
                }
            }
            return false;
        }

        public MoveType ValidateLength(Player player, Square from, Square to)
        {

            int vertical = Math.Abs(to.Y - from.Y);
            if (vertical == 1 && (from.X == to.X + 1 || from.X == to.X - 1))
            {
                return MoveType.Normal;

            }
            //Capture:
            else if (vertical == 2 && (from.X == to.X + 2 || from.X == to.X - 2))
            {
                return MoveType.Capture;
            }
            else
            {
                return MoveType.Invalid;
            }
        }

    }
}





